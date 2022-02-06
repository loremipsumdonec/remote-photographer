using System.Runtime.InteropServices;
using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Reactive.Services;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Commands;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Gphoto2.Commands;

[Handle(typeof(ConnectCamera))]
public class ConnectCameraHandler
    : CommandHandler<ConnectCamera>
{
    private readonly ICameraContextManager _manager;
    private IMethodValidator _validator;
    private IEventDispatcher _dispatcher;

    public ConnectCameraHandler(
        ICameraContextManager manager,
        IMethodValidator validator, 
        IEventDispatcher dispatcher)
    {
        _manager = manager;
        _validator = validator;
        _dispatcher = dispatcher;
    }

    public override Task<bool> ExecuteAsync(ConnectCamera command)
    {
        lock(_manager.Door) 
        {
            if(_manager.CameraContext != null) 
            {
                var cameraContext = _manager.CameraContext;
                _manager.CameraContext = null;

                _validator.Validate(
                    CameraService.gp_camera_exit(cameraContext.Camera, cameraContext.Context),
                    nameof(CameraService.gp_camera_exit)
                );

                _validator.Validate(
                    CameraService.gp_camera_free(cameraContext.Camera),
                    nameof(CameraService.gp_camera_free)
                );

                ContextService.gp_context_unref(cameraContext.Context);
            }

            var context = ContextService.gp_context_new();

            _validator.Validate(
                PortInfoListService.gp_port_info_list_new(out IntPtr portInfoList), 
                nameof(PortInfoListService.gp_port_info_list_new)
            );

            _validator.Validate(
                PortInfoListService.gp_port_info_list_load(portInfoList), 
                nameof(PortInfoListService.gp_port_info_list_load)
            );

            var portIndex = _validator.Validate(
                PortInfoListService.gp_port_info_list_lookup_path(portInfoList, command.CameraId), 
                nameof(PortInfoListService.gp_port_info_list_lookup_path)
            );

            _validator.Validate(
                PortInfoListService.gp_port_info_list_get_info(portInfoList,portIndex, out IntPtr info), 
                nameof(PortInfoListService.gp_port_info_list_get_info)
            );

            _validator.Validate(
                CameraService.gp_camera_new(out IntPtr camera), 
                nameof(CameraService.gp_camera_new)
            );
           
            _validator.Validate(
                CameraService.gp_camera_set_port_info(camera, info), 
                nameof(CameraService.gp_camera_new)
            );

            
            _validator.Validate(
                AbilitiesListService.gp_abilities_list_new(out IntPtr cameraAbilitiesList), 
                nameof(AbilitiesListService.gp_abilities_list_new)
            );
            
            _validator.Validate(
                AbilitiesListService.gp_abilities_list_load(cameraAbilitiesList, context), 
                nameof(AbilitiesListService.gp_abilities_list_load)
            );

            _validator.Validate(
                ListService.gp_list_new(out IntPtr cameraList), 
                nameof(ListService.gp_list_new)
            );

            _validator.Validate(
                AbilitiesListService.gp_abilities_list_detect(cameraAbilitiesList, portInfoList, cameraList, context), 
                nameof(AbilitiesListService.gp_abilities_list_detect)
            );

            var count = _validator.Validate(
                AbilitiesListService.gp_abilities_list_count(cameraAbilitiesList), 
                nameof(AbilitiesListService.gp_abilities_list_count)
            );

            _validator.Validate(
                ListService.gp_list_get_name(cameraList, 0, out IntPtr namePointer), 
                nameof(ListService.gp_list_get_name)
            );

            string model = Marshal.PtrToStringAnsi(namePointer);

            var index = _validator.Validate(
                AbilitiesListService.gp_abilities_list_lookup_model(cameraAbilitiesList, model), 
                nameof(AbilitiesListService.gp_abilities_list_lookup_model)
            );

            _validator.Validate(
                AbilitiesListService.gp_abilities_list_get_abilities(cameraAbilitiesList, index, out CameraAbilities cameraAbilities), 
                nameof(AbilitiesListService.gp_abilities_list_get_abilities)
            );

            _validator.Validate(
                CameraService.gp_camera_set_abilities(camera, cameraAbilities), 
                nameof(CameraService.gp_camera_set_abilities)
            );
            
            _validator.Validate(
                CameraService.gp_camera_init(camera, context), 
                nameof(CameraService.gp_camera_init)
            );

            _manager.CameraContext = new CameraContext(
                command.CameraId, 
                context, 
                camera, 
                command.Tags
            );
        }

        _dispatcher.Dispatch(new CameraConnected());

        return Task.FromResult(true);
    }
}