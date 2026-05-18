using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditShipmnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbUserSebders",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippmentStatus_TbShippments",
                table: "TbShippmentStatus");

            migrationBuilder.DropTable(
                name: "TbUserSebders");

            migrationBuilder.AddColumn<string>(
                name: "AddressDetails",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "TbUserReceivers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())")
                .Annotation("Relational:DefaultConstraintName", "DF_TbShipments_Id")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_TbShippments_Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "TbShippments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingPackagingId",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TbShippingPackagings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ShippingPackagingAname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingPackagingEname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingPackagings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbUserSenders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())")
                        .Annotation("Relational:DefaultConstraintName", "DF_TbUserSebders_Id"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AddressDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserSenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserSebders_TbCities",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbUserSenders_TbCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "TbCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbUserReceivers_CountryId",
                table: "TbUserReceivers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ShippingPackagingId",
                table: "TbShippments",
                column: "ShippingPackagingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSenders_CityId",
                table: "TbUserSenders",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSenders_CountryId",
                table: "TbUserSenders",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShippingPackagings_ShippingPackagingId",
                table: "TbShippments",
                column: "ShippingPackagingId",
                principalTable: "TbShippingPackagings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbUserSenders",
                table: "TbShippments",
                column: "SenderId",
                principalTable: "TbUserSenders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippmentStatus_TbShipments",
                table: "TbShippmentStatus",
                column: "ShippmentId",
                principalTable: "TbShippments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserReceivers_TbCountries_CountryId",
                table: "TbUserReceivers",
                column: "CountryId",
                principalTable: "TbCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbShippingPackagings_ShippingPackagingId",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbUserSenders",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippmentStatus_TbShipments",
                table: "TbShippmentStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserReceivers_TbCountries_CountryId",
                table: "TbUserReceivers");

            migrationBuilder.DropTable(
                name: "TbShippingPackagings");

            migrationBuilder.DropTable(
                name: "TbUserSenders");

            migrationBuilder.DropIndex(
                name: "IX_TbUserReceivers_CountryId",
                table: "TbUserReceivers");

            migrationBuilder.DropIndex(
                name: "IX_TbShippments_ShippingPackagingId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "AddressDetails",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "ShippingPackagingId",
                table: "TbShippments");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())")
                .Annotation("Relational:DefaultConstraintName", "DF_TbShippments_Id")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_TbShipments_Id");

            migrationBuilder.CreateTable(
                name: "TbUserSebders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())")
                        .Annotation("Relational:DefaultConstraintName", "DF_TbUserSebders_Id"),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbUserSebders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbUserSebders_TbCities",
                        column: x => x.CityId,
                        principalTable: "TbCities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbUserSebders_CityId",
                table: "TbUserSebders",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbUserSebders",
                table: "TbShippments",
                column: "SenderId",
                principalTable: "TbUserSebders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippmentStatus_TbShippments",
                table: "TbShippmentStatus",
                column: "ShippmentId",
                principalTable: "TbShippments",
                principalColumn: "Id");
        }
    }
}
