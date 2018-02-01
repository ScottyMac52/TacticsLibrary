using System.Drawing;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary
{
    public class ObjectFactory
    {
        public static IReferencePoint Create(PointF position, SizeF size)
        {
            return new ReferencePoint(position, size);
        }

    }
}
