using Freya.Desktop.Core.Features.Clients.Models;

namespace Freya.Features.Clients.Extensions;

public static class ServiceProviderMultimediaExtensions
{
    public static ImageMultimediaViewModel? ConvertToImageMultimediaViewModel(this IServiceProvider serviceProvider, ClientDetailViewModel parent, ImageMultimedia item)
    {
        if (serviceProvider.GetService(typeof(ImageMultimediaViewModel)) is ImageMultimediaViewModel vm)
        {
            vm.Data = item;
            vm.Parent = parent;

            return vm;
        }

        return null;
    }
    
    public static VideoMultimediaViewModel? ConvertToVideoMultimediaViewModel(this IServiceProvider serviceProvider, ClientDetailViewModel parent, VideoMultimedia item)
    {
        if (serviceProvider.GetService(typeof(VideoMultimediaViewModel)) is VideoMultimediaViewModel vm)
        {
            vm.Data = item;
            vm.Parent = parent;

            return vm;
        }

        return null;
    }

    public static SoundMultimediaViewModel? ConvertToSoundMultimediaViewModel(this IServiceProvider serviceProvider, ClientDetailViewModel parent, SoundMultimedia item)
    {
        if (serviceProvider.GetService(typeof(SoundMultimediaViewModel)) is SoundMultimediaViewModel vm)
        {
            vm.Data = item;
            vm.Parent = parent;

            return vm;
        }

        return null;
    }
}
