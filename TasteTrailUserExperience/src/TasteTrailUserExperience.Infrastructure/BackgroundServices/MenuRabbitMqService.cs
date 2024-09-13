using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TasteTrailUserExperience.Core.Common.Options;
using TasteTrailUserExperience.Core.Menus.Models;
using TasteTrailUserExperience.Core.Menus.Repositories;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Base;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Dtos;

namespace TasteTrailUserExperience.Infrastructure.BackgroundServices;

public class MenuRabbitMqService : BaseRabbitMqService, IHostedService
{
    public MenuRabbitMqService(IOptions<RabbitMqOptions> optionsSnapshot, IServiceScopeFactory serviceScopeFactory) :
        base(optionsSnapshot, serviceScopeFactory)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        base.StartListening("menu_create", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();

                var newMenu = JsonSerializer.Deserialize<Menu>(message)!;
                await menuRepository.CreateAsync(newMenu);
            }
        });

        base.StartListening("menu_put", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();

                var updatedMenu = JsonSerializer.Deserialize<Menu>(message)!;
                await menuRepository.PutAsync(updatedMenu);
            }
        });

        base.StartListening("menu_delete", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();

                int.TryParse(message, out int deletedId);
                await menuRepository.DeleteByIdAsync(deletedId);
            }
        });

        base.StartListening("menu_set_image", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();

                var imageMessage = JsonSerializer.Deserialize<ImageMessageDto>(message)!;
                var menuToUpdate = await menuRepository.GetByIdAsync(imageMessage.EntityId);

                menuToUpdate!.ImageUrlPath = imageMessage.ImageUrl;
                await menuRepository.PutAsync(menuToUpdate!);
            }
        });

        base.StartListening("menu_delete_image", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
                System.Console.WriteLine();

                var imageMessage = JsonSerializer.Deserialize<ImageMessageDto>(message)!;
                var menuToUpdate = await menuRepository.GetByIdAsync(imageMessage.EntityId);
                
                menuToUpdate!.ImageUrlPath = imageMessage.ImageUrl;
                await menuRepository.PutAsync(menuToUpdate!);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
