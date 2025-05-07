using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenreName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.GenreID);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.LanguageCode);
                });

            migrationBuilder.CreateTable(
                name: "PerformerType",
                columns: table => new
                {
                    PerformerTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformerType", x => x.PerformerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseType",
                columns: table => new
                {
                    ReleaseTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseType", x => x.ReleaseTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Rondo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rondo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scale",
                columns: table => new
                {
                    ScaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scale", x => x.ScaleId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Release",
                columns: table => new
                {
                    ReleaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MainGenreCode = table.Column<int>(type: "int", nullable: true),
                    MainGenreGenreID = table.Column<int>(type: "int", nullable: true),
                    SecondGenreCode = table.Column<int>(type: "int", nullable: true),
                    SecondGenreGenreID = table.Column<int>(type: "int", nullable: true),
                    ReleaseCover = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ReleaseTypeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Release", x => x.ReleaseID);
                    table.ForeignKey(
                        name: "FK_Release_Genre_MainGenreGenreID",
                        column: x => x.MainGenreGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Release_Genre_SecondGenreGenreID",
                        column: x => x.SecondGenreGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Release_ReleaseType_ReleaseTypeID",
                        column: x => x.ReleaseTypeID,
                        principalTable: "ReleaseType",
                        principalColumn: "ReleaseTypeID");
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RondoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryCode);
                    table.ForeignKey(
                        name: "FK_Country_Rondo_RondoId",
                        column: x => x.RondoId,
                        principalTable: "Rondo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contributor",
                columns: table => new
                {
                    ContributorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsMale = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributor", x => x.ContributorID);
                    table.ForeignKey(
                        name: "FK_Contributor_Country_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Country",
                        principalColumn: "CountryCode");
                });

            migrationBuilder.CreateTable(
                name: "Performer",
                columns: table => new
                {
                    PerformerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerformerTypeID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainGenreID = table.Column<int>(type: "int", nullable: true),
                    SecondaryGenreID = table.Column<int>(type: "int", nullable: true),
                    CountryID = table.Column<int>(type: "int", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performer", x => x.PerformerID);
                    table.ForeignKey(
                        name: "FK_Performer_Country_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Country",
                        principalColumn: "CountryCode");
                    table.ForeignKey(
                        name: "FK_Performer_Genre_MainGenreID",
                        column: x => x.MainGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Performer_Genre_SecondaryGenreID",
                        column: x => x.SecondaryGenreID,
                        principalTable: "Genre",
                        principalColumn: "GenreID");
                    table.ForeignKey(
                        name: "FK_Performer_PerformerType_PerformerTypeID",
                        column: x => x.PerformerTypeID,
                        principalTable: "PerformerType",
                        principalColumn: "PerformerTypeID");
                });

            migrationBuilder.CreateTable(
                name: "ReleasePerformer",
                columns: table => new
                {
                    ReleaseID = table.Column<int>(type: "int", nullable: false),
                    PerformerID = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleasePerformer", x => new { x.ReleaseID, x.PerformerID });
                    table.ForeignKey(
                        name: "FK_ReleasePerformer_Performer_PerformerID",
                        column: x => x.PerformerID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleasePerformer_Release_ReleaseID",
                        column: x => x.ReleaseID,
                        principalTable: "Release",
                        principalColumn: "ReleaseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Track",
                columns: table => new
                {
                    TrackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsSong = table.Column<bool>(type: "bit", nullable: true),
                    Lyrics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestID = table.Column<int>(type: "int", nullable: false),
                    /*MainGuestPerformerID = table.Column<int>(type: "int", nullable: false),*/
                    SecondGuestID = table.Column<int>(type: "int", nullable: true),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ScaleID = table.Column<int>(type: "int", nullable: true),
                    BPM = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Track", x => x.TrackID);
                    table.ForeignKey(
                        name: "FK_Track_Language_LanguageCode",
                        column: x => x.LanguageCode,
                        principalTable: "Language",
                        principalColumn: "LanguageCode");
                    table.ForeignKey(
                        name: "FK_Track_Performer_GuestID",
                        column: x => x.GuestID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Track_Performer_SecondGuestID",
                        column: x => x.SecondGuestID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID");
                    table.ForeignKey(
                        name: "FK_Track_Scale_ScaleID",
                        column: x => x.ScaleID,
                        principalTable: "Scale",
                        principalColumn: "ScaleId");
                });

            migrationBuilder.CreateTable(
                name: "ReleaseTrack",
                columns: table => new
                {
                    ReleaseID = table.Column<int>(type: "int", nullable: false),
                    TrackID = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false),
                    TrackNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseTrack", x => new { x.ReleaseID, x.TrackID });
                    table.ForeignKey(
                        name: "FK_ReleaseTrack_Release_ReleaseID",
                        column: x => x.ReleaseID,
                        principalTable: "Release",
                        principalColumn: "ReleaseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseTrack_Track_TrackID",
                        column: x => x.TrackID,
                        principalTable: "Track",
                        principalColumn: "TrackID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackPerformer",
                columns: table => new
                {
                    TrackPerformerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackID = table.Column<int>(type: "int", nullable: false),
                    PerformerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackPerformer", x => x.TrackPerformerID);
                    table.ForeignKey(
                        name: "FK_TrackPerformer_Performer_PerformerID",
                        column: x => x.PerformerID,
                        principalTable: "Performer",
                        principalColumn: "PerformerID",
                        onDelete: ReferentialAction.Cascade);
                    /*table.ForeignKey(
                        name: "FK_TrackPerformer_Track_TrackID",
                        column: x => x.TrackID,
                        principalTable: "Track",
                        principalColumn: "TrackID",
                        onDelete: ReferentialAction.Cascade);*/
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
                name: "IX_Contributor_CountryCode",
                table: "Contributor",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Country_RondoId",
                table: "Country",
                column: "RondoId");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_CountryCode",
                table: "Performer",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_MainGenreID",
                table: "Performer",
                column: "MainGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_PerformerTypeID",
                table: "Performer",
                column: "PerformerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Performer_SecondaryGenreID",
                table: "Performer",
                column: "SecondaryGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Release_MainGenreGenreID",
                table: "Release",
                column: "MainGenreGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_Release_ReleaseTypeID",
                table: "Release",
                column: "ReleaseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Release_SecondGenreGenreID",
                table: "Release",
                column: "SecondGenreGenreID");

            migrationBuilder.CreateIndex(
                name: "IX_ReleasePerformer_PerformerID",
                table: "ReleasePerformer",
                column: "PerformerID");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTrack_TrackID",
                table: "ReleaseTrack",
                column: "TrackID");

            migrationBuilder.CreateIndex(
                name: "IX_Track_LanguageCode",
                table: "Track",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Track_GuestID",
                table: "Track",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_Track_ScaleID",
                table: "Track",
                column: "ScaleID");

            migrationBuilder.CreateIndex(
                name: "IX_Track_SecondGuestID",
                table: "Track",
                column: "SecondGuestID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackPerformer_PerformerID",
                table: "TrackPerformer",
                column: "PerformerID");

            migrationBuilder.CreateIndex(
                name: "IX_TrackPerformer_TrackID",
                table: "TrackPerformer",
                column: "TrackID");
        }

        /// <inheritdoc />
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
                name: "Contributor");

            migrationBuilder.DropTable(
                name: "ReleasePerformer");

            migrationBuilder.DropTable(
                name: "ReleaseTrack");

            migrationBuilder.DropTable(
                name: "TrackPerformer");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Release");

            migrationBuilder.DropTable(
                name: "Track");

            migrationBuilder.DropTable(
                name: "ReleaseType");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Performer");

            migrationBuilder.DropTable(
                name: "Scale");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "PerformerType");

            migrationBuilder.DropTable(
                name: "Rondo");
        }
    }
}
