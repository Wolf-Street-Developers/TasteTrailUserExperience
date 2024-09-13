using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TasteTrailUserExperience.Core.Common.Options;
using TasteTrailUserExperience.Core.Venues.Models;
using TasteTrailUserExperience.Core.Venues.Repositories;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Base;
using TasteTrailUserExperience.Infrastructure.BackgroundServices.Dtos;

namespace TasteTrailUserExperience.Infrastructure.BackgroundServices;

public class VenueRabbitMqService : BaseRabbitMqService, IHostedService
{
    public VenueRabbitMqService(IOptions<RabbitMqOptions> optionsSnapshot, IServiceScopeFactory serviceScopeFactory) :
        base(optionsSnapshot, serviceScopeFactory)
    {
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        base.StartListening("venue_create", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var venueRepository = scope.ServiceProvider.GetRequiredService<IVenueRepository>();

                var newVenue = JsonSerializer.Deserialize<Venue>(message)!;
                await venueRepository.CreateAsync(newVenue);
            }
        });

        base.StartListening("venue_put", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var venueRepository = scope.ServiceProvider.GetRequiredService<IVenueRepository>();

                var updatedVenue = JsonSerializer.Deserialize<Venue>(message)!;
                await venueRepository.PutAsync(updatedVenue);
            }
        });

        base.StartListening("venue_delete", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var venueRepository = scope.ServiceProvider.GetRequiredService<IVenueRepository>();
                

                int.TryParse(message, out int deletedId);
                await venueRepository.DeleteByIdAsync(deletedId);
            }
        });

        base.StartListening("venue_set_image", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var venueRepository = scope.ServiceProvider.GetRequiredService<IVenueRepository>();

                var imageMessage = JsonSerializer.Deserialize<ImageMessageDto>(message)!;
                var venueToUpdate = await venueRepository.GetByIdAsync(imageMessage.EntityId);

                venueToUpdate!.LogoUrlPath = imageMessage.ImageUrl;
                await venueRepository.PutAsync(venueToUpdate!);
            }
        });

        base.StartListening("venue_delete_image", async message => {
            using (var scope = base.serviceScopeFactory.CreateScope())
            {
                var venueRepository = scope.ServiceProvider.GetRequiredService<IVenueRepository>();
                System.Console.WriteLine();

                var imageMessage = JsonSerializer.Deserialize<ImageMessageDto>(message)!;
                var venueToUpdate = await venueRepository.GetByIdAsync(imageMessage.EntityId);
                
                venueToUpdate!.LogoUrlPath = imageMessage.ImageUrl;
                await venueRepository.PutAsync(venueToUpdate!);
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
