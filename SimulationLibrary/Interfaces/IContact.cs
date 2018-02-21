using System;
using System.Threading;

namespace SimulationLibrary.Interfaces
{
    public interface IContact : IReferencePoint
    {
        void Start(Action processAction);
        void Stop();
        Thread ProcessThread { get; }
        bool Running { get;  }
        int? CustomUpdateDuration { get; set; }
        string ToString();
    }
}
