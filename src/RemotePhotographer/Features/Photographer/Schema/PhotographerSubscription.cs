using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Photographer.Schema;

public class PhotographerSubscription
{
    [Subscribe]
    public CameraConnected OnCameraConnected([EventMessage] CameraConnected @event) => @event;

    [Subscribe]
    public CameraDisconnected OnCameraDisconnected([EventMessage] CameraDisconnected @event) => @event;

    [Subscribe]
    public ImageCaptured OnImageCaptured([EventMessage] ImageCaptured @event) => @event;

    [Subscribe]
    public ApertureChanged OnApertureChanged([EventMessage] ApertureChanged @event) => @event;

    [Subscribe]
    public ShutterSpeedChanged OnShutterSpeedChanged([EventMessage] ShutterSpeedChanged @event) => @event;

    [Subscribe]
    public ISOChanged OnISOChanged([EventMessage] ISOChanged @event) => @event;
}