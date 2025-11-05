using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jwtmanualauthentication.Migrations
{
    /// <inheritdoc />
    public partial class actorsandmoviesmanytomanyrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ActorMovie");

            migrationBuilder.CreateTable(
                name: "ActorMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActorMovies_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "ActorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovies_ActorId",
                table: "ActorMovies",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorMovies_MovieId",
                table: "ActorMovies",
                column: "MovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ActorMovies");

            //migrationBuilder.CreateTable(
            //    name: "ActorMovie",
            //    columns: table => new
            //    {
            //        ActorsActorId = table.Column<int>(type: "int", nullable: false),
            //        MoviesMovieId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ActorMovie", x => new { x.ActorsActorId, x.MoviesMovieId });
            //        table.ForeignKey(
            //            name: "FK_ActorMovie_Actors_ActorsActorId",
            //            column: x => x.ActorsActorId,
            //            principalTable: "Actors",
            //            principalColumn: "ActorId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ActorMovie_Movies_MoviesMovieId",
            //            column: x => x.MoviesMovieId,
            //            principalTable: "Movies",
            //            principalColumn: "MovieId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ActorMovie_MoviesMovieId",
            //    table: "ActorMovie",
            //    column: "MoviesMovieId");
        }
    }
}
