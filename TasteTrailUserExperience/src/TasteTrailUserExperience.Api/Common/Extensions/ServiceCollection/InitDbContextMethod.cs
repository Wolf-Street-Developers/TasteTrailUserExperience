using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using TasteTrailUserExperience.Infrastructure.Common.Data;

namespace TasteTrailUserExperience.Api.Common.Extensions.ServiceCollection;

public static class InitDbContextMethod
{
    public static void InitDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection");
        serviceCollection.AddDbContext<UserExperienceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
