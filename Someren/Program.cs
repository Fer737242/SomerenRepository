using Someren.Repositories;

namespace Someren
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.Services.AddSingleton<ILecturersRepository, DbLecturersRepository>();
            builder.Services.AddSingleton<IActivitiesRepository, DbActivitiesRepository>();
            builder.Services.AddSingleton<IRoomsRepository, DbRoomsRepository>();
            builder.Services.AddSingleton<IParticipationRepository, ParticipationRepository>();
            builder.Services.AddScoped<IStudentsRepository, DbStudentsRepository>();
            builder.Services.AddScoped<IParticipationRepository, ParticipationRepository>();
            builder.Services.AddScoped<IDrinkRepository, DbDrinkRepository>();
            builder.Services.AddScoped<ISupervisedBy, DbSupervisedBy>(); // Add this line

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}