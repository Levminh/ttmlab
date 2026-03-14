using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Ovncr.Migrations
{
    /// <summary>
    /// Migration nâng cấp schema Identity từ ASP.NET MVC 5 (Identity v1/v2) 
    /// lên ASP.NET Core Identity.
    /// 
    /// Những gì thay đổi:
    /// 1. Thêm các cột mới vào AspNetUsers (NormalizedUserName, NormalizedEmail, ConcurrencyStamp, LockoutEnd, FullName)
    /// 2. Thêm các cột mới vào AspNetRoles  (NormalizedName, ConcurrencyStamp)  
    /// 3. Tạo bảng AspNetRoleClaims (mới, chưa có trong schema cũ)
    /// 4. Tạo bảng AspNetUserTokens  (mới, chưa có trong schema cũ)
    /// 5. Cập nhật index cho AspNetUserLogins (PK thay đổi)
    /// </summary>
    public partial class UpgradeIdentitySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ================================================================
            // 1. Thêm cột mới vào AspNetUsers (cột cũ vẫn giữ nguyên)
            //    Lưu ý: FullName đã có sẵn trong DB cũ, không cần thêm lại
            // ================================================================

            // NormalizedUserName: dùng cho tìm kiếm không phân biệt hoa thường
            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            // NormalizedEmail: dùng cho tìm kiếm email không phân biệt hoa thường
            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            // ConcurrencyStamp: chống xung đột cập nhật đồng thời
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            // LockoutEnd: thay thế LockoutEndDateUtc cũ (kiểu DateTimeOffset)
            // Lưu ý: cột LockoutEndDateUtc cũ vẫn giữ để không mất dữ liệu
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            // ================================================================
            // 2. Thêm cột mới vào AspNetRoles
            // ================================================================

            // NormalizedName: tên role không phân biệt hoa thường
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "AspNetRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            // ConcurrencyStamp: chống xung đột cập nhật đồng thời
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            // ================================================================
            // 3. Tạo bảng AspNetRoleClaims (bảng mới, chưa có trong schema cũ)
            //    Dùng để gán claims trực tiếp cho một Role
            // ================================================================
            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // ================================================================
            // 4. Tạo bảng AspNetUserTokens (bảng mới, chưa có trong schema cũ)
            //    Dùng để lưu authentication tokens (reset password, 2FA...)
            // ================================================================
            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // ================================================================
            // 5. Tạo indexes cần thiết cho Identity
            // ================================================================

            // Index cho AspNetRoleClaims
            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            // Index cho NormalizedName trên AspNetRoles
            // (Dùng tên mới để tránh xung đột với RoleNameIndex cũ)
            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_NormalizedName",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            // Index cho NormalizedEmail trên AspNetUsers
            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            // Index cho NormalizedUserName trên AspNetUsers
            // (Dùng tên mới để tránh xung đột với UserNameIndex cũ)
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NormalizedUserName",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback: Xóa bảng mới và bỏ cột thêm vào
            migrationBuilder.DropTable(name: "AspNetRoleClaims");
            migrationBuilder.DropTable(name: "AspNetUserTokens");

            migrationBuilder.DropIndex(name: "IX_AspNetRoles_NormalizedName", table: "AspNetRoles");
            migrationBuilder.DropIndex(name: "EmailIndex", table: "AspNetUsers");
            migrationBuilder.DropIndex(name: "IX_AspNetUsers_NormalizedUserName", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "NormalizedUserName", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "NormalizedEmail", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "ConcurrencyStamp", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "LockoutEnd", table: "AspNetUsers");
            migrationBuilder.DropColumn(name: "NormalizedName", table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "ConcurrencyStamp", table: "AspNetRoles");
        }
    }
}
