using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TasteTrailUserExperience.Core.Common.Options;
using TasteTrailUserExperience.Core.Feedbacks.Repositories;
using TasteTrailUserExperience.Core.Users.Models;
using TasteTrailUserExperience.Core.Users.Repositories;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Base;

namespace TasteTrailUserExperience.Infrastructure.BackgroundServices;

public class UserRabbitMqService : BaseRabbitMqService, IHostedService
{
    public UserRabbitMqService(IOptions<RabbitMqOptions> optionsSnapshot, IServiceScopeFactory serviceScopeFactory) :
        base(optionsSnapshot, serviceScopeFactory)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        base.StartListening("user_update_userexperience", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                System.Console.WriteLine("USER UPDATE");
                System.Console.WriteLine(message);

                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var feedbackRepositroy = scope.ServiceProvider.GetRequiredService<IFeedbackRepository>();

                var updatedUser = JsonSerializer.Deserialize<User>(message)!;
                await userRepository.PutAsync(updatedUser);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
