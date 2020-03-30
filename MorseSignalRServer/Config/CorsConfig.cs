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
                            "http://localhost:8080",
                            "https://localhost:8081")
                        .AllowCredentials();
                });

            options.AddPolicy(AllowKnownClientOriginsCorsPolicy,
                builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(
                            "https://baards-vue-client.com",
                            "https://vaagens-react-client.com")
                        .AllowCredentials();
                });
        }
    }
}