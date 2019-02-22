using Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// It's simple message based event aggregator provides publisher subscription model 
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly IDictionary<Type, IList> _subscriptions = new Dictionary<Type, IList>();

        public void Publish<TMessage>(TMessage message) 
            where TMessage : IMessage
        {
            if (message == null)
                throw new ArgumentNullException("message");

            Type messageType = typeof(TMessage);
            if (this._subscriptions.ContainsKey(messageType))
            {
                var subscriptionList =
                    new List<ISubscription<TMessage>>(this._subscriptions[messageType].Cast<ISubscription<TMessage>>());
                foreach (var subscription in subscriptionList)
                {
                    subscription.Action(message);

                    if(subscription.IsUnsubscriptionEnabled)
                    {
                        subscription.UnSubscribe();
                    }
                }
            }
        }

        public ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> action, bool isSubscribeToExecuteOnce = false) 
            where TMessage : IMessage
        {
            Type messageType = typeof(TMessage);
            var subscription = new Subscription<TMessage>(this, action);
            subscription.IsUnsubscriptionEnabled = isSubscribeToExecuteOnce;
            if (this._subscriptions.ContainsKey(messageType))
            {
                this._subscriptions[messageType].Add(subscription);
            }
            else
            {
                this._subscriptions.Add(messageType, new List<ISubscription<TMessage>>() { subscription });
            }
            return subscription;
        }

        public void UnSubscribe<TMessage>(ISubscription<TMessage> subscription) 
            where TMessage : IMessage
        {
            Type messageType = typeof(TMessage);
            if(this._subscriptions.ContainsKey(messageType))
            {
                this._subscriptions[messageType].Remove(subscription);
            }
        }

        public void ClearAllSubscriptions()
        {
            this.ClearAllSubscriptions(null);
        }

        public void ClearAllSubscriptions(Type[] expectMessages)
        {
            foreach (var messageSubscription in new Dictionary<Type, IList>(this._subscriptions))
            {
                bool canDelete = true;
                if(expectMessages != null)
                {
                    canDelete = !expectMessages.Contains(messageSubscription.Key);
                    if(canDelete)
                    {
                        this._subscriptions.Remove(messageSubscription);
                    }
                }
            }
        }
    }
}
