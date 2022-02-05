using Boilerplate.Features.Core;
using Boilerplate.Features.Core.Queries;
using RemotePhotographer.Features.Gphoto2.Models;
using RemotePhotographer.Features.Gphoto2.Services;
using RemotePhotographer.Features.Gphoto2.Services.Interop;
using RemotePhotographer.Features.Photographer.Models;
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

                var config = new Config() 
                {
                    ConfigId = Marshal.PtrToStringAnsi(namePointer)
                };

                LoadConfig(config);

                model.Add(config);
            }

            _validator.Validate(ListService.gp_list_free(list), nameof(ListService.gp_list_free));
        }

        return Task.FromResult((IModel)model);
    }

    private void LoadConfig(Config config)
    {
        _validator.Validate(CameraService.gp_camera_get_single_config(
            _manager.CameraContext.Camera, config.ConfigId, out IntPtr widget, _manager.CameraContext.Context
        ), nameof(CameraService.gp_camera_get_single_config));

        _validator.Validate(
            WidgetService.gp_widget_get_type(widget, out CameraWidgetType type), 
            nameof(WidgetService.gp_widget_get_type)
        );

        config.Type = Enum.GetName<CameraWidgetType>(type);

        _validator.Validate(
            WidgetService.gp_widget_get_label(widget, out IntPtr labelPointer), 
            nameof(WidgetService.gp_widget_get_label)
        );

        config.Name = Marshal.PtrToStringAnsi(labelPointer);

        switch(type) 
        {
            case CameraWidgetType.GP_WIDGET_DATE:
                LoadDateValue(widget, config);
                break;
            case CameraWidgetType.GP_WIDGET_TEXT:
                LoadTextValue(widget, config);
                break;
            case CameraWidgetType.GP_WIDGET_TOGGLE:
                LoadToggleValue(widget, config);
                break;
            case CameraWidgetType.GP_WIDGET_RADIO:
                LoadRadioValue(widget, config);
                break;
        }

        _validator.Validate(
            WidgetService.gp_widget_free(widget), 
            nameof(WidgetService.gp_widget_free)
        );
    }

    private void LoadRadioValue(IntPtr widget, Config config)
    {
        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        config.Current = Marshal.PtrToStringAnsi(valuePointer);

        int total = WidgetService.gp_widget_count_choices(widget);

        for(int index = 0; index < total; index++) 
        {
            _validator.Validate(
                WidgetService.gp_widget_get_choice(widget, index, out IntPtr choice),
                nameof(WidgetService.gp_widget_get_choice)
            );
            
            config.Add(Marshal.PtrToStringAnsi(choice));
        }
    }

    private void LoadTextValue(IntPtr widget, Config config) 
    {
        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        config.Current = Marshal.PtrToStringAnsi(valuePointer);
    }

    private void LoadToggleValue(IntPtr widget, Config config) 
    {
        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        config.Current = (1 == (int)valuePointer).ToString().ToLower();
    }

    private void LoadDateValue(IntPtr widget, Config config) 
    {
        _validator.Validate(
            WidgetService.gp_widget_get_value(widget, out IntPtr valuePointer), 
            nameof(WidgetService.gp_widget_get_value)
        );

        var datetime = DateTime.UnixEpoch.AddSeconds(((int)valuePointer));
        config.Current = datetime.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}