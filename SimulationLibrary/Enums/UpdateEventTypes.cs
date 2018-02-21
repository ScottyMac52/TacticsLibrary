using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLibrary
{
    public enum UpdateEventTypes
    {
        Unknown = -1,
        New,
        PositionChange,
        SpeedChange,
        AltitudeChange,
        HeadingChange,
        SelectedChange,
        ShowTextChange,
        Remove
    }
}
