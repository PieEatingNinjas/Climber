using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Climber
{
    public abstract class DrawableEntity : IDisposable, IDrawableEntity
    {
        protected const string TranslateTransformX = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)";
        protected const string TranslateTransformY = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)";
        protected const string ScaleTransformX = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)";
        protected const string ScaleTransformY = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)";

        public TextBlock UIElement { get; private set; }
        public int Row { get; protected set; }
        public abstract string Char { get; }

        public bool IsActive { get; protected set; }

        public TranslateTransform TranslateTransform { get; }

        public event EventHandler Destroyed;

        public TransformGroup TransformGroup { get; }

        public ScaleTransform ScaleTransform { get; }

        public DrawableEntity(int row)
        {
            Row = row;

            UIElement = new TextBlock();  
            UIElement.RenderTransformOrigin = new Point(.5, .5);
            UIElement.FontSize = 30;
            UIElement.Height = GameConstants.ROWHEIGHT - 2;

            TransformGroup = new TransformGroup();
            TranslateTransform = new TranslateTransform();
            ScaleTransform = new ScaleTransform();
            TransformGroup.Children.Add(ScaleTransform);
            TransformGroup.Children.Add(TranslateTransform);
            UIElement.RenderTransform = TransformGroup;
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

        public double GetYPosition(int row) => row * GameConstants.ROWHEIGHT;

        public double GetCenterXPosition() => GameConstants.CANVASWIDTH / 2 - GameConstants.DRAWABLEWIDTH / 2;
    }
}
