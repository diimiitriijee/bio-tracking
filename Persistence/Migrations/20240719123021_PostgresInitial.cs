using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PostgresInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: true),
                    Prezime = table.Column<string>(type: "text", nullable: true),
                    Telefon = table.Column<string>(type: "text", nullable: true),
                    DatumRodjenja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    StrucnaSprema = table.Column<string>(type: "text", nullable: true),
                    BrojOdrzanihObilazaka = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Biljke",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Naziv = table.Column<string>(type: "text", nullable: true),
                    Opis = table.Column<string>(type: "text", nullable: true),
                    Vrsta = table.Column<string>(type: "text", nullable: true),
                    Lekovita = table.Column<bool>(type: "boolean", nullable: false),
                    Slika = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biljke", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Podrucja",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Oblast = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podrucja", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VodicZahtevi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: true),
                    Prezime = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Telefon = table.Column<string>(type: "text", nullable: true),
                    DatumRodjenja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StrucnaSprema = table.Column<string>(type: "text", nullable: true),
                    BrojOdrzanihObilazaka = table.Column<int>(type: "integer", nullable: false),
                    PutanjaSlikeDiplome = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VodicZahtevi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
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
                name: "Ocene",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    KorisnikId = table.Column<Guid>(type: "uuid", nullable: true),
                    VodicId = table.Column<Guid>(type: "uuid", nullable: true),
                    VrednostOcene = table.Column<int>(type: "integer", nullable: false),
                    Komentar = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocene", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ocene_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ocene_AspNetUsers_VodicId",
                        column: x => x.VodicId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KorisnikId = table.Column<Guid>(type: "uuid", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Slike",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    KorisnikId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slike_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFollowings",
                columns: table => new
                {
                    ObserverId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowings", x => new { x.ObserverId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_UserFollowings_AspNetUsers_ObserverId",
                        column: x => x.ObserverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollowings_AspNetUsers_TargetId",
                        column: x => x.TargetId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KoordinatePodrucja",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    PodrucjeID = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoordinatePodrucja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KoordinatePodrucja_Podrucja_PodrucjeID",
                        column: x => x.PodrucjeID,
                        principalTable: "Podrucja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Podrucja_Biljke",
                columns: table => new
                {
                    BiljkaID = table.Column<Guid>(type: "uuid", nullable: false),
                    PodrucjeID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podrucja_Biljke", x => new { x.BiljkaID, x.PodrucjeID });
                    table.ForeignKey(
                        name: "FK_Podrucja_Biljke_Biljke_BiljkaID",
                        column: x => x.BiljkaID,
                        principalTable: "Biljke",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Podrucja_Biljke_Podrucja_PodrucjeID",
                        column: x => x.PodrucjeID,
                        principalTable: "Podrucja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rute",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Opis = table.Column<string>(type: "text", nullable: true),
                    Duzina = table.Column<double>(type: "double precision", nullable: false),
                    Tip = table.Column<string>(type: "text", nullable: true),
                    ZaDecu = table.Column<bool>(type: "boolean", nullable: false),
                    Prohodnost = table.Column<string>(type: "text", nullable: true),
                    Uspon = table.Column<string>(type: "text", nullable: true),
                    PodrucjeID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rute_Podrucja_PodrucjeID",
                        column: x => x.PodrucjeID,
                        principalTable: "Podrucja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KoordinateRute",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RutaID = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoordinateRute", x => x.ID);
                    table.ForeignKey(
                        name: "FK_KoordinateRute_Rute_RutaID",
                        column: x => x.RutaID,
                        principalTable: "Rute",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obilasci",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Naziv = table.Column<string>(type: "text", nullable: true),
                    Opis = table.Column<string>(type: "text", nullable: true),
                    DatumOdrzavanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BrojMaxPolaznika = table.Column<int>(type: "integer", nullable: false),
                    MestoOkupljanja = table.Column<string>(type: "text", nullable: true),
                    Alergije = table.Column<string>(type: "text", nullable: true),
                    VodicId = table.Column<Guid>(type: "uuid", nullable: true),
                    PodrucjeID = table.Column<Guid>(type: "uuid", nullable: true),
                    RutaID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obilasci", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Obilasci_AspNetUsers_VodicId",
                        column: x => x.VodicId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Obilasci_Podrucja_PodrucjeID",
                        column: x => x.PodrucjeID,
                        principalTable: "Podrucja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Obilasci_Rute_RutaID",
                        column: x => x.RutaID,
                        principalTable: "Rute",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Komentari",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Tekst = table.Column<string>(type: "text", nullable: true),
                    DatumKreiranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KorisnikId = table.Column<Guid>(type: "uuid", nullable: true),
                    ObilazakID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentari", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Komentari_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Komentari_Obilasci_ObilazakID",
                        column: x => x.ObilazakID,
                        principalTable: "Obilasci",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavljeniObilasci",
                columns: table => new
                {
                    ObilazakID = table.Column<Guid>(type: "uuid", nullable: false),
                    KorisnikID = table.Column<Guid>(type: "uuid", nullable: false),
                    DatumPrijave = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavljeniObilasci", x => new { x.ObilazakID, x.KorisnikID });
                    table.ForeignKey(
                        name: "FK_PrijavljeniObilasci_AspNetUsers_KorisnikID",
                        column: x => x.KorisnikID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrijavljeniObilasci_Obilasci_ObilazakID",
                        column: x => x.ObilazakID,
                        principalTable: "Obilasci",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_KorisnikId",
                table: "Komentari",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentari_ObilazakID",
                table: "Komentari",
                column: "ObilazakID");

            migrationBuilder.CreateIndex(
                name: "IX_KoordinatePodrucja_PodrucjeID",
                table: "KoordinatePodrucja",
                column: "PodrucjeID");

            migrationBuilder.CreateIndex(
                name: "IX_KoordinateRute_RutaID",
                table: "KoordinateRute",
                column: "RutaID");

            migrationBuilder.CreateIndex(
                name: "IX_Obilasci_PodrucjeID",
                table: "Obilasci",
                column: "PodrucjeID");

            migrationBuilder.CreateIndex(
                name: "IX_Obilasci_RutaID",
                table: "Obilasci",
                column: "RutaID");

            migrationBuilder.CreateIndex(
                name: "IX_Obilasci_VodicId",
                table: "Obilasci",
                column: "VodicId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocene_KorisnikId",
                table: "Ocene",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocene_VodicId",
                table: "Ocene",
                column: "VodicId");

            migrationBuilder.CreateIndex(
                name: "IX_Podrucja_Biljke_PodrucjeID",
                table: "Podrucja_Biljke",
                column: "PodrucjeID");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavljeniObilasci_KorisnikID",
                table: "PrijavljeniObilasci",
                column: "KorisnikID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_KorisnikId",
                table: "RefreshToken",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rute_PodrucjeID",
                table: "Rute",
                column: "PodrucjeID");

            migrationBuilder.CreateIndex(
                name: "IX_Slike_KorisnikId",
                table: "Slike",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowings_TargetId",
                table: "UserFollowings",
                column: "TargetId");
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
                name: "Komentari");

            migrationBuilder.DropTable(
                name: "KoordinatePodrucja");

            migrationBuilder.DropTable(
                name: "KoordinateRute");

            migrationBuilder.DropTable(
                name: "Ocene");

            migrationBuilder.DropTable(
                name: "Podrucja_Biljke");

            migrationBuilder.DropTable(
                name: "PrijavljeniObilasci");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Slike");

            migrationBuilder.DropTable(
                name: "UserFollowings");

            migrationBuilder.DropTable(
                name: "VodicZahtevi");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Biljke");

            migrationBuilder.DropTable(
                name: "Obilasci");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Rute");

            migrationBuilder.DropTable(
                name: "Podrucja");
        }
    }
}
