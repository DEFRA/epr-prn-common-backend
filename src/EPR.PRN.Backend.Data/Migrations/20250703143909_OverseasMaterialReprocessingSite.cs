using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPR.PRN.Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class OverseasMaterialReprocessingSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lookup.Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup.Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasMaterialReprocessingSite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    RegistrationMaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasMaterialReprocessingSite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    OrganisationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CityOrTown = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    StateProvince = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    SiteCoordinates = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddress_Lookup.Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Lookup.Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddress_Public.Registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Public.Registration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OverseasMaterialReprocessingSiteRegistrationMaterial",
                columns: table => new
                {
                    OverseasMaterialReprocessingSitesId = table.Column<int>(type: "int", nullable: false),
                    RegistrationMaterialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasMaterialReprocessingSiteRegistrationMaterial", x => new { x.OverseasMaterialReprocessingSitesId, x.RegistrationMaterialsId });
                    table.ForeignKey(
                        name: "FK_OverseasMaterialReprocessingSiteRegistrationMaterial_Public.OverseasMaterialReprocessingSite_OverseasMaterialReprocessingSit~",
                        column: x => x.OverseasMaterialReprocessingSitesId,
                        principalTable: "Public.OverseasMaterialReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasMaterialReprocessingSiteRegistrationMaterial_Public.RegistrationMaterial_RegistrationMaterialsId",
                        column: x => x.RegistrationMaterialsId,
                        principalTable: "Public.RegistrationMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OverseasAddressOverseasMaterialReprocessingSite",
                columns: table => new
                {
                    OverseasAddressesId = table.Column<int>(type: "int", nullable: false),
                    OverseasMaterialReprocessingSitesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverseasAddressOverseasMaterialReprocessingSite", x => new { x.OverseasAddressesId, x.OverseasMaterialReprocessingSitesId });
                    table.ForeignKey(
                        name: "FK_OverseasAddressOverseasMaterialReprocessingSite_Public.OverseasAddress_OverseasAddressesId",
                        column: x => x.OverseasAddressesId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverseasAddressOverseasMaterialReprocessingSite_Public.OverseasMaterialReprocessingSite_OverseasMaterialReprocessingSitesId",
                        column: x => x.OverseasMaterialReprocessingSitesId,
                        principalTable: "Public.OverseasMaterialReprocessingSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.InterimOverseasConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterimSiteId = table.Column<int>(type: "int", nullable: false),
                    ParentOverseasAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.InterimOverseasConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.InterimOverseasConnections_Public.OverseasAddress_ParentOverseasAddressId",
                        column: x => x.ParentOverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasAddressContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAddressContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddressContact_Public.OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Public.OverseasAddressWasteCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverseasAddressId = table.Column<int>(type: "int", nullable: false),
                    CodeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Public.OverseasAddressWasteCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Public.OverseasAddressWasteCode_Public.OverseasAddress_OverseasAddressId",
                        column: x => x.OverseasAddressId,
                        principalTable: "Public.OverseasAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lookup.Country",
                columns: new[] { "Id", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 1, "ad", "Andorra" },
                    { 2, "ae", "United Arab Emirates" },
                    { 3, "af", "Afghanistan" },
                    { 4, "ag", "Antigua and Barbuda" },
                    { 5, "ai", "Anguilla" },
                    { 6, "al", "Albania" },
                    { 7, "am", "Armenia" },
                    { 8, "an", "Netherlands Antilles" },
                    { 9, "ao", "Angola" },
                    { 10, "aq", "Antarctica" },
                    { 11, "ar", "Argentina" },
                    { 13, "as", "American Samoa" },
                    { 14, "at", "Austria" },
                    { 15, "au", "Australia" },
                    { 16, "aw", "Aruba" },
                    { 17, "az", "Azerbaidjan" },
                    { 18, "ba", "Bosnia-Herzegovina" },
                    { 19, "bb", "Barbados" },
                    { 20, "bd", "Bangladesh" },
                    { 21, "be", "Belgium" },
                    { 22, "bf", "Burkina Faso" },
                    { 23, "bg", "Bulgaria" },
                    { 24, "bh", "Bahrain" },
                    { 25, "bi", "Burundi" },
                    { 26, "bj", "Benin" },
                    { 27, "bm", "Bermuda" },
                    { 28, "bn", "Brunei Darussalam" },
                    { 29, "bo", "Bolivia" },
                    { 30, "br", "Brazil" },
                    { 31, "bs", "Bahamas" },
                    { 32, "bt", "Bhutan" },
                    { 33, "bv", "Bouvet Island" },
                    { 34, "bw", "Botswana" },
                    { 35, "by", "Belarus" },
                    { 36, "bz", "Belize" },
                    { 37, "ca", "Canada" },
                    { 38, "cc", "Cocos (Keeling) Islands" },
                    { 39, "cf", "Central African Republic" },
                    { 40, "cg", "Congo" },
                    { 41, "ch", "Switzerland" },
                    { 42, "ci", "Ivory Coast (Cote D'Ivoire)" },
                    { 43, "ck", "Cook Islands" },
                    { 44, "cl", "Chile" },
                    { 45, "cm", "Cameroon" },
                    { 46, "cn", "China" },
                    { 47, "co", "Colombia" },
                    { 48, "com", "Commercial" },
                    { 49, "cr", "Costa Rica" },
                    { 50, "cs", "Former Czechoslovakia" },
                    { 51, "cu", "Cuba" },
                    { 52, "cv", "Cape Verde" },
                    { 53, "cx", "Christmas Island" },
                    { 54, "cy", "Cyprus" },
                    { 55, "cz", "Czech Republic" },
                    { 56, "de", "Germany" },
                    { 57, "dj", "Djibouti" },
                    { 58, "dk", "Denmark" },
                    { 59, "dm", "Dominica" },
                    { 60, "do", "Dominican Republic" },
                    { 61, "dz", "Algeria" },
                    { 62, "ec", "Ecuador" },
                    { 64, "ee", "Estonia" },
                    { 65, "eg", "Egypt" },
                    { 66, "eh", "Western Sahara" },
                    { 67, "er", "Eritrea" },
                    { 68, "es", "Spain" },
                    { 69, "et", "Ethiopia" },
                    { 70, "fi", "Finland" },
                    { 71, "fj", "Fiji" },
                    { 72, "fk", "Falkland Islands" },
                    { 73, "fm", "Micronesia" },
                    { 74, "fo", "Faroe Islands" },
                    { 75, "fr", "France" },
                    { 76, "fx", "France (European Territory)" },
                    { 77, "ga", "Gabon" },
                    { 79, "gd", "Grenada" },
                    { 80, "ge", "Georgia" },
                    { 81, "gf", "French Guyana" },
                    { 82, "gh", "Ghana" },
                    { 83, "gi", "Gibraltar" },
                    { 84, "gl", "Greenland" },
                    { 85, "gm", "Gambia" },
                    { 86, "gn", "Guinea" },
                    { 88, "gp", "Guadeloupe (French)" },
                    { 89, "gq", "Equatorial Guinea" },
                    { 90, "gr", "Greece" },
                    { 91, "gs", "S. Georgia & S. Sandwich Isls." },
                    { 92, "gt", "Guatemala" },
                    { 93, "gu", "Guam (USA)" },
                    { 94, "gw", "Guinea Bissau" },
                    { 95, "gy", "Guyana" },
                    { 96, "hk", "Hong Kong" },
                    { 97, "hm", "Heard and McDonald Islands" },
                    { 98, "hn", "Honduras" },
                    { 99, "hr", "Croatia" },
                    { 100, "ht", "Haiti" },
                    { 101, "hu", "Hungary" },
                    { 102, "id", "Indonesia" },
                    { 103, "ie", "Ireland" },
                    { 104, "il", "Israel" },
                    { 105, "in", "India" },
                    { 106, "int", "International" },
                    { 107, "io", "British Indian Ocean Territory" },
                    { 108, "iq", "Iraq" },
                    { 109, "ir", "Iran" },
                    { 110, "is", "Iceland" },
                    { 111, "it", "Italy" },
                    { 112, "jm", "Jamaica" },
                    { 113, "jo", "Jordan" },
                    { 114, "jp", "Japan" },
                    { 115, "ke", "Kenya" },
                    { 116, "kg", "Kyrgyzstan" },
                    { 117, "kh", "Cambodia" },
                    { 118, "ki", "Kiribati" },
                    { 119, "km", "Comoros" },
                    { 120, "kn", "Saint Kitts & Nevis Anguilla" },
                    { 121, "kp", "North Korea" },
                    { 122, "kr", "South Korea" },
                    { 123, "kw", "Kuwait" },
                    { 124, "ky", "Cayman Islands" },
                    { 125, "kz", "Kazakhstan" },
                    { 126, "la", "Laos" },
                    { 127, "lb", "Lebanon" },
                    { 128, "lc", "Saint Lucia" },
                    { 129, "li", "Liechtenstein" },
                    { 130, "lk", "Sri Lanka" },
                    { 131, "lr", "Liberia" },
                    { 132, "ls", "Lesotho" },
                    { 133, "lt", "Lithuania" },
                    { 134, "lu", "Luxembourg" },
                    { 135, "lv", "Latvia" },
                    { 136, "ly", "Libya" },
                    { 137, "ma", "Morocco" },
                    { 138, "mc", "Monaco" },
                    { 139, "md", "Moldavia" },
                    { 140, "mg", "Madagascar" },
                    { 141, "mh", "Marshall Islands" },
                    { 143, "mk", "Macedonia" },
                    { 144, "ml", "Mali" },
                    { 145, "mm", "Myanmar" },
                    { 146, "mn", "Mongolia" },
                    { 147, "mo", "Macau" },
                    { 148, "mp", "Northern Mariana Islands" },
                    { 149, "mq", "Martinique (French)" },
                    { 150, "mr", "Mauritania" },
                    { 151, "ms", "Montserrat" },
                    { 152, "mt", "Malta" },
                    { 153, "mu", "Mauritius" },
                    { 154, "mv", "Maldives" },
                    { 155, "mw", "Malawi" },
                    { 156, "mx", "Mexico" },
                    { 157, "my", "Malaysia" },
                    { 158, "mz", "Mozambique" },
                    { 159, "na", "Namibia" },
                    { 161, "nc", "New Caledonia (French)" },
                    { 162, "ne", "Niger" },
                    { 163, "net", "Network" },
                    { 164, "nf", "Norfolk Island" },
                    { 165, "ng", "Nigeria" },
                    { 166, "ni", "Nicaragua" },
                    { 167, "nl", "Netherlands" },
                    { 168, "no", "Norway" },
                    { 169, "np", "Nepal" },
                    { 170, "nr", "Nauru" },
                    { 171, "nt", "Neutral Zone" },
                    { 172, "nu", "Niue" },
                    { 173, "nz", "New Zealand" },
                    { 174, "om", "Oman" },
                    { 176, "pa", "Panama" },
                    { 177, "pe", "Peru" },
                    { 178, "pf", "Polynesia (French)" },
                    { 179, "pg", "Papua New Guinea" },
                    { 180, "ph", "Philippines" },
                    { 181, "pk", "Pakistan" },
                    { 182, "pl", "Poland" },
                    { 183, "pm", "Saint Pierre and Miquelon" },
                    { 184, "pn", "Pitcairn Island" },
                    { 185, "pr", "Puerto Rico" },
                    { 186, "pt", "Portugal" },
                    { 187, "pw", "Palau" },
                    { 188, "py", "Paraguay" },
                    { 189, "qa", "Qatar" },
                    { 190, "re", "Reunion (French)" },
                    { 191, "ro", "Romania" },
                    { 192, "ru", "Russian Federation" },
                    { 193, "rw", "Rwanda" },
                    { 194, "sa", "Saudi Arabia" },
                    { 195, "sb", "Solomon Islands" },
                    { 196, "sc", "Seychelles" },
                    { 197, "sd", "Sudan" },
                    { 198, "se", "Sweden" },
                    { 199, "sg", "Singapore" },
                    { 200, "sh", "Saint Helena" },
                    { 201, "si", "Slovenia" },
                    { 202, "sj", "Svalbard and Jan Mayen Islands" },
                    { 203, "sk", "Slovak Republic" },
                    { 204, "sl", "Sierra Leone" },
                    { 205, "sm", "San Marino" },
                    { 206, "sn", "Senegal" },
                    { 207, "so", "Somalia" },
                    { 208, "sr", "Suriname" },
                    { 209, "st", "Saint Tome (Sao Tome) and Principe" },
                    { 210, "su", "Former USSR" },
                    { 211, "sv", "El Salvador" },
                    { 212, "sy", "Syria" },
                    { 213, "sz", "Swaziland" },
                    { 214, "tc", "Turks and Caicos Islands" },
                    { 215, "td", "Chad" },
                    { 216, "tf", "French Southern Territories" },
                    { 217, "tg", "Togo" },
                    { 218, "th", "Thailand" },
                    { 219, "tj", "Tadjikistan" },
                    { 220, "tk", "Tokelau" },
                    { 221, "tm", "Turkmenistan" },
                    { 222, "tn", "Tunisia" },
                    { 223, "to", "Tonga" },
                    { 224, "tp", "East Timor" },
                    { 225, "tr", "Turkey" },
                    { 226, "tt", "Trinidad and Tobago" },
                    { 227, "tv", "Tuvalu" },
                    { 228, "tw", "Taiwan" },
                    { 229, "tz", "Tanzania" },
                    { 230, "ua", "Ukraine" },
                    { 231, "ug", "Uganda" },
                    { 233, "um", "USA Minor Outlying Islands" },
                    { 234, "us", "United States" },
                    { 235, "uy", "Uruguay" },
                    { 236, "uz", "Uzbekistan" },
                    { 237, "va", "Vatican City State" },
                    { 238, "vc", "Saint Vincent & Grenadines" },
                    { 239, "ve", "Venezuela" },
                    { 240, "vg", "Virgin Islands (British)" },
                    { 241, "vi", "Virgin Islands (USA)" },
                    { 242, "vn", "Vietnam" },
                    { 243, "vu", "Vanuatu" },
                    { 244, "wf", "Wallis and Futuna Islands" },
                    { 245, "ws", "Samoa" },
                    { 246, "ye", "Yemen" },
                    { 247, "yt", "Mayotte" },
                    { 248, "yu", "Yugoslavia" },
                    { 249, "za", "South Africa" },
                    { 250, "zm", "Zambia" },
                    { 251, "zr", "Zaire" },
                    { 252, "zw", "Zimbabwe" }
                });

            migrationBuilder.InsertData(
                table: "Lookup.RegulatorTask",
                columns: new[] { "Id", "ApplicationTypeId", "IsMaterialSpecific", "JourneyTypeId", "Name" },
                values: new object[] { 30, 2, true, 1, "InterimSites" });

            migrationBuilder.CreateIndex(
                name: "IX_OverseasAddressOverseasMaterialReprocessingSite_OverseasMaterialReprocessingSitesId",
                table: "OverseasAddressOverseasMaterialReprocessingSite",
                column: "OverseasMaterialReprocessingSitesId");

            migrationBuilder.CreateIndex(
                name: "IX_OverseasMaterialReprocessingSiteRegistrationMaterial_RegistrationMaterialsId",
                table: "OverseasMaterialReprocessingSiteRegistrationMaterial",
                column: "RegistrationMaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.InterimOverseasConnections_ExternalId",
                table: "Public.InterimOverseasConnections",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.InterimOverseasConnections_ParentOverseasAddressId",
                table: "Public.InterimOverseasConnections",
                column: "ParentOverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_CountryId",
                table: "Public.OverseasAddress",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_ExternalId",
                table: "Public.OverseasAddress",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddress_RegistrationId",
                table: "Public.OverseasAddress",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressContact_OverseasAddressId",
                table: "Public.OverseasAddressContact",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressWasteCode_ExternalId",
                table: "Public.OverseasAddressWasteCode",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasAddressWasteCode_OverseasAddressId",
                table: "Public.OverseasAddressWasteCode",
                column: "OverseasAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Public.OverseasMaterialReprocessingSite_ExternalId",
                table: "Public.OverseasMaterialReprocessingSite",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverseasAddressOverseasMaterialReprocessingSite");

            migrationBuilder.DropTable(
                name: "OverseasMaterialReprocessingSiteRegistrationMaterial");

            migrationBuilder.DropTable(
                name: "Public.InterimOverseasConnections");

            migrationBuilder.DropTable(
                name: "Public.OverseasAddressContact");

            migrationBuilder.DropTable(
                name: "Public.OverseasAddressWasteCode");

            migrationBuilder.DropTable(
                name: "Public.OverseasMaterialReprocessingSite");

            migrationBuilder.DropTable(
                name: "Public.OverseasAddress");

            migrationBuilder.DropTable(
                name: "Lookup.Country");

            migrationBuilder.DeleteData(
                table: "Lookup.RegulatorTask",
                keyColumn: "Id",
                keyValue: 30);
        }
    }
}
