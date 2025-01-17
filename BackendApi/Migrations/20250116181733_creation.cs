using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BackendApi.Migrations
{
    /// <inheritdoc />
    public partial class creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocumentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocumentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserDocumentValue = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserDocumentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserDocumentType_UserDocumentTypeId",
                        column: x => x.UserDocumentTypeId,
                        principalTable: "UserDocumentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message_Sended",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message_Sended", x => new { x.UserId, x.Date, x.Time });
                    table.ForeignKey(
                        name: "FK_Message_Sended_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Message_ToSend",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message_ToSend", x => new { x.UserId, x.Date, x.Time });
                    table.ForeignKey(
                        name: "FK_Message_ToSend_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 1, true, "Devices and gadgets", "Electronics" },
                    { 2, true, "Fiction and non-fiction books", "Books" },
                    { 3, true, "Men's and women's clothing", "Clothing" },
                    { 4, true, "Home appliances and kitchen gadgets", "Home & Kitchen" },
                    { 5, true, "Sports equipment and accessories", "Sports" }
                });

            migrationBuilder.InsertData(
                table: "UserDocumentType",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[,]
                {
                    { 1, true, "Dni" },
                    { 2, true, "Pasaporte" },
                    { 3, true, "Libreta Militar" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "Active", "CategoryId", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, true, 1, "High-performance laptop", "Laptop", 999.99m },
                    { 2L, true, 1, "Latest model smartphone", "Smartphone", 699.99m },
                    { 3L, true, 1, "Noise cancelling headphones", "Headphones", 199.99m },
                    { 4L, true, 1, "Wireless mouse", "Mouse", 29.99m },
                    { 5L, true, 1, "Mechanical keyboard", "Keyboard", 89.99m },
                    { 6L, true, 2, "Award-winning fiction novel", "Novel", 15.99m },
                    { 7L, true, 2, "Biography of a famous scientist", "Biography", 19.99m },
                    { 8L, true, 2, "A collection of gourmet recipes", "Cookbook", 35.99m },
                    { 9L, true, 2, "Detailed history of the 20th century", "History", 40.99m },
                    { 10L, true, 2, "Sci-fi thriller set in space", "Science Fiction", 14.99m },
                    { 11L, true, 3, "Cotton t-shirt", "T-shirt", 20.99m },
                    { 12L, true, 3, "Comfortable blue jeans", "Jeans", 45.99m },
                    { 13L, true, 3, "Leather jacket", "Jacket", 89.99m },
                    { 14L, true, 3, "Running sneakers", "Sneakers", 50.99m },
                    { 15L, true, 3, "Woolen scarf", "Scarf", 19.99m },
                    { 16L, true, 4, "Multi-function blender", "Blender", 59.99m },
                    { 17L, true, 4, "Compact microwave", "Microwave", 99.99m },
                    { 18L, true, 4, "Stainless steel toaster", "Toaster", 29.99m },
                    { 19L, true, 4, "Espresso coffee maker", "Coffee Maker", 120.99m },
                    { 20L, true, 4, "Energy efficient dishwasher", "Dishwasher", 399.99m },
                    { 21L, true, 5, "Professional football", "Football", 25.99m },
                    { 22L, true, 5, "Carbon fiber tennis racket", "Tennis Racket", 115.99m },
                    { 23L, true, 5, "Official size basketball", "Basketball", 29.99m },
                    { 24L, true, 5, "Set of golf clubs", "Golf Clubs", 460.99m },
                    { 25L, true, 5, "Aluminum baseball bat", "Baseball Bat", 89.99m }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Name", "Password", "UserDocumentTypeId", "UserDocumentValue" },
                values: new object[,]
                {
                    { 1L, "john.doe@email.com", "John Doe", "123", 1, "01123456" },
                    { 2L, "jane.smith@email.com", "Jane Smith", "123", 1, "02123456" },
                    { 3L, "alice.johnson@email.com", "Alice Johnson", "123", 1, "03234567" },
                    { 4L, "bob.brown@email.com", "Bob Brown", "123", 1, "04234567" },
                    { 5L, "carol.white@email.com", "Carol White", "123", 1, "05234567" },
                    { 6L, "dave.black@email.com", "Dave Black", "123", 1, "06234567" },
                    { 7L, "eve.green@email.com", "Eve Green", "123", 1, "07234567" },
                    { 8L, "frank.gray@email.com", "Frank Gray", "123", 1, "08234567" },
                    { 9L, "grace.blue@email.com", "Grace Blue", "123", 1, "09234567" },
                    { 10L, "henry@email.com", "Henry", "123", 1, "10234567" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserDocumentTypeId",
                table: "User",
                column: "UserDocumentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message_Sended");

            migrationBuilder.DropTable(
                name: "Message_ToSend");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "UserDocumentType");
        }
    }
}
