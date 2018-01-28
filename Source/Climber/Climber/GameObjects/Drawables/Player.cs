using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Climber
{
    public class Player : DrawableEntity, IPlayer
    {
        bool canAnimate = true;

        public override string Char => "🐵";

        public Player(int row) : base(row)
        {
            Row = row;
            Canvas.SetZIndex(UIElement, 1000);
        }

        public void Climb(ClimbingDirection direction)
        {
            if(canAnimate && CanClimb(direction))
            {
                canAnimate = false;
                ClimbWithAnimation(direction);
            }
        }

        private bool CanClimb(ClimbingDirection direction)
        {
            var newRow = CaluculateNewRow(direction);
            return newRow > -1 && newRow < GameConstants.NUMBEROFROWS-1;
        }

        private int CaluculateNewRow(ClimbingDirection direction) 
            => Row + (direction == ClimbingDirection.Up ? -1 : 1);

       
        private void ClimbWithAnimation(ClimbingDirection direction)
        {
            var newRow = CaluculateNewRow(direction);

            var verticalAnimation = GetVerticalAnimation(newRow);
            var scaleAnimations = GetScaleAnimations();

            Storyboard sb = new Storyboard();
            sb.Children.Add(verticalAnimation);
            sb.Children.Add(scaleAnimations.xAnimation);
            sb.Children.Add(scaleAnimations.yAnimation);

            sb.Begin();
            sb.Completed += (_, __) =>
            {
                Row = newRow;
                canAnimate = true;
            };
        }

        protected override void StartInternal()
        {
            TranslateTransform.Y = GetYPosition(Row);
            TranslateTransform.X = GetCenterXPosition();
        }

        private DoubleAnimation GetVerticalAnimation(int toRow)
        {
            DoubleAnimation yPosition = new DoubleAnimation();
            yPosition.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            yPosition.To = toRow * GameConstants.ROWHEIGHT;
            yPosition.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            yPosition.SetValue(Storyboard.TargetPropertyProperty, TranslateTransformY);
            Storyboard.SetTarget(yPosition, UIElement);
            return yPosition;
        }

        private (DoubleAnimation xAnimation, DoubleAnimation yAnimation) GetScaleAnimations()
        {
            var xScale = GetBaseScaleAnimation();
            xScale.SetValue(Storyboard.TargetPropertyProperty, ScaleTransformX);
            Storyboard.SetTarget(xScale, UIElement);

            var yScale = GetBaseScaleAnimation();
            yScale.SetValue(Storyboard.TargetPropertyProperty, ScaleTransformY);
            Storyboard.SetTarget(yScale, UIElement);

            return (xScale, yScale);
        }

        private DoubleAnimation GetBaseScaleAnimation()
        {
            DoubleAnimation scale = new DoubleAnimation();
            scale.EasingFunction = new ExponentialEase()
            {
                EasingMode = EasingMode.EaseInOut
            };
            scale.To = 1.2;
            scale.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            scale.AutoReverse = true;
            return scale;
        }
    }
}
