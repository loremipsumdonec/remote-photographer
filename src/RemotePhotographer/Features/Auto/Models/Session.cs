using Boilerplate.Features.Core;
using RemotePhotographer.Features.Auto.Actions;

namespace RemotePhotographer.Features.Auto.Models;

public class Session
    : IModel
{
    public string SessionId { get; set; }

    public List<IAction> Actions { get; set; } = new();
}