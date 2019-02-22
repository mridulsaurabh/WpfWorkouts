using Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public abstract class ViewModelBase : NotificationObject
    {
        private readonly IWorkAsync _asyncService;
        private readonly IEventAggregator _eventAggregator;

        public ViewModelBase()
        {

        }

        public ViewModelBase(IWorkAsync asyncService, IEventAggregator eventAggregator)
        {
            this._asyncService = asyncService;
            this._eventAggregator = eventAggregator;
        }
    }
}
