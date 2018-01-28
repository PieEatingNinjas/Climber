using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Climber
{
    public interface IDrawableEntity : IDisposable
    {
        TextBlock UIElement { get; }
        int Row { get; }
        bool IsActive { get; }
        Rect GetRect(Canvas canvas);
        void Start();
        void Destroy();
        event EventHandler Destroyed;
    }
}
