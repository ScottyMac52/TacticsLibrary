using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary.EventHandlers
{
    public class ReferencePointEventArgs
    {
        public IReferencePoint Sender { get; set; }
    }
}
