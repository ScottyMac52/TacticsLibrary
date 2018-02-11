using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Enums;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary.EventHandlers
{
    public class UpdateEventArgs
    {
        public Region UpdateRegion { get; set; }

        public UpdateEventTypes EventType { get; set; }
    }
}
