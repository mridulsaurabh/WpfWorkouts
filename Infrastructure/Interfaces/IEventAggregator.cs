using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IEventAggregator
    {
        void Publish<TMessage>(TMessage message) where TMessage : IMessage;

        ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> action, bool isSubscribeToExecuteOnce = false) where TMessage : IMessage;

        void UnSubscribe<TMessage>(ISubscription<TMessage> subscription) where TMessage : IMessage;

        void ClearAllSubscriptions();

        void ClearAllSubscriptions(Type[] expectMessages);
    }
}
