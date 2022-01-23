using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using Boilerplate.Features.Core.Services;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
using RemotePhotographer.Features.Photographer.Queries;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Queries;

[Handle(typeof(GetPorts))]
public class GetPortsHandler
    : QueryHandler<GetPorts>
{
    private readonly IModelService _service;
    private readonly ICameraContextManager _manager;
    private readonly IMethodValidator _validator;

    public GetPortsHandler(
        IModelService service, 
        ICameraContextManager manager, 
        IMethodValidator validator)
    {
        _service = service;
        _manager = manager;
        _validator = validator;
    }

    public override async Task<IModel> ExecuteAsync(GetPorts query)
    {
        var model = new GetPortsModel();

        _validator.Validate(
            PortInfoListService.gp_port_info_list_new(out IntPtr portInfoList), 
            nameof(PortInfoListService.gp_port_info_list_new)
        );

        _validator.Validate(
            PortInfoListService.gp_port_info_list_load(portInfoList), 
            nameof(PortInfoListService.gp_port_info_list_load)
        );

        var count = PortInfoListService.gp_port_info_list_count(portInfoList);

        for(int index = 0; index < count; index++) 
        {
            _validator.Validate(
                PortInfoListService.gp_port_info_list_get_info(portInfoList, index, out IntPtr info), 
                nameof(PortInfoListService.gp_port_info_list_get_info)
            );

            var portInfo = await _service.CreateModelAsync<PortInfo>(info);
            model.Add(portInfo);
        }

        _validator.Validate(
            PortInfoListService.gp_port_info_list_free(portInfoList), 
            nameof(PortInfoListService.gp_port_info_list_free)
        );

        return model;
    }
}