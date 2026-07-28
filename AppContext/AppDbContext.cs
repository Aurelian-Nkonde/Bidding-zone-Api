

using bidding_zone_api.Models;
using bidding_zone_api.Utils;
using Microsoft.EntityFrameworkCore;

namespace bidding_zone_api.AppContext;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {}

    public DbSet<User> Users {get;set;}
    public DbSet<Item> Items {get;set;}
    public DbSet<Bid> Bids {get;set;}
    public DbSet<Notification> Notifications {get;set;}

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(prop => prop.Id);
            entity.Property(prop => prop.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(prop => prop.LastName).IsRequired().HasMaxLength(100);
            entity.Property(prop => prop.Email).IsRequired();
            entity.Property(prop => prop.Password).IsRequired().HasMaxLength(100);
            entity.OwnsOne(prop => prop.Address,address =>
            {
                address.Property(prop => prop.HouseNumber).IsRequired();
                address.Property(prop => prop.Province).IsRequired();
                address.Property(prop => prop.StreetName).IsRequired().HasMaxLength(100);
                address.Property(prop => prop.Surbub).IsRequired().HasMaxLength(100);
            });
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.HasKey(prop => prop.Id);
            entity.Property(prop => prop.ItemId).IsRequired();
            entity.Property(prop => prop.UserId).IsRequired();
            entity.Property(prop => prop.Price).IsRequired();
            entity.Property(prop => prop.Status).IsRequired();

        });

        modelBuilder.Entity<Item>(entity =>
        {
           entity.HasKey(prop => prop.Id);
           entity.Property(prop => prop.UserId).IsRequired();
           entity.Property(prop => prop.EndTimer).IsRequired(); 
           entity.Property(prop => prop.StartingPrice).IsRequired(); 
           entity.Property(prop => prop.WinnerUserId).IsRequired(); 
           entity.Property(prop => prop.Title).IsRequired(); 
           entity.Property(prop => prop.Description).IsRequired(); 
           entity.Property(prop => prop.Status).IsRequired(); 

        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(prop => prop.Id);
            entity.Property(prop => prop.UserId).IsRequired();
            entity.Property(prop => prop.Title).IsRequired();            
            entity.Property(prop => prop.Message).IsRequired();            
            entity.Property(prop => prop.IsRead).IsRequired();            

        });

        var hashedPassword = "$2a$11$Gdb7jhHcS3IG96XZX1Aw/O169GIvyQWA4LmqkBt1G01AuMhSGKnWG";

        modelBuilder.Entity<User>().HasData(
            new User{Id = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), FirstName = "jack", LastName = "phiri", Email ="ja@gmail.com", Password =  hashedPassword},
            new User{Id = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), FirstName = "lewis", LastName = "moyo", Email ="lewis@gmail.com", Password = hashedPassword},
            new User{Id = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003"), FirstName = "taza", LastName = "thousand", Email ="taza@gmail.com", Password = hashedPassword},
            new User{Id = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004"), FirstName = "admin", LastName = "admin", Email ="admin@gmail.com", Password = hashedPassword},
            new User{Id = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c005"), FirstName = "ngoni", LastName = "mathews", Email ="ngoni@gmail.com", Password = hashedPassword}
        );

        // Id variables
        var user1 = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001");
        var user2 = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002");
        var user3 = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003");
        var user4 = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004");
        var user5 = Guid.Parse("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c005");

        var item1 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d001");
        var item2 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d002");
        var item3 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d003");
        var item4 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d004");
        var item5 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d005");
        var item6 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d006");
        var item7 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d007");
        var item8 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d008");
        var item9 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d009");
        var item10 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d010");
        var item11 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d011");
        var item12 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d012");
        var item13 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d013");
        var item14 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d014");
        var item15 = Guid.Parse("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d015");

        modelBuilder.Entity<Item>().HasData(
            // user1 - 7 items
            new Item{Id = item1, UserId = user1, Title = "Antique Wooden Chair", Description = "A well preserved antique wooden chair.", StartingPrice = 150.00m, EndTimer = new DateTime(2026, 8, 3, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item2, UserId = user1, Title = "Vintage Camera", Description = "Classic film camera in working condition.", StartingPrice = 300.00m, EndTimer = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 22, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item3, UserId = user1, Title = "Mountain Bike", Description = "Lightly used mountain bike, great condition.", StartingPrice = 800.00m, EndTimer = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item4, UserId = user1, Title = "Leather Jacket", Description = "Genuine leather jacket, size medium.", StartingPrice = 120.00m, EndTimer = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Sold, WinnerUserId = user2, CreatedAt = new DateTime(2026, 7, 18, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item5, UserId = user1, Title = "Acoustic Guitar", Description = "Six string acoustic guitar with case.", StartingPrice = 250.00m, EndTimer = new DateTime(2026, 8, 12, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item6, UserId = user1, Title = "Gaming Console", Description = "Latest generation gaming console, barely used.", StartingPrice = 400.00m, EndTimer = new DateTime(2026, 8, 15, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 26, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item7, UserId = user1, Title = "Coffee Table", Description = "Modern glass top coffee table.", StartingPrice = 90.00m, EndTimer = new DateTime(2026, 7, 30, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Canceled, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc)},

            // user2 - 5 items
            new Item{Id = item8, UserId = user2, Title = "Office Desk", Description = "Sturdy wooden office desk with drawers.", StartingPrice = 200.00m, EndTimer = new DateTime(2026, 8, 6, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 21, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item9, UserId = user2, Title = "Smartphone", Description = "Unlocked smartphone, excellent condition.", StartingPrice = 500.00m, EndTimer = new DateTime(2026, 8, 9, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 23, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item10, UserId = user2, Title = "Wristwatch", Description = "Elegant stainless steel wristwatch.", StartingPrice = 350.00m, EndTimer = new DateTime(2026, 8, 2, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Sold, WinnerUserId = user1, CreatedAt = new DateTime(2026, 7, 19, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item11, UserId = user2, Title = "Bookshelf", Description = "Tall wooden bookshelf with five shelves.", StartingPrice = 110.00m, EndTimer = new DateTime(2026, 8, 14, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item12, UserId = user2, Title = "Electric Kettle", Description = "Stainless steel electric kettle, fast boil.", StartingPrice = 40.00m, EndTimer = new DateTime(2026, 7, 29, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Canceled, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 14, 0, 0, 0, DateTimeKind.Utc)},

            // user3 - 3 items
            new Item{Id = item13, UserId = user3, Title = "Skateboard", Description = "Well maintained skateboard, ready to ride.", StartingPrice = 70.00m, EndTimer = new DateTime(2026, 8, 8, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 22, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item14, UserId = user3, Title = "Painting", Description = "Original oil painting, framed.", StartingPrice = 600.00m, EndTimer = new DateTime(2026, 8, 20, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc)},
            new Item{Id = item15, UserId = user3, Title = "Garden Tools Set", Description = "Complete set of gardening tools.", StartingPrice = 85.00m, EndTimer = new DateTime(2026, 8, 4, 0, 0, 0, DateTimeKind.Utc), Status = ItemStatus.Active, WinnerUserId = Guid.Empty, CreatedAt = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc)}
        );

        modelBuilder.Entity<Bid>().HasData(
            // bids on user1's items (bidders: user2, user3, user4, user5 - never the item owner)
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e001"), ItemId = item1, UserId = user2, Price = 160.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 26, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e002"), ItemId = item1, UserId = user4, Price = 170.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e003"), ItemId = item2, UserId = user3, Price = 310.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 26, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e004"), ItemId = item3, UserId = user5, Price = 820.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e005"), ItemId = item4, UserId = user2, Price = 130.00m, Status = BidStatus.Accepted, CreatedAt = new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e006"), ItemId = item5, UserId = user4, Price = 260.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 26, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e007"), ItemId = item6, UserId = user3, Price = 410.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e008"), ItemId = item7, UserId = user5, Price = 95.00m, Status = BidStatus.Canceled, CreatedAt = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc)},

            // bids on user2's items (bidders: user1, user3, user4, user5 - never the item owner)
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e009"), ItemId = item8, UserId = user1, Price = 210.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 26, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e010"), ItemId = item9, UserId = user3, Price = 510.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e011"), ItemId = item10, UserId = user1, Price = 360.00m, Status = BidStatus.Accepted, CreatedAt = new DateTime(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e012"), ItemId = item11, UserId = user5, Price = 115.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e013"), ItemId = item12, UserId = user4, Price = 45.00m, Status = BidStatus.Canceled, CreatedAt = new DateTime(2026, 7, 19, 0, 0, 0, DateTimeKind.Utc)},

            // bids on user3's items (bidders: user1, user2, user4, user5 - never the item owner)
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e014"), ItemId = item13, UserId = user1, Price = 75.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 26, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e015"), ItemId = item14, UserId = user2, Price = 610.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 27, 0, 0, 0, DateTimeKind.Utc)},
            new Bid{Id = Guid.Parse("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e016"), ItemId = item15, UserId = user4, Price = 90.00m, Status = BidStatus.Active, CreatedAt = new DateTime(2026, 7, 25, 0, 0, 0, DateTimeKind.Utc)}
        );
    }
}