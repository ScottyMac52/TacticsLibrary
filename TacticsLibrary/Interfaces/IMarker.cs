using System;

namespace TacticsLibrary.Interfaces
{
    public interface IMarker
    {
        Action<IGraphics, IReferencePoint> PaintMethod { get; }
        void Draw(IGraphics g);
    }
}