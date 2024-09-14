using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TasteTrailUserExperience.Core.Common.Options;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Core.MenuItems.Repositories;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Base;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Dtos;

namespace TasteTrailUserExperience.Infrastructure.BackgroundServices;

public class MenuItemRabbitMqService : BaseRabbitMqService, IHostedService
{
    public MenuItemRabbitMqService(IOptions<RabbitMqOptions> optionsSnapshot, IServiceScopeFactory serviceScopeFactory) :
        base(optionsSnapshot, serviceScopeFactory)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        base.StartListening("menuitem_create", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                System.Console.WriteLine("CREATE");
                System.Console.WriteLine(message);
                var menuItemRepository = scope.ServiceProvider.GetRequiredService<IMenuItemRepository>();

                var newMenuItem = JsonSerializer.Deserialize<MenuItem>(message)!;
                await menuItemRepository.CreateAsync(newMenuItem);
            }
        });

        base.StartListening("menuitem_put", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                System.Console.WriteLine("UPDATE");
                System.Console.WriteLine(message);
                var menuItemRepository = scope.ServiceProvider.GetRequiredService<IMenuItemRepository>();

                var newMenuItem = JsonSerializer.Deserialize<MenuItem>(message)!;
                await menuItemRepository.PutAsync(newMenuItem);
            }
        });

        base.StartListening("menuitem_delete", async message => {
            System.Console.WriteLine("DELETE");
            System.Console.WriteLine(message);
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuItemRepository = scope.ServiceProvider.GetRequiredService<IMenuItemRepository>();

                int.TryParse(message, out int deletedId);
                await menuItemRepository.DeleteByIdAsync(deletedId);
            }
        });

        base.StartListening("menuitem_set_image", async message => {
            System.Console.WriteLine("SET IMAGE");
            System.Console.WriteLine(message);
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuItemRepository = scope.ServiceProvider.GetRequiredService<IMenuItemRepository>();

                var imageMessage = JsonSerializer.Deserialize<ImageMessageDto>(message)!;
                var menuItemToUpdate = await menuItemRepository.GetByIdAsync(imageMessage.EntityId);

                menuItemToUpdate!.ImageUrlPath = imageMessage.ImageUrl;
                await menuItemRepository.PutAsync(menuItemToUpdate!);
            }
        });

        base.StartListening("menuitem_delete_image", async message => {
            System.Console.WriteLine("DELETE IMAGE");
            System.Console.WriteLine(message);
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var menuItemRepository = scope.ServiceProvider.GetRequiredService<IMenuItemRepository>();

                var imageMessage = JsonSerializer.Deserialize<ImageMessageDto>(message)!;
                var menuItemToUpdate = await menuItemRepository.GetByIdAsync(imageMessage.EntityId);

                menuItemToUpdate!.ImageUrlPath = imageMessage.ImageUrl;
                await menuItemRepository.PutAsync(menuItemToUpdate!);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
