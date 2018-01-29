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
            if (canAnimate && CanClimb(direction))
            {
                canAnimate = false;
                ClimbWithAnimation(direction);
            }
        }

        private bool CanClimb(ClimbingDirection direction)
        {
            var newRow = CaluculateNewRow(direction);
            return newRow > -1 && newRow < GameConstants.NUMBEROFROWS;
        }

        private int CaluculateNewRow(ClimbingDirection direction)
            => Row + (direction == ClimbingDirection.Up ? -1 : 1);

        private void ClimbWithAnimation(ClimbingDirection direction)
        {
            var newRow = CaluculateNewRow(direction);

            Storyboard sb = PlayerAnimationHelper
                .GetClimbingAnimation(UIElement, newRow, new Duration(TimeSpan.FromMilliseconds(100)));

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
    }
}
