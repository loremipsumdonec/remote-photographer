using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetCameras))]
public class GetCamerasHandler
    : QueryHandler<GetCameras>
{
    private readonly IMethodResutlValidator _validator;

    public override Task<IModel> ExecuteAsync(GetCameras query)
    {
        var model = new GetCamerasModel();

        var context = ContextService.gp_context_new();

        _validator.Validate(
            PortInfoListService.gp_port_info_list_new(out IntPtr portInfoList), 
            nameof(PortInfoListService.gp_port_info_list_new)
        );

        _validator.Validate(
            AbilitiesListService.gp_abilities_list_new(out IntPtr cameraAbilitiesList), 
            nameof(AbilitiesListService.gp_abilities_list_new)
        );

        _validator.Validate(
            PortInfoListService.gp_port_info_list_load(portInfoList), 
            nameof(PortInfoListService.gp_port_info_list_load)
        );

        _validator.Validate(
            AbilitiesListService.gp_abilities_list_load(cameraAbilitiesList, context), 
            nameof(AbilitiesListService.gp_abilities_list_load)
        );

        _validator.Validate(
            ListService.gp_list_new(out IntPtr list), 
            nameof(ListService.gp_list_new)
        );

        _validator.Validate(
            AbilitiesListService.gp_abilities_list_detect(cameraAbilitiesList, portInfoList, list, context),
            nameof(AbilitiesListService.gp_abilities_list_detect)
        );

        var count = ListService.gp_list_count(list);

        for(int index = 0; index < count; index++) 
        {
            _validator.Validate(
                ListService.gp_list_get_name(list, index, out IntPtr namePointer), 
                nameof(ListService.gp_list_get_name)
            );

            _validator.Validate(
                ListService.gp_list_get_value(list, index, out IntPtr valuePointer), 
                nameof(ListService.gp_list_get_value)
            );

            var camera = new Camera(
                Marshal.PtrToStringAnsi(namePointer), 
                Marshal.PtrToStringAnsi(valuePointer)
            );

            model.Add(camera);
        }

        _validator.Validate(
            ListService.gp_list_free(list), nameof(ListService.gp_list_free)
        );

        _validator.Validate(
            AbilitiesListService.gp_abilities_list_free(cameraAbilitiesList), 
            nameof(AbilitiesListService.gp_abilities_list_free)
        );
        
        _validator.Validate(
            PortInfoListService.gp_port_info_list_free(portInfoList), 
            nameof(PortInfoListService.gp_port_info_list_free)
        );

        ContextService.gp_context_unref(context);

        return Task.FromResult((IModel)model);
    }
}