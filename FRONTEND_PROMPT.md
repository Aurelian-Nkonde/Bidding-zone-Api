I have an ASP.NET Core backend for a bidding-zone marketplace app (Users, Items, Bids). Build a clean, minimal skeleton frontend for it as a Next.js (App Router) + Tailwind + shadcn/ui project. This is a backend-heavy project — keep the UI as plain and simple as possible: shadcn `Card`, `Badge`, `Button`, `Input`, `Select`, `Table`, `Dialog` (for confirmations/small forms) where it genuinely helps, no dashboards, no dense data grids, no heavy custom styling or animation. Neutral/gray palette with one accent color for primary actions and status badges.

## Known backend quirks — read carefully, these are not optional details

1. **Enums are asymmetric in JSON.** Request bodies take enum fields as **strings** (e.g. `"gender": "Male"`, `"status": "Active"`), matched case-insensitively against the C# enum member name. But **response** bodies serialize enums as their **numeric ordinal** (no `JsonStringEnumConverter` is registered backend-side), e.g. a `Sold` item comes back as `"status": 1`, not `"status": "Sold"`. You must write number→label maps for display and reuse the same maps for filtering, matching this exact ordinal order:
   - `Gender`: `0 = Male`, `1 = Female`
   - `Province`: `0 = Western_cape`, `1 = Northen_cape`, `2 = Mpumalanga`, `3 = Limpopo`, `4 = Gauteng`, `5 = Kwazulu_natal`, `6 = North_west`, `7 = Eastern_cape`
   - `ItemStatus`: `0 = Active`, `1 = Sold`, `2 = Canceled`
   - `BidStatus`: `0 = Rejected`, `1 = Accepted`, `2 = Active`, `3 = Canceled`, `4 = Closed`

   Keep request-side string values (`"Active"`, `"Female"`, etc.) and response-side numeric parsing in one shared module (`lib/enums.ts`) so this asymmetry only has to be handled once.

2. **No CORS is configured on the backend.** Calling the API directly from the browser on a different origin/port will fail. Add a Next.js rewrite in `next.config.ts` (`/api/backend/:path*` → `${API_URL}/api/:path*`) and call through that same-origin path from the browser, rather than hitting the backend URL directly client-side. Mention this tradeoff in a code comment so it's easy to rip out once the backend adds real CORS.

3. **No real authentication exists on the backend yet.** `POST /api/Users` hashes the password with BCrypt and stores it, but there is no login endpoint, no token issuance, and no `[Authorize]` guards anywhere — every mutating endpoint currently just uses a hardcoded fake user id server-side. Build **mock session auth** entirely client-side as a stand-in, structured so it's easy to swap for real JWT auth later:
   - `/signup` — real form hitting `POST /api/Users` (first/last name, email, password, gender, optional address).
   - `/login` — form with email + password fields for realism, but since there's no password-verification endpoint, actually authenticate by calling `GET /api/Users`, finding the user with a matching email, and — if found — treating that as a successful login (do **not** attempt to check the password client-side; add a short comment noting this is a placeholder until the backend exposes a real `/api/Auth/login`).
   - On success, store the matched `User` as the session in a React context (`AuthProvider`) backed by `localStorage`, exposed via a `useAuth()` hook (`user`, `login()`, `logout()`, `isAuthenticated`).
   - Route protection: wrap authenticated-only pages/actions (add item, place bid, my-items, my-bids, profile, editing/canceling) in a client-side check that redirects to `/login` if there's no session — this is "authorization" in the sense of gating UI actions to the owning user (e.g. only the item's owner sees Edit/Cancel on it, only a bid's owner sees Cancel on it), not real server-enforced authorization, since the backend doesn't enforce it either. Note this limitation in a comment near the auth provider.
   - Header shows a user menu (shadcn `DropdownMenu`) with Profile / Logout when authenticated, or Login / Sign up buttons when not.

## API base

Base URL: `process.env.NEXT_PUBLIC_API_URL` (e.g. `http://localhost:5074`), consumed through the Next.js rewrite described above. Create a small typed fetch client (`lib/api.ts`) with one function per endpoint below, throwing on non-2xx with the response status attached.

## Data model (TypeScript types)

```ts
type Gender = "Male" | "Female";
type Province = "Western_cape" | "Northen_cape" | "Mpumalanga" | "Limpopo" | "Gauteng" | "Kwazulu_natal" | "North_west" | "Eastern_cape";
type ItemStatus = "Active" | "Sold" | "Canceled";
type BidStatus = "Rejected" | "Accepted" | "Active" | "Canceled" | "Closed";
type BiddingTime = "Minutes_5" | "Minutes_10" | "Minutes_30" | "Hours_1"; // duration options when creating an item

interface Address {
  streetName: string;
  surbub: string; // note: backend spelling, keep as-is
  houseNumber: number;
  province: Province;
}

interface User {
  id: string;
  firstName: string;
  lastName: string;
  gender: Gender;
  email: string;
  address: Address | null;
  createdAt: string | null;
  updatedAt: string | null;
}

interface Item {
  id: string;
  userId: string; // owner
  endTimer: string; // ISO datetime, the auction's actual end time (only on responses)
  startingPrice: number;
  winnerUserId: string | null;
  title: string;
  description: string;
  image: string | null;
  status: ItemStatus;
  createdAt: string | null;
  updatedAt: string | null;
}

interface Bid {
  id: string;
  itemId: string;
  userId: string;
  price: number;
  status: BidStatus;
  createdAt: string | null;
}
```

Remember: on the wire, `gender`/`status`/`province` in **responses** are numbers per the map in the quirks section above — decode them into the string union types above at the API-client boundary so the rest of the app only ever sees the friendly string types.

## API endpoints (exact routes and payloads)

**Users**
- `GET /api/Users` → `User[]`
- `GET /api/Users/{id}` → `User` (404 if missing)
- `POST /api/Users` → body `{ firstName, lastName, password, gender: Gender, email, address?: Address }` → 201 `User`
- `PUT /api/Users/update/{id}` → body `{ firstName, lastName, gender: Gender, email, address?: Address }` → 204
- `PUT /api/Users/update/address/{id}` → body `Address` → 204

**Items**
- `GET /api/Items` → `Item[]`
- `GET /api/Items/{id}` → `Item` (404 if missing)
- `POST /api/Items` → body `{ userId, endTimer: BiddingTime, startingPrice, title, description, image? }` → 201 `Item`. Note: on **create**, `endTimer` is a duration enum (how long from now the auction runs) — a select of the 4 options, not a date picker. The response's `endTimer` is a computed ISO datetime.
- `PUT /api/Items/update/{id}` → body `{ title, description, startingPrice, endTimer: BiddingTime }` → 204. Standard PUT, not GET — ignore any earlier notes claiming otherwise.
- `PUT /api/Items/update/{id}/status` → body `{ status: ItemStatus }` → 204
- There is **no** endpoint to fetch items by user. For "My Items", fetch `GET /api/Items` and filter client-side by `item.userId === user.id`.

**Bids**
- `GET /api/Bids` → `Bid[]`
- `GET /api/Bids/{id}` → `Bid`
- `GET /api/Bids/user/{id}` → intended to return one user's bids, but currently returns **all** bids (backend bug). Treat it the same as `GET /api/Bids` and filter client-side by `bid.userId === id`.
- `POST /api/Bids` → body `{ itemId, userId, price }` → 201 `Bid`. Status is set server-side; don't send it.
- `PUT /api/Bids/status/{id}` → body `{ status: BidStatus }` → 204. Use this to implement "cancel a bid" (`status: "Canceled"`).
- `DELETE /api/Bids/{id}` → 204, permanently removes a bid.
- Do **not** build an "edit bid price" feature — `PUT /api/Bids/{id}` is currently broken server-side (it just creates a new bid instead of updating one). Skip it. For "cancel", prefer the status-update endpoint over delete so the bid stays visible in history.

## Pages to build

1. `/` — Marketplace grid of all items. Cards show title, image (fallback placeholder), starting price, status badge, live countdown to `endTimer`. Click through to detail. No auth required to browse.
2. `/items/[id]` — Item detail: full info, owner, current highest active bid, list of all bids on the item (fetch `GET /api/Bids`, filter by `itemId`, sort by price desc), and — if logged in and not the owner and the item is `Active` — a "Place a bid" form posting to `POST /api/Bids`.
3. `/items/new` — Create-item form (requires login), matching `POST /api/Items`, `endTimer` as a `Select` of the 4 duration options.
4. `/items/[id]/edit` — Edit form (title/description/price/duration → `PUT /api/Items/update/{id}`) plus a status changer (Active/Sold/Canceled → `PUT /api/Items/update/{id}/status`). Only reachable/visible to the item's owner.
5. `/profile` — The logged-in user's own profile. Clean single page combining: profile info + edit form (name/email/gender/address → the two user PUT endpoints), a "My Items" section (tabs or filter chips: Active / Sold / Canceled, each item card has Edit and Cancel buttons — Cancel sets status to Canceled via the status endpoint, behind a confirm `Dialog`), and a "My Bids" section (tabs: Active / Accepted / Rejected / Canceled / Closed, each bid card has a Cancel button — behind a confirm `Dialog`, using the bid status endpoint). Keep it simple: this is the one page that shows the most information, so lean on shadcn `Tabs` and plain lists rather than a dense table.
6. `/users` — Simple directory of all users (`GET /api/Users`): name, email, a couple of address fields. Read-only list, links to nothing sensitive (no other user's profile page — only the logged-in user can see their own via `/profile`).
7. `/login`, `/signup` — as described in the auth section above.

A persistent header/nav (all pages): app name, links to Home / Users, "+ Add item" button (only visible when logged in, links to `/items/new`), and the auth user menu on the right.

## Explicitly out of scope for this skeleton

- Real password verification / JWT / server-enforced authorization (see quirks section — backend doesn't support it yet).
- Editing bid price.
- Any notification UI (a `Notification` model exists backend-side but there's no controller for it yet).
- Image upload — treat `image` as a plain URL text input.
