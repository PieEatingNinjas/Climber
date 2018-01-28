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
            TranslateTransform.Y = Row * GameConstants.ROWHEIGHT;

            DoubleAnimation position = new DoubleAnimation();
            position.From = 0;// -GameConstants.DRAWABLEWIDTH;
            position.To = GameConstants.CANVASWIDTH;// + GameConstants.DRAWABLEWIDTH;
            position.Duration = new Duration(TimeSpan.FromMilliseconds(TTL));
            position.SetValue(Storyboard.TargetPropertyProperty, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)");
            Storyboard.SetTarget(position, UIElement);

            var sb = new Storyboard();
            sb.Children.Add(position);
            sb.Begin();
            sb.Completed += Sb_Completed;
        }

        private void Sb_Completed(object sender, object e)
        {
            ((Storyboard)sender).Completed -= Sb_Completed;
            Destroy();
        }
    }
}
