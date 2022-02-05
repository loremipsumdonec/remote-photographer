using Boilerplate.Features.Core;

namespace RemotePhotographer.Features.Photographer.Models;

public class BatteryLevel
    : IModel
{
    public int Percent { get; set; }
}