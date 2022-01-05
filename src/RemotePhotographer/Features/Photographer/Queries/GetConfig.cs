using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Photographer.Services;

namespace RemotePhotographer.Features.Photographer.Queries;

public class GetConfig 
    : Query
{
}

public class GetConfigModel 
    : IModel
{
}

public class GetConfigHandler
    : QueryHandler<GetConfig>
{
    private readonly ICameraService _service;

    public GetConfigHandler(ICameraService service)
    {
        _service = service;
    }

    public override Task<IModel> ExecuteAsync(GetConfig query)
    {
        var model = new GetConfigModel();

        return Task.FromResult<IModel>((IModel)model);
    }
}