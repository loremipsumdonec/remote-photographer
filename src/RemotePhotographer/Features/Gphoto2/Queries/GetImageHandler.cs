using System.Runtime.InteropServices;
using System.Text;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Queries;
using RemotePhotographer.Features.Gphoto2.Extensions;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetImage))]
public class GetImageHandler
    : QueryHandler<GetImage>
{
    private readonly ICameraContextManager _manager;

    public GetImageHandler(ICameraContextManager manager) 
    {
        _manager = manager;
    }

    public override Task<IModel> ExecuteAsync(GetImage query)
    {
        var model = new GetImageModel();

        string folder = System.IO.Path.GetDirectoryName(query.Path);
        string name = System.IO.Path.GetFileName(query.Path);

        FileService.gp_file_new(out IntPtr file);
        
        var fileGetStatus = CameraService.gp_camera_file_get(
            _manager.CameraContext.Camera, 
            folder.ConvertToSByte(), 
            name.ConvertToSByte(), 
            1, 
            file, 
            _manager.CameraContext.Context
        );

        var fileGetDataAndSizeStatus = FileService.gp_file_get_data_and_size(file, out IntPtr data, out ulong size);
        
        model.Data = new byte[size];
        Marshal.Copy(data, model.Data, 0, model.Data.Length);

        FileService.gp_file_free(file);

        return Task.FromResult((IModel)model);
    }

}