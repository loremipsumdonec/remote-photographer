using RemotePhotographer.Features.Auto.Events;
using RemotePhotographer.Features.Photographer.Events;

namespace RemotePhotographer.Features.Photographer.Schema;

public class PhotographerSubscription
{
    [Subscribe]
    public CameraConnected OnCameraConnected([EventMessage] CameraConnected @event) => @event;

    [Subscribe]
    public CameraDisconnected OnCameraDisconnected([EventMessage] CameraDisconnected @event) => @event;

    [Subscribe]
    public ApertureChanged OnApertureChanged([EventMessage] ApertureChanged @event) => @event;

    [Subscribe]
    public ShutterSpeedChanged OnShutterSpeedChanged([EventMessage] ShutterSpeedChanged @event) => @event;

    [Subscribe]
    public CaptureTargetChanged OnCaptureTargetChanged([EventMessage] CaptureTargetChanged @event) => @event;

    [Subscribe]
    public ImageFormatChanged OnImageFormatChanged([EventMessage] ImageFormatChanged @event) => @event;

    [Subscribe]
    public ISOChanged OnISOChanged([EventMessage] ISOChanged @event) => @event;

    [Subscribe]
    public SessionStarted OnSessionStarted([EventMessage] SessionStarted @event) => @event;

    [Subscribe]
    public SessionStopped OnSessionStopped([EventMessage] SessionStopped @event) => @event;

    [Subscribe]
    public SessionFinished OnSessionFinished([EventMessage] SessionFinished @event) => @event;

    [Subscribe]
    public SessionFailed OnSessionFailed([EventMessage] SessionFailed @event) => @event;

    [Subscribe]
    public ActionStarted OnActionStarted([EventMessage] ActionStarted @event) => @event;

    [Subscribe]
    public ActionFinished OnActionFinsihed([EventMessage] ActionFinished @event) => @event;
}