using BuisnessLayer.Jobs;
using BuisnessLayer.Workers;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataLayer.Repos;

namespace Library
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<JobFactory>();
            services.AddTransient<DataJob>();
            services.AddTransient<IOrderChecker, OrderChecker>();

            services.AddTransient(typeof(IBookRepository<>), typeof(BookRepository<>));
            services.AddTransient(typeof(IOrderRepository<>), typeof(OrderRepository<>));

            services.AddDbContextPool<LibraryDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("DataLayer")));

            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 5;   // минимальная длина
                opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуются ли цифры
                opts.User.RequireUniqueEmail = true;    // уникальный email
            })
                .AddEntityFrameworkStores<LibraryDbContext>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Books}/{action=Index}/{id?}");
            });

            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            var scope = scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await RoleInitializer.InitializeAsync(userManager, rolesManager);

            DataScheduler.Start(scope.ServiceProvider);
        }
    }
}
