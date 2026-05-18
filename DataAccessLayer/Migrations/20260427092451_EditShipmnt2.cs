using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditShipmnt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbPaymentMethods",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbShippingPackagings_ShippingPackagingId",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbShippingTypes",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbUserReceivers",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbUserSenders",
                table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSebders_TbCities",
                table: "TbUserSenders");

            migrationBuilder.DropTable(
                name: "TbShippmentStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbShippments",
                table: "TbShippments");

            migrationBuilder.RenameTable(
                name: "TbShippments",
                newName: "TbShipments");

            migrationBuilder.RenameIndex(
                name: "IX_TbShippments_ShippingTypeId",
                table: "TbShipments",
                newName: "IX_TbShipments_ShippingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShippments_ShippingPackagingId",
                table: "TbShipments",
                newName: "IX_TbShipments_ShippingPackagingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShippments_SenderId",
                table: "TbShipments",
                newName: "IX_TbShipments_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShippments_ReceiverId",
                table: "TbShipments",
                newName: "IX_TbShipments_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShippments_PaymentMethodId",
                table: "TbShipments",
                newName: "IX_TbShipments_PaymentMethodId");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TbUserSenders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())")
                .Annotation("Relational:DefaultConstraintName", "DF_TbUserSenders_Id")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_TbUserSebders_Id");

            migrationBuilder.AlterColumn<string>(
                name: "TrackingNumber",
                table: "TbShipments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbShipments",
                table: "TbShipments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbShipmentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())")
                        .Annotation("Relational:DefaultConstraintName", "DF_TbShipmentStatus_Id"),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShipmentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShipmentStatus_TbCarriers",
                        column: x => x.CarrierId,
                        principalTable: "TbCarriers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShipmentStatus_TbShipments",
                        column: x => x.ShipmentId,
                        principalTable: "TbShipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatus_CarrierId",
                table: "TbShipmentStatus",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShipmentStatus_ShipmentId",
                table: "TbShipmentStatus",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipments_TbPaymentMethods",
                table: "TbShipments",
                column: "PaymentMethodId",
                principalTable: "TbPaymentMethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipments_TbShippingPackagings_ShippingPackagingId",
                table: "TbShipments",
                column: "ShippingPackagingId",
                principalTable: "TbShippingPackagings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipments_TbShippingTypes",
                table: "TbShipments",
                column: "ShippingTypeId",
                principalTable: "TbShippingTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipments_TbUserReceivers",
                table: "TbShipments",
                column: "ReceiverId",
                principalTable: "TbUserReceivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShipments_TbUserSenders",
                table: "TbShipments",
                column: "SenderId",
                principalTable: "TbUserSenders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSenders_TbCities",
                table: "TbUserSenders",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShipments_TbPaymentMethods",
                table: "TbShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShipments_TbShippingPackagings_ShippingPackagingId",
                table: "TbShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShipments_TbShippingTypes",
                table: "TbShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShipments_TbUserReceivers",
                table: "TbShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShipments_TbUserSenders",
                table: "TbShipments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbUserSenders_TbCities",
                table: "TbUserSenders");

            migrationBuilder.DropTable(
                name: "TbShipmentStatus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbShipments",
                table: "TbShipments");

            migrationBuilder.RenameTable(
                name: "TbShipments",
                newName: "TbShippments");

            migrationBuilder.RenameIndex(
                name: "IX_TbShipments_ShippingTypeId",
                table: "TbShippments",
                newName: "IX_TbShippments_ShippingTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShipments_ShippingPackagingId",
                table: "TbShippments",
                newName: "IX_TbShippments_ShippingPackagingId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShipments_SenderId",
                table: "TbShippments",
                newName: "IX_TbShippments_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShipments_ReceiverId",
                table: "TbShippments",
                newName: "IX_TbShippments_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_TbShipments_PaymentMethodId",
                table: "TbShippments",
                newName: "IX_TbShippments_PaymentMethodId");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Contact",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TbUserSenders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "(newid())")
                .Annotation("Relational:DefaultConstraintName", "DF_TbUserSebders_Id")
                .OldAnnotation("Relational:DefaultConstraintName", "DF_TbUserSenders_Id");

            migrationBuilder.AlterColumn<double>(
                name: "TrackingNumber",
                table: "TbShippments",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbShippments",
                table: "TbShippments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TbShippmentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())")
                        .Annotation("Relational:DefaultConstraintName", "DF_TbShippmentStatus_Id"),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippmentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbShippmentStatus_TbCarriers",
                        column: x => x.CarrierId,
                        principalTable: "TbCarriers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TbShippmentStatus_TbShipments",
                        column: x => x.ShippmentId,
                        principalTable: "TbShippments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShippmentStatus_CarrierId",
                table: "TbShippmentStatus",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippmentStatus_ShippmentId",
                table: "TbShippmentStatus",
                column: "ShippmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbPaymentMethods",
                table: "TbShippments",
                column: "PaymentMethodId",
                principalTable: "TbPaymentMethods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShippingPackagings_ShippingPackagingId",
                table: "TbShippments",
                column: "ShippingPackagingId",
                principalTable: "TbShippingPackagings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShippingTypes",
                table: "TbShippments",
                column: "ShippingTypeId",
                principalTable: "TbShippingTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbUserReceivers",
                table: "TbShippments",
                column: "ReceiverId",
                principalTable: "TbUserReceivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbUserSenders",
                table: "TbShippments",
                column: "SenderId",
                principalTable: "TbUserSenders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbUserSebders_TbCities",
                table: "TbUserSenders",
                column: "CityId",
                principalTable: "TbCities",
                principalColumn: "Id");
        }
    }
}
