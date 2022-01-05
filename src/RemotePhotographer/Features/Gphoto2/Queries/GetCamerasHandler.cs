using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.InteropServices;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetCameras))]
public class GetCamerasHandler
    : QueryHandler<GetCameras>
{
    public override Task<IModel> ExecuteAsync(GetCameras query)
    {
        var model = new GetCamerasModel();

        var context = ContextService.gp_context_new();
        PortInfoListService.gp_port_info_list_new(out IntPtr portInfoList);
        AbilitiesListService.gp_abilities_list_new(out IntPtr cameraAbilitiesList);
        PortInfoListService.gp_port_info_list_load(portInfoList);
        AbilitiesListService.gp_abilities_list_load(cameraAbilitiesList, context);
        ListService.gp_list_new(out IntPtr list);

        AbilitiesListService.gp_abilities_list_detect(cameraAbilitiesList, portInfoList, list, context);

        var count = ListService.gp_list_count(list);

        for(int index = 0; index < count; index++) 
        {
            ListService.gp_list_get_name(list, index, out IntPtr namePointer);
            ListService.gp_list_get_value (list, index, out IntPtr valuePointer);

            var camera = new Camera(
                Marshal.PtrToStringAnsi(namePointer), 
                Marshal.PtrToStringAnsi(valuePointer)
            );

            model.Add(camera);
        }

        ListService.gp_list_free(list);
        AbilitiesListService.gp_abilities_list_free(cameraAbilitiesList);
        PortInfoListService.gp_port_info_list_free(portInfoList);
        ContextService.gp_context_unref(context);

        return Task.FromResult((IModel)model);
    }
}