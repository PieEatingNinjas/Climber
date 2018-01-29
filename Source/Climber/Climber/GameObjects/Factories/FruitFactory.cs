using System;
using System.Linq;
using Windows.UI.Xaml;

namespace Climber
{
    public class FruitFactory
    {
        DispatcherTimer timer;
        Game game;

        public event EventHandler<IFruit> NewFruit;

        public FruitFactory(Game g, TimeSpan interval)
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
            var row = game.Random.Next(GameConstants.NUMBEROFROWS - 1);

            while (game.Fruits.Any(b => b.Row == row) || game.Player.Row == row || game.Enemies.Any(s => s is Spider && s.Row == row))
            {
                row--;
                if (row == -1)
                    row = GameConstants.NUMBEROFROWS - 1;
            }
            var banana = new Banana(9, 3000);
            banana.Start();
            NewFruit?.Invoke(this, banana);
        }

        public void Start()
        {
            timer.Start();
        }
    }
}
