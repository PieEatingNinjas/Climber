using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Climber
{
    public static class SnakeAnimationHelper
    {
        public static Storyboard GetHorizontalAnimation(UIElement uiElement, Duration duration)
        {
            DoubleAnimation horizontalAnimation = new DoubleAnimation
            {
                From = 0,
                To = GameConstants.CANVASWIDTH,
                Duration = duration
            };
            horizontalAnimation.SetValue(Storyboard.TargetPropertyProperty, DrawableEntity.TranslateTransformX);
            Storyboard.SetTarget(horizontalAnimation, uiElement);

            var sb = new Storyboard();
            sb.Children.Add(horizontalAnimation);
            return sb;
        }
    }
}
