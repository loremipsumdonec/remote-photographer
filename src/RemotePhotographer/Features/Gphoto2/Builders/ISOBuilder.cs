using RemotePhotographer.Features.Gphoto2.Services.Interop;
using Boilerplate.Features.Mapper.Services;
using Boilerplate.Features.Mapper.Attributes;
using RemotePhotographer.Features.Photographer.Models;
using System.Runtime.InteropServices;

namespace RemotePhotographer.Features.Gphoto2.Builders;

[BuilderFor(typeof(ISO), typeof(IntPtr))]
public class ISOBuilder
    : ModelBuilder<IntPtr, ISO>
{
    public override Task BuildAsync(IntPtr widget, ISO model)
    {
        WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer);
        model.Current = Marshal.PtrToStringAnsi(valuePointer);

        int total = WidgetService.gp_widget_count_choices(widget);

        for(int index = 0; index < total; index++) 
        {
            WidgetService.gp_widget_get_choice(widget, index, out IntPtr choice);
            model.Add(Marshal.PtrToStringAnsi(choice));
        }

        return Task.CompletedTask;
    }
}