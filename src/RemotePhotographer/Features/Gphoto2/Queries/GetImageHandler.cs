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
    private readonly IMethodValidator _validator;

    public GetImageHandler(ICameraContextManager manager, IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetImage query)
    {
        var model = new GetImageModel();

        string folder = System.IO.Path.GetDirectoryName(query.Path);
        string name = System.IO.Path.GetFileName(query.Path);

        _validator.Validate(
            FileService.gp_file_new(out IntPtr file), 
            nameof(FileService.gp_file_new)
        );

        _validator.Validate(
            CameraService.gp_camera_file_get(
                _manager.CameraContext.Camera, 
                folder.ConvertToSByte(), 
                name.ConvertToSByte(), 
                1, 
                file, 
                _manager.CameraContext.Context
            ), 
            nameof(CameraService.gp_camera_file_get)
        );

        _validator.Validate(
            FileService.gp_file_get_data_and_size(file, out IntPtr data, out ulong size),
            nameof(FileService.gp_file_get_data_and_size)
        );
        
        model.Data = new byte[size];
        Marshal.Copy(data, model.Data, 0, model.Data.Length);

        _validator.Validate(
            FileService.gp_file_free(file), 
            nameof(FileService.gp_file_free)
        );

        return Task.FromResult((IModel)model);
    }

}