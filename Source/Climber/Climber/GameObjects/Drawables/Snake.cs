using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Climber
{
    public class Snake : DrawableEntity, IEnemy
    {
        public override string Char => "🐍";

        public int TTL { get; }

        public Snake(int row, int ttl) : base(row) => TTL = ttl;

        protected override void StartInternal()
        {
            TranslateTransform.Y = GetYPosition(Row);

            var sb = new Storyboard();
            sb.Children.Add(GetHorizontalAnimation());
            sb.Begin();
            sb.Completed += Sb_Completed;
        }

        private DoubleAnimation GetHorizontalAnimation()
        {
            DoubleAnimation yPosition = new DoubleAnimation();
            yPosition.From = 0;
            yPosition.To = GameConstants.CANVASWIDTH;
            yPosition.Duration = new Duration(TimeSpan.FromMilliseconds(TTL));
            yPosition.SetValue(Storyboard.TargetPropertyProperty, TranslateTransformX);
            Storyboard.SetTarget(yPosition, UIElement);
            return yPosition;
        }

        private void Sb_Completed(object sender, object e)
        {
            ((Storyboard)sender).Completed -= Sb_Completed;
            Destroy();
        }
    }
}
