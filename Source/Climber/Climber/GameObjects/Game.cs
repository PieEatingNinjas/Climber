using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Climber
{
    public class Game : INotifyPropertyChanged
    {
        private object Lock = new object();

        FruitFactory BananaFactory { get; }
        EnemyFactory EnemyFactory { get; }

        public Random Random { get; }

        DispatcherTimer gameTimer;

        private int _BananaCount;

        public int BananaCount
        {
            get { return _BananaCount; }
            set { _BananaCount = value; OnPropertyChanged(); }
        }


        public IPlayer Player { get; }

        public List<IDrawableEntity> Drawables
        {
            get
            {
                return Enemies.Cast<IDrawableEntity>().Concat(Fruits.Cast<IDrawableEntity>()).ToList();
            }
        }

        public List<IEnemy> Enemies { get; }
        public List<IFruit> Fruits { get; }
        public Canvas GameCanvas { get; }

        TimeSpan _ElapsedTime = new TimeSpan();
        public TimeSpan ElapsedTime
        {
            get
            {
                return _ElapsedTime;
            }
            private set
            {
                _ElapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTimeString));
            }
        } 

        public string ElapsedTimeString
        {
            get => ((int)ElapsedTime.TotalSeconds).ToString("D3");
        }

        public Game(Canvas gameCanvas)
        {
            GameCanvas = gameCanvas;
            gameTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            gameTimer.Tick += GameTimer_Tick;

            Player = new Player(GameConstants.NUMBEROFROWS / 2);
            GameCanvas.Children.Add(Player.UIElement);

            Enemies = new List<IEnemy>();
            Fruits = new List<IFruit>();

            Random = new Random();

            BananaFactory = new FruitFactory(this, TimeSpan.FromSeconds(3));
            BananaFactory.NewFruit += BananaFactory_NewFruit;

            EnemyFactory = new EnemyFactory(this, TimeSpan.FromSeconds(1));
            EnemyFactory.NewEnemey += EnemyFactory_NewEnemey;
        }


        private void ItemDestroyed(object sender, EventArgs e)
        {
            ((IDrawableEntity)sender).Destroyed -= ItemDestroyed;
            GameCanvas.Children.Remove(((IDrawableEntity)sender).UIElement);
            ((IDrawableEntity)sender).Dispose();
        }

        public void Start()
        {
            Player.Start();
            gameTimer.Start();
            BananaFactory.Start();
            EnemyFactory.Start();
        }

        private void EnemyFactory_NewEnemey(object sender, IEnemy e) => AddEnemy(e);

        private void AddEnemy(IEnemy e)
        {
            lock(Lock)
            {
                e.Destroyed += ItemDestroyed;
                Enemies.Add(e);
                GameCanvas.Children.Add(e.UIElement);
            }
        }

        private void BananaFactory_NewFruit(object sender, IFruit f) => AddFruit(f);

        private void AddFruit(IFruit f)
        {
            lock (Lock)
            {
                f.Destroyed += ItemDestroyed;
                Fruits.Add(f);
                GameCanvas.Children.Add(f.UIElement);
            }
        }

        private void GameTimer_Tick(object sender, object o)
        {
            ElapsedTime = ElapsedTime.Add(gameTimer.Interval);
            lock (Lock)
            {
                gameTimer.Stop();
                foreach (var item1 in Drawables.Where(d => d.IsActive))
                {
                    var item1Rect = item1.GetRect(GameCanvas);
                    var playerRect = Player.GetRect(GameCanvas);
                    playerRect.Intersect(item1Rect);
                    if (!playerRect.IsEmpty)
                    {
                        var collision = CollisionHelper.HandlePlayerCollision(Player, item1, playerRect);
                        if(collision == CollisionType.EnemyGotPlayer)
                        {
                            GenericFixedDrawable explosion = new GenericFixedDrawable("💥", item1.Row);
                            explosion.Start();
                            Canvas.SetZIndex(explosion.UIElement, (int)(Player.UIElement.GetValue(Canvas.ZIndexProperty)) + 1);
                            GameCanvas.Children.Add(explosion.UIElement);
                            EndGame?.Invoke(this, null);
                            break;
                        }
                        else if(collision == CollisionType.PlayerGotFruit)
                        {
                            item1.Destroy();
                            BananaCount++;
                        }
                    }
                }

                foreach (var enemy in Enemies.Where(e => e.IsActive))
                {
                    var enemyType = enemy.GetType();
                    foreach (var item in Drawables.Where(d => d.IsActive && d.GetType() != enemyType))
                    {
                        var enemyRect = enemy.GetRect(GameCanvas);
                        var itemRect = item.GetRect(GameCanvas);
                        itemRect.Intersect(enemyRect);
                        if (!itemRect.IsEmpty)
                        {
                            if(CollisionHelper.HandleCollision(enemy, item, itemRect) == CollisionType.EnemyGotItem)
                                item.Destroy();
                        }
                    }
                }

                Enemies.RemoveAll(en => !en.IsActive);
                Fruits.RemoveAll(f => !f.IsActive);
                gameTimer.Start();
            }
        }

        public void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler EndGame;
    }
}
