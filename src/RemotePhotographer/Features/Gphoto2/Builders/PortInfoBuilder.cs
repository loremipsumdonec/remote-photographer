using RemotePhotographer.Features.Gphoto2.Services.Interop;
using Boilerplate.Features.Mapper.Services;
using Boilerplate.Features.Mapper.Attributes;
using RemotePhotographer.Features.Photographer.Models;
using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Models;

namespace RemotePhotographer.Features.Gphoto2.Builders;

[BuilderFor(typeof(PortInfo), typeof(IntPtr))]
public class PortInfoBuilder
    : ModelBuilder<IntPtr, PortInfo>
{
    private IMethodValidator _validator;

    public PortInfoBuilder(IMethodValidator validator)
    {
        _validator = validator;
    }

    public override Task BuildAsync(IntPtr info, PortInfo model)
    {

        _validator.Validate(
            PortInfoListService.gp_port_info_get_name(info, out IntPtr namePointer), 
            nameof(PortInfoListService.gp_port_info_get_name)
        );

        _validator.Validate(
            PortInfoListService.gp_port_info_get_path(info, out IntPtr pathPointer), 
            nameof(PortInfoListService.gp_port_info_get_path)
        );

        _validator.Validate(
            PortInfoListService.gp_port_info_get_type(info, out GPPortType type), 
            nameof(PortInfoListService.gp_port_info_get_type)
        );

        model.Name = Marshal.PtrToStringAnsi(namePointer);
        model.Path = Marshal.PtrToStringAnsi(pathPointer);
        model.Type = type.ToString();

        return Task.CompletedTask;
    }

    private string GetTypeName(GPPortType type) => type switch
    {
        GPPortType.GP_PORT_NONE => "No specific type associated",
        GPPortType.GP_PORT_SERIAL => "Serial port",
        GPPortType.GP_PORT_USB => "USB port",
        GPPortType.GP_PORT_DISK => "Disk / local mountpoint port",
        GPPortType.GP_PORT_PTPIP => "PTP/IP port",
        GPPortType.GP_PORT_USB_DISK_DIRECT => "Direct IO to an usb mass storage device",
        GPPortType.GP_PORT_USB_SCSI => "USB Mass Storage raw SCSI port",
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not a valid type value {type}"),
    };

}