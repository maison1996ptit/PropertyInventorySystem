using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PIMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceOfAcquisitions_Contacts_ContactId",
                table: "PriceOfAcquisitions");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceOfAcquisitions_Properties_PropertyId",
                table: "PriceOfAcquisitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PriceOfAcquisitions",
                table: "PriceOfAcquisitions");

            migrationBuilder.RenameTable(
                name: "PriceOfAcquisitions",
                newName: "PropertyContacts");

            migrationBuilder.RenameIndex(
                name: "IX_PriceOfAcquisitions_ContactId",
                table: "PropertyContacts",
                newName: "IX_PropertyContacts_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyContacts",
                table: "PropertyContacts",
                columns: new[] { "PropertyId", "ContactId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyContacts_Contacts_ContactId",
                table: "PropertyContacts",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyContacts_Properties_PropertyId",
                table: "PropertyContacts",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyContacts_Contacts_ContactId",
                table: "PropertyContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyContacts_Properties_PropertyId",
                table: "PropertyContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyContacts",
                table: "PropertyContacts");

            migrationBuilder.RenameTable(
                name: "PropertyContacts",
                newName: "PriceOfAcquisitions");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyContacts_ContactId",
                table: "PriceOfAcquisitions",
                newName: "IX_PriceOfAcquisitions_ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PriceOfAcquisitions",
                table: "PriceOfAcquisitions",
                columns: new[] { "PropertyId", "ContactId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PriceOfAcquisitions_Contacts_ContactId",
                table: "PriceOfAcquisitions",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceOfAcquisitions_Properties_PropertyId",
                table: "PriceOfAcquisitions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
