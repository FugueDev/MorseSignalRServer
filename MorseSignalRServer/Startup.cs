using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MorseSignalRServer.Rewrite;
using MorseSignalRServer.Config;
using MorseSignalRServer.Hubs.Channel;
using MorseSignalRServer.Hubs.Lobby;
using MorseSignalRServer.Hubs.Room;

namespace MorseSignalRServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Load Kestrel configurations from appsettings
            services.Configure<KestrelServerOptions>(
                Configuration.GetSection("Kestrel"));

            // Forwarding of X-headers (For when hosted behind an inverse proxy)
            services.Configure<ForwardedHeadersOptions>(options => {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.0"));
            });
 
            services.AddSignalR();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                }));
            services.AddCors(CorsConfig.Configure);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
        //    if (env.IsDevelopment())
        //    {
       //         app.UseDeveloperExceptionPage();
                
         //       app.UseCors(CorsConfig.AllowKnownLocalClientOriginsCorsPolicy);
       //     } else {
                // HTTPS redirect rule for runing behind an inverse proxy
                var options = new RewriteOptions()
                    .AddRedirectToProxiedHttps()
                    .AddRedirect("(.*)/$", "$1");  // remove trailing slash

                app.UseRewriter(options);

                app.UseCors(CorsConfig.AllowKnownClientOriginsCorsPolicy);
        //    }
            
            
            //app.UseCors("CorsPolicy");
           app.UseForwardedHeaders();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RoomHub>("/Room");
                endpoints.MapHub<LobbyHub>("/Lobby");
                endpoints.MapHub<ChannelHub>("/Channel");
            });
        }
    }
}