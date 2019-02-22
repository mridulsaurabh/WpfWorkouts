using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class Subscription<TMessage> : ISubscription<TMessage>
        where TMessage : IMessage
    {
        public Subscription(IEventAggregator eventAggregator, Action<TMessage> action)
        {
            this.Action = action;
            this.EventAggregator = eventAggregator;
        }

        public Action<TMessage> Action { get; private set; }
        public bool IsUnsubscriptionEnabled { get; set; }
        public IEventAggregator EventAggregator { get; private set; }
        public void UnSubscribe()
        {
            if(this != null)
            {
                this.EventAggregator.UnSubscribe(this);
            }
        }

        #region IDisposable members
        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
               this.EventAggregator.UnSubscribe(this);
            }
        }
        #endregion
    }
}
