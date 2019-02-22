using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Events
{
    public class ApplicationSettingsEventMessage : IMessage
    {
        public bool IsThemeChanged { get; set; }
        public bool IsSkinChanged { get; set; }
        public bool IsBackgroundChanged { get; set; }
    }
}
