
using Boilerplate.Features.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using RemotePhotographer.Features.Photographer.Queries;

namespace RemotePhotographer.Features.Photographer.Controllers;

[Route("/api/photographer"), ApiController]
public class ImageController
    : ControllerBase
{
    private readonly IQueryDispatcher _dispatcher;

    public ImageController(IQueryDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet, Route("ping")]
    public string Ping() 
    {
        return "Pong";
    }

    [HttpGet, Route("images")]
    public async Task<object> GetImage([FromQuery] string path) 
    {
        var model = await _dispatcher.DispatchAsync<GetImageModel>(
            new GetImage(path)
        );

        return File(model.Data, "image/jpg");
    }
}