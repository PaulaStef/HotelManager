using BusinessLayer;
using BusinessLayer.Contracts;
using BusinessLayer.Contracts.Factory;
using BusinessLayer.Contracts.Interfaces;
using BusinessLayer.Contracts.Services;
using DataAccess;
using DataAccess.Contracts;
using DataAccess.Contracts.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LayersOnWeb
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
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});

            services.AddSwaggerGen();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IApartmentService, ApartmentService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IDocumentFactory, DocumentFactory>();
            services.AddDbContext<WeatherDbContext>(options => options.UseSqlServer(@"Server=.;Database=dbhotel;Trusted_Connection=True;"));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<WeatherDbContext>()
                  .AddDefaultTokenProviders();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            
            CreateUserRoles(serviceProvider).Wait();
            CreateStartupUsers(serviceProvider);
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Administrator");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            roleCheck = await RoleManager.RoleExistsAsync("Receptionist");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                await RoleManager.CreateAsync(new IdentityRole("Receptionist"));
            }
        }

        private void CreateStartupUsers(IServiceProvider serviceProvider)
        {
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var users = userMgr.Users;
            if (!users.Any(x=> x.UserName == "admin@webdotnet.com"))
            {
                var user = new ApplicationUser { UserName = "admin@webdotnet.com" };
                userMgr.CreateAsync(user,  "P@ssw0rd").Wait();
                var registeredUser = userMgr.FindByNameAsync(user.UserName).Result;
                userMgr.AddToRoleAsync(registeredUser, "Administrator").Wait();
            }
        }
    }
}
