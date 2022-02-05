using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(StopPreview))]
public class StopPreviewHandler
    : CommandHandler<StopPreview>
{
    private readonly IPreviewService _service;

    public StopPreviewHandler(IPreviewService service)
    {
        _service = service;
    }

    public override async Task<bool> ExecuteAsync(StopPreview command)
    {   
        await _service.StopPreviewAsync();        
        return true;
    }
}