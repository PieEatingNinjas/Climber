using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Climber
{
    public class Player
    {
        public TextBlock UIElement { get; private set; }

        public Rect GetRect(Canvas canvas)
        {
            return UIElement.TransformToVisual(canvas).TransformBounds(new Rect(new Point(0, 0), UIElement.RenderSize));
        }

        public int Row { get; private set; }
        public string Char { get; set; } = "🐵";

        ScaleTransform scaleTransform;
        TranslateTransform translateTransform;
        public Player(int rowStart)
        {
            Row = rowStart;
            UIElement = new TextBlock();
            UIElement.Text = Char;
            UIElement.RenderTransformOrigin = new Point(.5, .5);
            UIElement.FontSize = 30;
            UIElement.Height = 45;
            Canvas.SetZIndex(UIElement, 1000);

            var group = new TransformGroup();
            scaleTransform = new ScaleTransform();
            translateTransform = new TranslateTransform();
            translateTransform.X = 250 - 23;
            translateTransform.Y = Row * 46;
            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);
            UIElement.RenderTransform = group;
        }

        public void Up()
        {
            if (canAnimate)
            {
                canAnimate = false;
                Animate(true);
            }
        }

        public void Down()
        {
            if (canAnimate)
            {
                canAnimate = false;
                Animate(false);
            }
        }

        bool canAnimate = true;
        private void Animate(bool isUp)
        {
            var newRow = Row + (isUp ? -1 : 1);
            var y = newRow * 46;

            DoubleAnimation position = new DoubleAnimation();
            position.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            position.To = y;
            position.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            position.SetValue(Storyboard.TargetPropertyProperty, "(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)");
            Storyboard.SetTarget(position, UIElement);


            DoubleAnimation scaleX = new DoubleAnimation();
            scaleX.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            scaleX.To = 1.2;
            scaleX.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            scaleX.AutoReverse = true;
            scaleX.SetValue(Storyboard.TargetPropertyProperty, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)");
            Storyboard.SetTarget(scaleX, UIElement);

            DoubleAnimation scaleY = new DoubleAnimation();
            scaleY.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            scaleY.To = 1.2;
            scaleY.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            scaleY.AutoReverse = true;
            scaleY.SetValue(Storyboard.TargetPropertyProperty, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)");
            Storyboard.SetTarget(scaleY, UIElement);



            Storyboard sb = new Storyboard();
            sb.Children.Add(position);
            sb.Children.Add(scaleX);
            sb.Children.Add(scaleY);

            sb.Begin();
            sb.Completed += (_, __) =>
            {
                Row = newRow;
                canAnimate = true;
            };
        }
    }
}
