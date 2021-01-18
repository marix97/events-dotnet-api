using AutoMapper;
using EventAPI.Domain;
using EventAPI.Domain.Interfaces;
using EventAPI.Entities.DatabaseContext;
using EventAPI.Helpers;
using EventAPI.Repositories;
using EventAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Mail;

namespace EventAPI
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
            var emailConfig = Configuration.GetSection("EmailConfiguration")
            .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);

            services.AddDbContext<EventsAPIContext>(opts =>
            opts.UseSqlServer(Configuration["ConnectionString:DBConnection"]));

            services.AddAutoMapper(System.Reflection.Assembly.GetExecutingAssembly());

            services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventGuestRepository, EventGuestRepository>();

            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventGuestService, EventGuestService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<MailMessage>();
            services.AddScoped<ICalendarService, CalendarService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
