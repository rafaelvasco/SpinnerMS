using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpinnerMS.Services;

namespace SpinnerMS
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
            var db_config = Configuration.GetSection("SpinnerDB");

            var db_name = db_config.GetSection("DatabaseName").Value;

            var database_client = InitDatabase(db_config).GetAwaiter().GetResult();


            services.AddControllers();
            services.AddSingleton<IPlayerService>(InitPlayerService(database_client, db_name));
            services.AddSingleton<IMatchService>(InitMatchService(database_client, db_name));
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        private async Task<CosmosClient> InitDatabase(IConfigurationSection config)
        {
            var database_name = config.GetSection("DatabaseName").Value;
            var account = config.GetSection("Account").Value;
            var key = config.GetSection("Key").Value;
            var client_builder = new CosmosClientBuilder(account, key);
            var client = client_builder
                .WithConnectionModeDirect()
                .Build();

            var database = await client.CreateDatabaseIfNotExistsAsync(database_name);

            await database.Database.CreateContainerIfNotExistsAsync("Players", "/id");
            await database.Database.CreateContainerIfNotExistsAsync("Matches", "/id");

            return client;
        }

        private static PlayerService InitPlayerService(CosmosClient db_client, string data_base_name)
        {
            var player_service = new PlayerService(db_client, data_base_name, "Players");

            return player_service;
        }

        public static MatchService InitMatchService(CosmosClient db_client, string data_base_name)
        {
            var match_service = new MatchService(db_client, data_base_name, "Matches");

            return match_service;
        }

    }
}
