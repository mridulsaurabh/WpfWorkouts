using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cerberus
{
    public class SerialDataEventArgs : EventArgs
    {
        public SerialDataEventArgs(byte[] dataInByteArray)
        {
            Data = dataInByteArray;
        }

        public byte[] Data
        {
            get;
            private set;
        }
    }
}
