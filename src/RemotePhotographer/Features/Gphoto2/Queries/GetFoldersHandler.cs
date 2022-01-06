using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographer.Features.Gphoto2.Extensions;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetFolders))]
public class GetFoldersHandler
    : QueryHandler<GetFolders>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;

    public GetFoldersHandler(IModelService service, ICameraContextManager manager) 
    {
        _service = service;
        _manager = manager;
    }

    public override Task<IModel> ExecuteAsync(GetFolders query)
    {
        var model = new GetFoldersModel();

        ListService.gp_list_new(out IntPtr list);

        var status = CameraService.gp_camera_folder_list_folders(
            _manager.CameraContext.Camera, 
            query.Path.ConvertToSByte(), 
            list, 
            _manager.CameraContext.Context
        );

        var count = ListService.gp_list_count(list);

        for(int index = 0; index < count; index++) 
        {
            ListService.gp_list_get_name(list, index, out IntPtr namePointer);

            string name = Marshal.PtrToStringAnsi(namePointer);
            model.Add(System.IO.Path.Combine(query.Path, name));
        }

        ListService.gp_list_free(list);

        return Task.FromResult((IModel)model);
    }
}