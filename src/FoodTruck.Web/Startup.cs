using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using FoodTruck.Core.Interfaces;
using FoodTruck.Infrastructure;
using FoodTruck.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using TinyCsvParser;

namespace FoodTruck.Web
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
            services.AddDbContext<FoodTruckContext>(opt => opt.UseInMemoryDatabase("FoodTruckDb"));
            services.AddScoped<IFoodTruckProvider, FoodTruckProvider>();

            services.AddHealthChecks()
                    .AddCheck("FoodTruckService", () => HealthCheckResult.Healthy("Servie is healthy!"));

            services.AddControllers()
                    .AddJsonOptions(opts =>
                    {
                        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FoodTruckContext context)
        {
            // Initialize the DB with dat from the food truck CSV
            //var context = app.ApplicationServices.GetService<FoodTruckDbContext>();
            InitData(context);

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
                endpoints.MapHealthChecks("/healthz");
            });
        }


        /// <summary>
        /// Parse and store the data records from the food truck CSV
        /// </summary>
        /// <param name="context"></param>
        private void InitData(FoodTruckContext context)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<Core.Models.FoodTruck>(csvParserOptions, new CsvFoodTruckMapping());
            var records = csvParser.ReadFromFile("Resources/Mobile_Food_Facility_Permit.csv", Encoding.UTF8).ToList();

            foreach (var record in records)
            {
                if (record.IsValid)
                    context.FoodTrucks.Add(record.Result);
            }

            context.SaveChanges();
        }
    }
}
