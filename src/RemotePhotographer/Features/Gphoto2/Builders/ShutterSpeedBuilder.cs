using RemotePhotographer.Features.Gphoto2.Services.Interop;
using Boilerplate.Features.Mapper.Services;
using Boilerplate.Features.Mapper.Attributes;
using RemotePhotographer.Features.Photographer.Models;
using System.Runtime.InteropServices;
using RemotePhotographer.Features.Gphoto2.Services;

namespace RemotePhotographer.Features.Gphoto2.Builders;

[BuilderFor(typeof(ShutterSpeed), typeof(IntPtr))]
public class ShutterSpeedBuilder
    : ModelBuilder<IntPtr, ShutterSpeed>
{
    private IMethodValidator _validator;

    public ShutterSpeedBuilder(IMethodValidator validator)
    {
        _validator = validator;
    }


    public override Task BuildAsync(IntPtr widget, ShutterSpeed model)
    {
        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer),
            nameof(WidgetService.gp_widget_get_value)
        );

        model.Current = Marshal.PtrToStringAnsi(valuePointer);

        int total = WidgetService.gp_widget_count_choices(widget);

        for (int index = 0; index < total; index++)
        {
            _validator.Validate(
                WidgetService.gp_widget_get_choice(widget, index, out IntPtr choice),
                nameof(WidgetService.gp_widget_get_choice)
            );
            model.Add(Marshal.PtrToStringAnsi(choice));
        }

        return Task.CompletedTask;
    }
}