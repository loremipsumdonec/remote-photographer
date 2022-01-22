using Boilerplate.Features.Reactive.Events;
using Boilerplate.Features.Reactive.Services;
using HotChocolate.Subscriptions;
using System.Reflection;

namespace Boilerplate.Features.GraphQL.Services
{
    public class SubscriptionEventHub
        : IEventHub
    {
        private readonly IEventHub _decorated;
        private readonly ITopicEventSender _sender;

        public SubscriptionEventHub(
            IEventHub decorated,
            ITopicEventSender sender
        )
        {
            _decorated = decorated;
            _sender = sender;
        }

        public bool IsOpen => _decorated.IsOpen;

        public void Close()
        {
            _decorated.Close();
        }

        public void Connect(Action<IObservable<IEvent>> connect)
        {
            _decorated.Connect(connect);
        }

        public void Connect(Func<IObservable<IEvent>, IDisposable> connect)
        {
            _decorated.Connect(connect);
        }

        public void Dispatch(IEvent @event)
        {
            Lorem(@event);
            _decorated.Dispatch(@event);
        }

        public void Open()
        {
            _decorated.Open();
        }

        private void Lorem(IEvent @event) 
        {
            string name = $"On" + @event.GetType().Name;

            MethodInfo method = _sender.GetType().GetMethod(nameof(ITopicEventSender.SendAsync));
            var task = (ValueTask)method.MakeGenericMethod(typeof(string), @event.GetType())
                .Invoke(_sender, new object[] { name, @event, default(CancellationToken) });

            task.GetAwaiter().GetResult();
        }
    }
}
