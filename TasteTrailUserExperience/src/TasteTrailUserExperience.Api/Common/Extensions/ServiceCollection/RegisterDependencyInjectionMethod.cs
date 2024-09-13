using TasteTrailUserExperience.Core.FeedbackLikes.Repositories;
using TasteTrailUserExperience.Core.FeedbackLikes.Services;
using TasteTrailUserExperience.Core.Feedbacks.Repositories;
using TasteTrailUserExperience.Core.Feedbacks.Services;
using TasteTrailUserExperience.Core.MenuItemLikes.Repositories;
using TasteTrailUserExperience.Core.MenuItemLikes.Services;
using TasteTrailUserExperience.Core.MenuItems.Repositories;
using TasteTrailUserExperience.Core.MenuItems.Services;
using TasteTrailUserExperience.Core.Menus.Repositories;
using TasteTrailUserExperience.Core.Menus.Services;
using TasteTrailUserExperience.Core.Users.Repositories;
using TasteTrailUserExperience.Core.Venues.Repositories;
using TasteTrailUserExperience.Core.Venues.Services;
using TasteTrailUserExperience.Infrastructure.FeedbackLikes.Repositories;
using TasteTrailUserExperience.Infrastructure.FeedbackLikes.Services;
using TasteTrailUserExperience.Infrastructure.Feedbacks.Repositories;
using TasteTrailUserExperience.Infrastructure.Feedbacks.Services;
using TasteTrailUserExperience.Infrastructure.MenuItemLikes.Repositories;
using TasteTrailUserExperience.Infrastructure.MenuItemLikes.Services;
using TasteTrailUserExperience.Infrastructure.MenuItems.Repositories;
using TasteTrailUserExperience.Infrastructure.MenuItems.Services;
using TasteTrailUserExperience.Infrastructure.Menus.Repositories;
using TasteTrailUserExperience.Infrastructure.Menus.Services;
using TasteTrailUserExperience.Infrastructure.Users.Repositories;
using TasteTrailUserExperience.Infrastructure.Venues.Repositories;
using TasteTrailUserExperience.Infrastructure.Venues.Services;

namespace TasteTrailUserExperience.Api.Common.Extensions.ServiceCollection;

public static class RegisterDependencyInjectionMethod
{
    public static void RegisterDependencyInjection(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IVenueRepository, VenueEfCoreRepository>();
        serviceCollection.AddTransient<IMenuRepository, MenuEfCoreRepository>();
        serviceCollection.AddTransient<IMenuItemRepository, MenuItemEfCoreRepository>();
        serviceCollection.AddTransient<IFeedbackRepository, FeedbackEfCoreRepository>();
        serviceCollection.AddTransient<IFeedbackLikeRepository, FeedbackLikeEfCoreRepository>();
        serviceCollection.AddTransient<IMenuItemLikeRepository, MenuItemLikeEfCoreRepository>();
        serviceCollection.AddTransient<IUserRepository, UserEfCoreRepository>();

        serviceCollection.AddTransient<IVenueService, VenueService>();
        serviceCollection.AddTransient<IMenuService, MenuService>();
        serviceCollection.AddTransient<IMenuItemService, MenuItemService>();
        serviceCollection.AddTransient<IFeedbackService, FeedbackService>();
        serviceCollection.AddTransient<IFeedbackLikeService, FeedbackLikeService>();
        serviceCollection.AddTransient<IMenuItemLikeService, MenuItemLikeService>();
    } 
}
