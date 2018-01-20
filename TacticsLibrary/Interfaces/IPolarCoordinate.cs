using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Interfaces
{
    public interface IPolarCoordinate
    {
        double Degrees { get; set; }
        double Radius { get; set; }
        bool Equals(PolarCoordinate other);
        string ToString();
    }
}