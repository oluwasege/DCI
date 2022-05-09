using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DCI.Entities.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Approval",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    LastDateModified = table.Column<DateTime>(nullable: false),
                    ApprovedBy = table.Column<string>(nullable: true),
                    RejectedBy = table.Column<string>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: false),
                    ActionComment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approval", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    ResetPasswordToken = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    MiddleName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(nullable: true),
                    LastLoginDate = table.Column<DateTime>(nullable: true),
                    Activated = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserType = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsCSO = table.Column<bool>(nullable: false),
                    IsSupervisor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViolenceType",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    PhysicalAssault = table.Column<bool>(nullable: false),
                    Defilement = table.Column<bool>(nullable: false),
                    Rape = table.Column<bool>(nullable: false),
                    ForcedMarriage = table.Column<bool>(nullable: false),
                    DenialOfResources = table.Column<bool>(nullable: false),
                    Psychological = table.Column<bool>(nullable: false),
                    SocialAssault = table.Column<bool>(nullable: false),
                    FemaleGenitalMutilation = table.Column<bool>(nullable: false),
                    ViolationOfProperty = table.Column<bool>(nullable: false),
                    ChildAbuse = table.Column<bool>(nullable: false),
                    EarlyMarriage = table.Column<bool>(nullable: false),
                    CyberBullying = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolenceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    DCIUserId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    Expires = table.Column<DateTime>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Revoked = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => new { x.DCIUserId, x.Id });
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_DCIUserId",
                        column: x => x.DCIUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    CSOUserId = table.Column<string>(nullable: true),
                    ViolenceTypeId = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    LGA = table.Column<string>(nullable: true),
                    Statement = table.Column<string>(nullable: true),
                    IsFatal = table.Column<bool>(nullable: false),
                    IsPerpetratorArrested = table.Column<bool>(nullable: false),
                    StateOfCase = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUserId = table.Column<string>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ApprovalStatus = table.Column<int>(nullable: false),
                    ApprovalActionId = table.Column<string>(nullable: true),
                    LastDateModified = table.Column<DateTime>(nullable: false),
                    LastModifiedUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Approval_ApprovalActionId",
                        column: x => x.ApprovalActionId,
                        principalTable: "Approval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cases_AspNetUsers_CSOUserId",
                        column: x => x.CSOUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cases_ViolenceType_ViolenceTypeId",
                        column: x => x.ViolenceTypeId,
                        principalTable: "ViolenceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Approval",
                columns: new[] { "Id", "ActionComment", "ActionDate", "ApprovedBy", "CreatedOnUtc", "LastDateModified", "RejectedBy" },
                values: new object[,]
                {
                    { "5e31db4e-6e79-487e-5c9e-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(1006), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e319b4e-6e79-487e-4c9e-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 461, DateTimeKind.Utc).AddTicks(6581), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e31db4e-6e79-487e-4c9e-08d9b6be7c9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(510), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e31db4e-6e79-487e-4c9e-08d9b6be7d9a", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(768), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e21db4e-6e79-487e-4c9e-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(819), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "4e31db4e-6e79-487e-4c9e-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(861), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e31db4e-6a79-487e-4c9e-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(926), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e31db4e-6e79-487e-4c9a-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(967), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e31db4e-6e79-487e-4c9e-18d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(1143), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { "5e31db4e-6e79-437e-4c9e-08d9b6be7d9e", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(1199), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c5e174e-3b0e-446f-86af-483d56fd7210", "54a281f5-230c-407d-acbc-442796a58e89", "ADMIN", "ADMIN" },
                    { "e82fe09a-2419-4b9b-8a2c-b5001e71c997", "23818d71-cc79-4910-86c7-3ffbe96f95af", "CSO", "CSO" },
                    { "fe8d7501-6cb1-4d99-4c9f-08d9b6be7d9e", "be07866f-c63a-46d8-9dbb-354048279aa1", "SUPERVISOR", "SUPERVISOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Activated", "ConcurrencyStamp", "CreatedOnUtc", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "Gender", "IsAdmin", "IsCSO", "IsDeleted", "IsSupervisor", "LastLoginDate", "LastModifiedDate", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "ModifiedOnUtc", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ResetPasswordToken", "SecurityStamp", "State", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[,]
                {
                    { "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", 0, false, "9fcb3b00-9484-4c84-b06f-cdd6e51e3c9d", new DateTime(2022, 5, 8, 21, 39, 26, 475, DateTimeKind.Utc).AddTicks(552), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CSO@dci.com", true, "CSO", 0, false, true, false, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CSO", false, null, null, null, "CSO@dci.com", "CSO@dci.com", "AQAAAAEAACcQAAAAEJ2wjnXSkmMbP18aIP7A7DhXSBQvz03CW60y4NBxUPKITXemGYdx1+fGL8PREqO6VA==", null, false, null, "97093800-6bb2-46e6-949a-57864d2ebfe8", "Lagos", false, "CSO@dci.com", 0 },
                    { "eeae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", 0, false, "249641f9-6667-40eb-92f1-832dd6d2c26a", new DateTime(2022, 5, 8, 21, 39, 26, 484, DateTimeKind.Utc).AddTicks(2481), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CSOSecond@dci.com", true, "Second", 0, false, true, false, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Second", false, null, null, null, "CSOSecond@dci.com", "CSOSecond@dci.com", "AQAAAAEAACcQAAAAECMiUdiD+yIUMUMbT69HtLCHysTerEgYFT1mUFUlt4zASmaH3U6h1sOnn/7U2clmuA==", null, false, null, "f70d266d-5814-418a-8e45-96edbb110be5", "Ogun", false, "CSOSecond@dci.com", 0 },
                    { "5e31db4e-6e79-487e-4c9e-08d9b6be7d9e", 0, false, "79767d03-2323-4983-8f42-d319b7c5f6de", new DateTime(2022, 5, 8, 21, 39, 26, 492, DateTimeKind.Utc).AddTicks(5634), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor@dci.com", true, "Supervisor", 0, false, false, false, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor", false, null, null, null, "Supervisor@dci.com", "Supervisor@dci.com", "AQAAAAEAACcQAAAAEBPd2Fikc3U13m4Y+LJwxwNmbNZGY6937d2zN004ZXupWQIjn7uBTyNH/jy2D4CzqQ==", null, false, null, "cf20fc5e-d5d3-4f4d-81af-efd3395ec049", "Lagos", false, "Supervisor@dci.com", 1 },
                    { "b8633e2d-a33b-45e6-8329-1958b3252bbd", 0, false, "dca8dc13-c492-458d-a17f-dfb62840e6de", new DateTime(2022, 5, 8, 21, 39, 26, 463, DateTimeKind.Utc).AddTicks(9380), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin@dci.com", true, "Admin", 0, true, false, false, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", false, null, null, null, "Admin@dci.com", "Admin@dci.com", "AQAAAAEAACcQAAAAED3D5dUaE+Z6DZXcTWbz19Irgh4ipNDw4gwby7WMXbwlNi4wcPezSHAPeK5/W3h0Fg==", null, false, null, "bcc30d81-b82f-4369-8dd7-bc95cc4d1e1b", "Lagos", false, "Admin@dci.com", 2 }
                });

            migrationBuilder.InsertData(
                table: "ViolenceType",
                columns: new[] { "Id", "ChildAbuse", "CyberBullying", "Defilement", "DenialOfResources", "EarlyMarriage", "FemaleGenitalMutilation", "ForcedMarriage", "PhysicalAssault", "Psychological", "Rape", "SocialAssault", "ViolationOfProperty" },
                values: new object[] { "00000000-0000-0000-0000-000000000000", true, true, true, true, true, true, true, true, true, true, true, true });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "b8633e2d-a33b-45e6-8329-1958b3252bbd", "2c5e174e-3b0e-446f-86af-483d56fd7210" },
                    { "5e31db4e-6e79-487e-4c9e-08d9b6be7d9e", "fe8d7501-6cb1-4d99-4c9f-08d9b6be7d9e" },
                    { "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", "e82fe09a-2419-4b9b-8a2c-b5001e71c997" },
                    { "eeae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", "e82fe09a-2419-4b9b-8a2c-b5001e71c997" }
                });

            migrationBuilder.InsertData(
                table: "Cases",
                columns: new[] { "Id", "ApprovalActionId", "ApprovalStatus", "CSOUserId", "CreatedOnUtc", "DeletedUserId", "DeletionTime", "IsDeleted", "IsFatal", "IsPerpetratorArrested", "LGA", "LastDateModified", "LastModifiedUserId", "State", "StateOfCase", "Statement", "ViolenceTypeId" },
                values: new object[,]
                {
                    { "5e319b4e-6e79-487e-4c9e-08d9b6be7d9e", "5e319b4e-6e79-487e-4c9e-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 502, DateTimeKind.Utc).AddTicks(6397), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "First Case", "00000000-0000-0000-0000-000000000000" },
                    { "5e21db4e-6e79-487e-4c9e-08d9b6be7d9e", "5e21db4e-6e79-487e-4c9e-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(897), null, null, false, true, true, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 0, "Second case", "00000000-0000-0000-0000-000000000000" },
                    { "4e31db4e-6e79-487e-4c9e-08d9b6be7d9e", "4e31db4e-6e79-487e-4c9e-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(1008), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "Third case", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6a79-487e-4c9e-08d9b6be7d9e", "5e31db4e-6a79-487e-4c9e-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(1072), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "fourth", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6e79-487e-4c9a-08d9b6be7d9e", "5e31db4e-6e79-487e-4c9a-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(1119), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "fifth", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6e79-487e-5c9e-08d9b6be7d9e", "5e31db4e-6e79-487e-5c9e-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(1167), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "sixth", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6e79-487e-4c9e-18d9b6be7d9e", "5e31db4e-6e79-487e-4c9e-18d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(1214), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "seventh", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6e79-437e-4c9e-08d9b6be7d9e", "5e31db4e-6e79-437e-4c9e-08d9b6be7d9e", 4, "cdae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(1263), null, null, false, true, false, "Lagos", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lagos", 1, "eight", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6e79-487e-4c9e-08d9b6be7c9e", "5e31db4e-6e79-487e-4c9e-08d9b6be7c9e", 4, "eeae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(641), null, null, false, true, false, "Abeokuta", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ogun", 1, "statement", "00000000-0000-0000-0000-000000000000" },
                    { "5e31db4e-6e79-487e-4c9e-08d9b6be7d9a", "5e31db4e-6e79-487e-4c9e-08d9b6be7d9a", 4, "eeae8cc2-adec-4ab1-4ca0-08d9b6be7d9e", new DateTime(2022, 5, 8, 21, 39, 26, 503, DateTimeKind.Utc).AddTicks(828), null, null, false, false, false, "Ogun", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ogun", 0, "Second Statement", "00000000-0000-0000-0000-000000000000" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ApprovalActionId",
                table: "Cases",
                column: "ApprovalActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CSOUserId",
                table: "Cases",
                column: "CSOUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ViolenceTypeId",
                table: "Cases",
                column: "ViolenceTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Approval");

            migrationBuilder.DropTable(
                name: "ViolenceType");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
