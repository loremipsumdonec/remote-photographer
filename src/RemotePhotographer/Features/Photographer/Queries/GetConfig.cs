using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;

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
    public override Task<IModel> ExecuteAsync(GetConfig query)
    {
        var model = new GetConfigModel();

        return Task.FromResult<IModel>((IModel)model);
    }
}