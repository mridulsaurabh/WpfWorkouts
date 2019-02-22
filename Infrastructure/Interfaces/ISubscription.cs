using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ISubscription<TMessage> : IDisposable
        where TMessage : IMessage
    {
        Action<TMessage> Action { get; }
        bool IsUnsubscriptionEnabled { get; set; }
        IEventAggregator EventAggregator { get; }
        void UnSubscribe();
    }
}
