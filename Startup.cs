using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using starter_dotnet_core.Services;

namespace starter_dotnet_core
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
            /// <summary>
            /// Creates a Cosmos DB database and a container with the specified partition key. 
            /// </summary>
            /// <returns></returns>
            private static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync()
            {
                string databaseName = "Car";
                string containerName = "Models";
                string containerNameWheels = "Wheels";
                string account = "https://localhost:8081";
                string key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                CosmosClientBuilder clientBuilder = new CosmosClientBuilder(account, key);
                CosmosClient client = clientBuilder
                                    .WithConnectionModeDirect()
                                    .Build();
                CosmosDbService cosmosDbService = new CosmosDbService(client, databaseName, containerName);
                DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
                //The 2nd parameter is the partion key [MWH]
                await database.Database.CreateContainerIfNotExistsAsync(containerName, "/VehicleModel");
                await database.Database.CreateContainerIfNotExistsAsync(containerNameWheels, "/WheelDimensions/Width");

                return cosmosDbService;
            }
    }
}
