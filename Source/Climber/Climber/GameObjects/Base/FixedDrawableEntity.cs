using System;
using Windows.UI.Xaml;

namespace Climber
{
    public abstract class FixedDrawableEntity : DrawableEntity
    {
        DispatcherTimer maxTtlTimer;
        int? ttl;

        public FixedDrawableEntity(int row, int? maxTimeToLive) : base(row)
        {
            ttl = maxTimeToLive;
        }

        protected override void StartInternal()
        {
            TranslateTransform.Y = Row * GameConstants.ROWHEIGHT;
            TranslateTransform.X = GameConstants.CANVASWIDTH / 2 - GameConstants.DRAWABLEWIDTH / 2;

            if (ttl.HasValue)
            {
                maxTtlTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(ttl.Value)
                };
                maxTtlTimer.Tick += Timer_Tick;
                maxTtlTimer.Start();
            }
        }

        protected override void DestroyInternal()
        {
            if (maxTtlTimer != null)
            {
                maxTtlTimer.Stop();
                maxTtlTimer.Tick -= Timer_Tick;
                maxTtlTimer = null;
            }
        }

        private void Timer_Tick(object sender, object e) => Destroy();
    }
}
