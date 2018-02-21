using GraphicsLibrary;
using System;

namespace SimulationLibrary.Interfaces

{
    public interface IMarker
    {
        Action<IGraphics, IReferencePoint> PaintMethod { get; }
        void Draw(IGraphics g);
    }
}