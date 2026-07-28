using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bidding_zone_api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EndTimer = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartingPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    WinnerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Address_StreetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_Surbub = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Address_HouseNumber = table.Column<int>(type: "integer", nullable: true),
                    Address_Province = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Bids",
                columns: new[] { "Id", "CreatedAt", "ItemId", "Price", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e001"), new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d001"), 160.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e002"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d001"), 170.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e003"), new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d002"), 310.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e004"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d003"), 820.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c005") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e005"), new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d004"), 130.00m, 1, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e006"), new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d005"), 260.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e007"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d006"), 410.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e008"), new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d007"), 95.00m, 3, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c005") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e009"), new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d008"), 210.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e010"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d009"), 510.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e011"), new DateTime(2026, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d010"), 360.00m, 1, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e012"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d011"), 115.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c005") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e013"), new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d012"), 45.00m, 3, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e014"), new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d013"), 75.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e015"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d014"), 610.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002") },
                    { new Guid("ac4aab6e-2d5f-4d84-a3e4-6b4d0cb8e016"), new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d015"), 90.00m, 2, null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004") }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "CreatedAt", "Description", "EndTimer", "Image", "StartingPrice", "Status", "Title", "UpdatedAt", "UserId", "WinnerUserId" },
                values: new object[,]
                {
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d001"), new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), "A well preserved antique wooden chair.", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Utc), "", 150.00m, 0, "Antique Wooden Chair", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d002"), new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Classic film camera in working condition.", new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), "", 300.00m, 0, "Vintage Camera", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d003"), new DateTime(2026, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Lightly used mountain bike, great condition.", new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), "", 800.00m, 0, "Mountain Bike", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d004"), new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Genuine leather jacket, size medium.", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", 120.00m, 1, "Leather Jacket", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d005"), new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Six string acoustic guitar with case.", new DateTime(2026, 8, 12, 0, 0, 0, 0, DateTimeKind.Utc), "", 250.00m, 0, "Acoustic Guitar", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d006"), new DateTime(2026, 7, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Latest generation gaming console, barely used.", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), "", 400.00m, 0, "Gaming Console", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d007"), new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Modern glass top coffee table.", new DateTime(2026, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), "", 90.00m, 2, "Coffee Table", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d008"), new DateTime(2026, 7, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Sturdy wooden office desk with drawers.", new DateTime(2026, 8, 6, 0, 0, 0, 0, DateTimeKind.Utc), "", 200.00m, 0, "Office Desk", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d009"), new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Utc), "Unlocked smartphone, excellent condition.", new DateTime(2026, 8, 9, 0, 0, 0, 0, DateTimeKind.Utc), "", 500.00m, 0, "Smartphone", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d010"), new DateTime(2026, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Elegant stainless steel wristwatch.", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc), "", 350.00m, 1, "Wristwatch", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d011"), new DateTime(2026, 7, 27, 0, 0, 0, 0, DateTimeKind.Utc), "Tall wooden bookshelf with five shelves.", new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Utc), "", 110.00m, 0, "Bookshelf", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d012"), new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Stainless steel electric kettle, fast boil.", new DateTime(2026, 7, 29, 0, 0, 0, 0, DateTimeKind.Utc), "", 40.00m, 2, "Electric Kettle", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d013"), new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Well maintained skateboard, ready to ride.", new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), "", 70.00m, 0, "Skateboard", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d014"), new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Original oil painting, framed.", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Utc), "", 600.00m, 0, "Painting", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003"), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9c4aab6e-2d5f-4d84-a3e4-6b4d0cb8d015"), new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Complete set of gardening tools.", new DateTime(2026, 8, 4, 0, 0, 0, 0, DateTimeKind.Utc), "", 85.00m, 0, "Garden Tools Set", null, new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003"), new Guid("00000000-0000-0000-0000-000000000000") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "Gender", "LastName", "Password", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c001"), null, "ja@gmail.com", "jack", 0, "phiri", "$2a$11$Gdb7jhHcS3IG96XZX1Aw/O169GIvyQWA4LmqkBt1G01AuMhSGKnWG", 0, null },
                    { new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c002"), null, "lewis@gmail.com", "lewis", 0, "moyo", "$2a$11$Gdb7jhHcS3IG96XZX1Aw/O169GIvyQWA4LmqkBt1G01AuMhSGKnWG", 0, null },
                    { new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c003"), null, "taza@gmail.com", "taza", 0, "thousand", "$2a$11$Gdb7jhHcS3IG96XZX1Aw/O169GIvyQWA4LmqkBt1G01AuMhSGKnWG", 0, null },
                    { new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c004"), null, "admin@gmail.com", "admin", 0, "admin", "$2a$11$Gdb7jhHcS3IG96XZX1Aw/O169GIvyQWA4LmqkBt1G01AuMhSGKnWG", 0, null },
                    { new Guid("8c4aab6e-2d5f-4d84-a3e4-6b4d0cb8c005"), null, "ngoni@gmail.com", "ngoni", 0, "mathews", "$2a$11$Gdb7jhHcS3IG96XZX1Aw/O169GIvyQWA4LmqkBt1G01AuMhSGKnWG", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
