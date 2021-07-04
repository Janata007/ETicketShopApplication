using ETicket.Domain.Identity;
using ETicket.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicket.Repository;
using ETicket.Repository.Interface;
using ETicket.Repository.Implementation;
using ETicket.Services.Interface;
using ETicket.Services.Implementation;
using ETicket.Repository.Interface;
using ETicket.Services;
using Stripe;

namespace ETicket.Web
{
    public class Startup
    {


        private EmailSettings emailService; 

        public Startup(IConfiguration configuration)
        {

            emailService = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailService);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ETicketApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));


            services.AddTransient<IMovieService, Services.Implementation.MovieService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IOrderService, Services.Implementation.OrderService>();

            //services.AddScoped<EmailSettings>(es => emailService);
            //services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailService));
            //services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
            //services.AddHostedService<ConsumeScopedHostedService>();


            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddControllersWithViews()            
                    .AddNewtonsoftJson(options =>
          options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
                        
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["sk_test_51J7fvFGo3LBE0wkApE6ZmX0Y7BUZiC1BQnvbOttSnlfsJYQK6xqYwAYeJKgeAZZu4Pdty4rQsk3hNkY52gAHCQeW00kCCTAfxn"]);
             
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
