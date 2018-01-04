using System.Drawing;
using TacticsLibrary.Adapters;

namespace TacticsLibrary.DrawObjects
{
    public interface IVisibleObjects
    {
        void Draw(IGraphics g);
        void Invalidate(Rectangle invalidRect);
    }
}
