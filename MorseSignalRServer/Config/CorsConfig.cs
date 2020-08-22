using Microsoft.AspNetCore.Cors.Infrastructure;

namespace MorseSignalRServer.Config {
    public static class CorsConfig {
        public static string AllowAllOriginsCorsPolicy = "AllowAllOrigins";
        public static string AllowKnownLocalClientOriginsCorsPolicy = "AllowKnownLocalClientOriginsCorsPolicy";
        public static string AllowKnownClientOriginsCorsPolicy = "AllowKnownClientsOrigins";
        public static void Configure(CorsOptions options) {
            options.AddPolicy(AllowAllOriginsCorsPolicy,
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

            options.AddPolicy(AllowKnownLocalClientOriginsCorsPolicy,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(
                            "http://localhost:8008",
                            "http://localhost:8080",
                            "http://localhost:80",
                            "https://localhost:8009")
                        .AllowCredentials();
                });

            options.AddPolicy(AllowKnownClientOriginsCorsPolicy,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin()
                        //.WithOrigins(
                          //  "http://morsechat.io",
                            //"https://morsechat.io")
                        .AllowCredentials();
                });
        }
    }
}