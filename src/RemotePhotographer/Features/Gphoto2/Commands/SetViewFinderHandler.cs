using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Photographer.Commands;
using Boilerplate.Features.Reactive.Services;
using Boilerplate.Features.Core.Queries;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(SetViewFinder))]
public class SetViewFinderHandler
    : CommandHandler<SetViewFinder>
{
    private readonly ICameraContextManager _manager;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMethodValidator _validator;
    private readonly IEventDispatcher _dispatcher;

    public SetViewFinderHandler(
        ICameraContextManager manager,
        IQueryDispatcher queryDispatcher,
        IMethodValidator validator, 
        IEventDispatcher dispatcher)
    {
        _manager = manager;
        _queryDispatcher = queryDispatcher;
        _validator = validator;
        _dispatcher = dispatcher;
    }

    public override async Task<bool> ExecuteAsync(SetViewFinder command)
    {
        await Task.Run(() => {

            lock(_manager.Door) 
            {
                _manager.EnsureCameraContext();

                _validator.Validate(
                    CameraService.gp_camera_get_single_config(
                        _manager.CameraContext.Camera, "viewfinder", out IntPtr widget, _manager.CameraContext.Context
                    ), 
                    nameof(CameraService.gp_camera_get_single_config)
                );

                IntPtr ptr = Marshal.AllocHGlobal(sizeof(int));
                byte[] valueAsByte = BitConverter.GetBytes(command.Open ? 1 : 0);           
                Marshal.Copy(valueAsByte, 0, ptr, valueAsByte.Length);

                _validator.Validate(
                    WidgetService.gp_widget_set_value(widget, ptr), 
                    nameof(WidgetService.gp_widget_set_value)
                );

                _validator.Validate(
                    CameraService.gp_camera_set_single_config(
                        _manager.CameraContext.Camera, "viewfinder", widget, _manager.CameraContext.Context
                    ), 
                    nameof(CameraService.gp_camera_set_single_config)
                );

                Marshal.FreeHGlobal(ptr);               
            }

        });

        return true;
    }
}