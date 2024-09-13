using dotenv.net;

namespace TasteTrailUserExperience.Api.Common.Utilities;

public static class SetupEnvironmentVariables
{
    public static void SetupEnvironmentVariablesMethod(IConfiguration configuration, bool IsDevelopment)
    {
        var options = new DotEnvOptions(envFilePaths: ["../../../.env"]);
        DotEnv.Load(options);

        string? postgresConnectionString;

        // Setup Db Connection String
        if (IsDevelopment) {
            var postgresHost = "localhost";
            var postgresPort = "6500";
            var postgresUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var postgresDb = Environment.GetEnvironmentVariable("POSTGRES_DB");

            postgresConnectionString = $"Host={postgresHost};Port={postgresPort};Username={postgresUser};Password={postgresPassword};Database={postgresDb};Pooling=true;";
        }
        else
            postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

        System.Console.WriteLine("SETUP");
        System.Console.WriteLine(postgresConnectionString);

        if (!string.IsNullOrEmpty(postgresConnectionString)) {
            configuration["ConnectionStrings:PostgresConnection"] = postgresConnectionString;
        }

        System.Console.WriteLine(postgresConnectionString);

        // Setup JWT
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtLifeTimeInMinutes = Environment.GetEnvironmentVariable("JWT_LIFE_TIME_IN_MINUTES");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");


        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtLifeTimeInMinutes)
        || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience)) {
            throw new ArgumentException("Cannot find JWT configurations in environment variables.");
        }

        configuration["Jwt:Key"] = jwtKey;
        configuration["Jwt:LifeTimeInMinutes"] = jwtLifeTimeInMinutes;
        configuration["Jwt:Issuer"] = jwtIssuer;
        configuration["Jwt:Audience"] = jwtAudience;
    }
}
