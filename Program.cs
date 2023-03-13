using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.CommentsRepository;

namespace Web_Social_network_BE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ----------------------add scope-----------------------------//

            builder.Services.AddControllers();

            // ------------------------------------------------------------//


            // ----------------------add scope-----------------------------//

            builder.Services.AddDbContext<SocialNetworkN01Context>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            // ------------------------------------------------------------//

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}