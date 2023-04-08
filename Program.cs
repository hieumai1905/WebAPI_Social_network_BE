using Web_Social_network_BE.Middleware;
using Web_Social_network_BE.Models;
using Web_Social_network_BE.Repositories.CommentRepository;
using Web_Social_network_BE.Repositories.LikeRepository;
using Web_Social_network_BE.Repositories.RelationRepository;
using Web_Social_network_BE.Repositories.ImageRepository;
using Web_Social_network_BE.Repositories.PostRepository;
using Web_Social_network_BE.Repositories.UserRepository;

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


            // ----------------------register session/cookie-----------------------------//

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();

            // ------------------------------------------------------------//


            // ----------------------register cors-----------------------------//

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7261")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            // ------------------------------------------------------------//


            // ----------------------add scope-----------------------------//

            builder.Services.AddDbContext<SocialNetworkN01Context>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IRelationRepository, RelationRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRequestCodeRepository, RequestCodeRepository>();

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

            app.UseSession();

            app.UseCors("AllowAllOrigins");

            // app.UseMiddleware<Authentication>();
            //
            // app.UseMiddleware<Authorization>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}