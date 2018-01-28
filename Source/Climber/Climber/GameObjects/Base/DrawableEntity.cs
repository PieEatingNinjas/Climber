using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Climber
{
    public abstract class DrawableEntity : IDisposable, IDrawableEntity
    {
        public TextBlock UIElement { get; private set; }
        public int Row { get; protected set; }
        public abstract string Char { get; }

        public bool IsActive { get; protected set; }

        public TranslateTransform TranslateTransform { get; }

        public event EventHandler Destroyed;

        public DrawableEntity(int row)
        {
            Row = row;

            UIElement = new TextBlock();  
            UIElement.RenderTransformOrigin = new Point(.5, .5);
            UIElement.FontSize = 30;
            UIElement.Height = GameConstants.ROWHEIGHT - 2;

            var transformGroup = new TransformGroup();
            TranslateTransform = new TranslateTransform();
            transformGroup.Children.Add(TranslateTransform);

            UIElement.RenderTransform = transformGroup;
        }

        public Rect GetRect(Canvas canvas) 
            => UIElement.TransformToVisual(canvas).TransformBounds(new Rect(new Point(0, 0), UIElement.RenderSize));

        public void Start()
        {
            UIElement.Text = Char;
            IsActive = true;
            StartInternal();
        }

        protected abstract void StartInternal();

        public void Destroy()
        {
            IsActive = false;
            DestroyInternal();
            Destroyed?.Invoke(this, null);
        }

        protected virtual void DestroyInternal() { }

        public void Dispose() => UIElement = null;
    }
}
