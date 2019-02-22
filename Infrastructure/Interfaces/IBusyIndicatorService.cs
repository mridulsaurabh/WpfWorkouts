using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cerberus
{
    public interface IBusyIndicatorService
    {
        void Start(string message);

        void Stop();
    }
}
