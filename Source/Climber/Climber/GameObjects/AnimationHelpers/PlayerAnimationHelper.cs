using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Climber
{
    public static class PlayerAnimationHelper
    {
        public static Storyboard GetClimbingAnimation(UIElement uiElement, int newRow, Duration duration)
        {
            var verticalAnimation = GetVerticalAnimation(uiElement, newRow, duration);
            var scaleAnimations = GetScaleAnimations(uiElement, duration);

            Storyboard sb = new Storyboard();
            sb.Children.Add(verticalAnimation);
            sb.Children.Add(scaleAnimations.xAnimation);
            sb.Children.Add(scaleAnimations.yAnimation);

            return sb;
        }

        public static DoubleAnimation GetVerticalAnimation(UIElement uiElement, int toRow, Duration duration)
        {
            DoubleAnimation verticalPosition = new DoubleAnimation();
            verticalPosition.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            verticalPosition.To = toRow * GameConstants.ROWHEIGHT;
            verticalPosition.Duration = duration;
            verticalPosition.SetValue(Storyboard.TargetPropertyProperty, DrawableEntity.TranslateTransformY);
            Storyboard.SetTarget(verticalPosition, uiElement);
            return verticalPosition;
        }

        public static (DoubleAnimation xAnimation, DoubleAnimation yAnimation) GetScaleAnimations(UIElement uiElement, Duration duration)
        {
            var xScale = GetBaseScaleAnimation(duration);
            xScale.SetValue(Storyboard.TargetPropertyProperty, DrawableEntity.ScaleTransformX);
            Storyboard.SetTarget(xScale, uiElement);

            var yScale = GetBaseScaleAnimation(duration);
            yScale.SetValue(Storyboard.TargetPropertyProperty, DrawableEntity.ScaleTransformY);
            Storyboard.SetTarget(yScale, uiElement);

            return (xScale, yScale);
        }

        private static DoubleAnimation GetBaseScaleAnimation(Duration duration)
        {
            DoubleAnimation scale = new DoubleAnimation();
            scale.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            scale.To = 1.2;
            scale.Duration = duration;
            scale.AutoReverse = true;
            return scale;
        }
    }
}
