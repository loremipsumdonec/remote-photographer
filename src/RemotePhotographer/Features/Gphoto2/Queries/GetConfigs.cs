using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Queries;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetConfigs))]
public class GetConfigsHandler
    : QueryHandler<GetConfigs>
{
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetConfigsHandler(
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _manager = manager;
        _validator = validator;
    }

    public override Task<IModel> ExecuteAsync(GetConfigs query)
    {
        var model = new GetConfigsModel();

        lock(_manager.Door) 
        {
            _validator.Validate(
                ListService.gp_list_new(out IntPtr list), 
                nameof(ListService.gp_list_new)
            );

            _validator.Validate(
                CameraService.gp_camera_list_config(
                    _manager.CameraContext.Camera,
                    list, 
                    _manager.CameraContext.Context
                ), 
                nameof(CameraService.gp_camera_list_config)
            );

            var count = ListService.gp_list_count(list);

            for(int index = 0; index < count; index++) 
            {
                _validator.Validate(
                    ListService.gp_list_get_name(list, index, out IntPtr namePointer),
                    nameof(ListService.gp_list_get_name)
                );

                string name = Marshal.PtrToStringAnsi(namePointer);
                model.Add(name);
            }

            _validator.Validate(ListService.gp_list_free(list), nameof(ListService.gp_list_free));
        }

        return Task.FromResult((IModel)model);
    }
}