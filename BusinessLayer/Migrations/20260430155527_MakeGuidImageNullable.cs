using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miners.Web.BusinessLayer.Migrations
{
    /// <inheritdoc />
    public partial class MakeGuidImageNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sch_tsfw4");

            migrationBuilder.EnsureSchema(
                name: "sch_miners");

            migrationBuilder.CreateTable(
                name: "cis_fronta_stav",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    popis = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cis_fronta_stav", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_article",
                schema: "sch_miners",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    guid_image = table.Column<Guid>(type: "uuid", nullable: true),
                    top = table.Column<bool>(type: "boolean", nullable: false),
                    url_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_article", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tbl_category",
                schema: "sch_miners",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    code = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_category", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tbl_file",
                schema: "sch_miners",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    filename = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    size = table.Column<int>(type: "integer", nullable: false),
                    data = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_file", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tbl_job_definice",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    trigger_type = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    period = table.Column<long>(type: "bigint", nullable: true),
                    times = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    days = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    timeout = table.Column<int>(type: "integer", nullable: false),
                    appsettings = table.Column<bool>(type: "boolean", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_running = table.Column<bool>(type: "boolean", nullable: false),
                    last_run = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_run_ok = table.Column<bool>(type: "boolean", nullable: false),
                    log_only_error = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_job_definice", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_person",
                schema: "sch_miners",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    jmeno = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    prijmeni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    telefon = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    pozice_tym = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    licence = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    poznamka = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    foto = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_person", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    global = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scoreboard",
                schema: "sch_miners",
                columns: table => new
                {
                    live_tv = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    live_tv_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tbl_team",
                schema: "sch_miners",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    category = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    training_days = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fee = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    league = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    coach1_function = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    coach2_function = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    coach3_function = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    coach4_function = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    guid_coach1 = table.Column<Guid>(type: "uuid", nullable: true),
                    guid_coach2 = table.Column<Guid>(type: "uuid", nullable: true),
                    guid_coach3 = table.Column<Guid>(type: "uuid", nullable: true),
                    guid_coach4 = table.Column<Guid>(type: "uuid", nullable: true),
                    team_photo = table.Column<Guid>(type: "uuid", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: false),
                    treneri_kategorie = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_team", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    login = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    first_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    last_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    degree_before = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    degree_after = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    guid_tenant = table.Column<Guid>(type: "uuid", nullable: true),
                    security_stamp = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    dn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    application = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_token",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_user_token", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_games",
                schema: "sch_miners",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_date = table.Column<DateOnly>(type: "date", nullable: false),
                    league = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    home_team = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    away_team = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    home_score = table.Column<int>(type: "integer", nullable: true),
                    away_score = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_games", x => x.guid);
                    table.ForeignKey(
                        name: "fk_tbl_games_category_id",
                        column: x => x.category_id,
                        principalSchema: "sch_miners",
                        principalTable: "tbl_category",
                        principalColumn: "guid");
                });

            migrationBuilder.CreateTable(
                name: "tbl_job",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    job_definice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    startdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    enddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    succeeded = table.Column<bool>(type: "boolean", nullable: false),
                    error_message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    error_stack = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    queuedate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    argument = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_job", x => x.id);
                    table.ForeignKey(
                        name: "fk_tbl_job_job_definice_id",
                        column: x => x.job_definice_id,
                        principalSchema: "sch_tsfw4",
                        principalTable: "tbl_job_definice",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbl_membership",
                schema: "sch_tsfw4",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guid_tenant = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tbl_membership", x => x.id);
                    table.ForeignKey(
                        name: "fk_tbl_membership_role_id",
                        column: x => x.role_id,
                        principalSchema: "sch_tsfw4",
                        principalTable: "tbl_role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tbl_membership_user_id",
                        column: x => x.user_id,
                        principalSchema: "sch_tsfw4",
                        principalTable: "tbl_user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_tbl_category_code",
                schema: "sch_miners",
                table: "tbl_category",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_tbl_games_category_id",
                schema: "sch_miners",
                table: "tbl_games",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "fk_tbl_job_job_definice_id",
                schema: "sch_tsfw4",
                table: "tbl_job",
                column: "job_definice_id");

            migrationBuilder.CreateIndex(
                name: "fk_tbl_membership_role_id",
                schema: "sch_tsfw4",
                table: "tbl_membership",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "fk_tbl_membership_user_id",
                schema: "sch_tsfw4",
                table: "tbl_membership",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cis_fronta_stav",
                schema: "sch_tsfw4");

            migrationBuilder.DropTable(
                name: "tbl_article",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_file",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_games",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_job",
                schema: "sch_tsfw4");

            migrationBuilder.DropTable(
                name: "tbl_membership",
                schema: "sch_tsfw4");

            migrationBuilder.DropTable(
                name: "tbl_person",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_scoreboard",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_team",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_user_token",
                schema: "sch_tsfw4");

            migrationBuilder.DropTable(
                name: "tbl_category",
                schema: "sch_miners");

            migrationBuilder.DropTable(
                name: "tbl_job_definice",
                schema: "sch_tsfw4");

            migrationBuilder.DropTable(
                name: "tbl_role",
                schema: "sch_tsfw4");

            migrationBuilder.DropTable(
                name: "tbl_user",
                schema: "sch_tsfw4");
        }
    }
}
