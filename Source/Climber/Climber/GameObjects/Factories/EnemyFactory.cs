using System;
using Windows.UI.Xaml;

namespace Climber
{
    public class EnemyFactory
    {
        DispatcherTimer timer;
        Game game;

        public event EventHandler<IEnemy> NewEnemey;

        public EnemyFactory(Game g, TimeSpan interval)
        {
            timer = new DispatcherTimer()
            {
                Interval = interval
            };
            timer.Tick += Timer_Tick;

            game = g;
        }

        private void Timer_Tick(object sender, object e)
        {
            IEnemy enemy = null;
            var row = game.Random.Next(9);
            if (Math.Abs(row-game.Player.Row) > 1 && game.Random.Next(5) == 1)
                enemy = new Spider(row, 5000);
            else
                enemy = new Snake(row, 2000);
            enemy.Start();
            NewEnemey?.Invoke(this, enemy);
        }

        public void Start()
        {
            timer.Start();
        }
    }
}
