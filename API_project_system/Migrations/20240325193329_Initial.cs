using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_project_system.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "approval_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_approval_statuses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    status_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "user_ibfk_1",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_ibfk_2",
                        column: x => x.status_id,
                        principalTable: "approval_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icon_path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    owner_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses", x => x.id);
                    table.ForeignKey(
                        name: "fk_courses_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "assigments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deadline_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    visibility = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    course_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assigments", x => x.id);
                    table.ForeignKey(
                        name: "fk_assigments_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "courses_enrolled_users",
                columns: table => new
                {
                    enrolled_courses_id = table.Column<int>(type: "int", nullable: false),
                    enrolled_users_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses_enrolled_users", x => new { x.enrolled_courses_id, x.enrolled_users_id });
                    table.ForeignKey(
                        name: "fk_courses_enrolled_users_courses_enrolled_courses_id",
                        column: x => x.enrolled_courses_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_courses_enrolled_users_users_enrolled_users_id",
                        column: x => x.enrolled_users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "courses_pending_users",
                columns: table => new
                {
                    pending_courses_id = table.Column<int>(type: "int", nullable: false),
                    pending_users_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses_pending_users", x => new { x.pending_courses_id, x.pending_users_id });
                    table.ForeignKey(
                        name: "fk_courses_pending_users_courses_pending_courses_id",
                        column: x => x.pending_courses_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_courses_pending_users_users_pending_users_id",
                        column: x => x.pending_users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "submissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    assignment_id = table.Column<int>(type: "int", nullable: false),
                    submission_date_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    student_comment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_submissions_assignments_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "assigments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_submissions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "submission_files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    submission_id = table.Column<int>(type: "int", nullable: false),
                    file_path = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_submission_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_submission_files_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_assigments_course_id",
                table: "assigments",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses_owner_id",
                table: "courses",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses_enrolled_users_enrolled_users_id",
                table: "courses_enrolled_users",
                column: "enrolled_users_id");

            migrationBuilder.CreateIndex(
                name: "ix_courses_pending_users_pending_users_id",
                table: "courses_pending_users",
                column: "pending_users_id");

            migrationBuilder.CreateIndex(
                name: "ix_submission_files_submission_id",
                table: "submission_files",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_assignment_id",
                table: "submissions",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_submissions_user_id",
                table: "submissions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_status_id",
                table: "users",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courses_enrolled_users");

            migrationBuilder.DropTable(
                name: "courses_pending_users");

            migrationBuilder.DropTable(
                name: "submission_files");

            migrationBuilder.DropTable(
                name: "submissions");

            migrationBuilder.DropTable(
                name: "assigments");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "approval_statuses");
        }
    }
}
