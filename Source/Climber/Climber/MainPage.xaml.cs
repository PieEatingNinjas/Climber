using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Climber
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        RadialController myController;

        public Game Game { get; }

        public MainPage()
        {
            this.InitializeComponent();
            Game = new Game(gameCanvas);
            Game.EndGame += Game_EndGame;
           // GamepadListener();
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;


            myController = RadialController.CreateForCurrentView();

            RandomAccessStreamReference icon =
          RandomAccessStreamReference.CreateFromUri(
            new Uri("ms-appx:///Assets/18-monkey-with-banana.png"));

            RadialControllerMenuItem myItem =
              RadialControllerMenuItem.CreateFromIcon("START!", icon);
            myItem.Invoked += MyItem_Invoked;
            myController.Menu.Items.Add(myItem);
            myController.RotationChanged += MyController_RotationChanged;
            myController.UseAutomaticHapticFeedback = false;
        }

        private async void Game_EndGame(object sender, EventArgs e)
        {
            Debug.WriteLine("DEAD!!!");
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(maingrid, (int)maingrid.ActualWidth, (int)maingrid.ActualHeight);
            Frame.Navigate(typeof(BlankPage1), renderTargetBitmap);
        }

        private async void GamepadListener()
        {
            while (true)
            {
                // TaskCompletionSource<GamepadReading> readTask = new TaskCompletionSource<GamepadReading>();
                await Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, () =>
                {
                    if (Gamepad == null)
                    {
                        return;
                    }

                    // Get the current state
                    var reading = Gamepad.GetCurrentReading();


                    if (reading.Buttons.HasFlag(GamepadButtons.A))
                        Start();
                    else if (reading.Buttons.HasFlag(GamepadButtons.DPadUp))
                        Game.Player.Up();
                    else if (reading.Buttons.HasFlag(GamepadButtons.DPadDown))
                        Game.Player.Down();
                });
                await Task.Delay(TimeSpan.FromMilliseconds(30));
            }
        }

        Gamepad Gamepad;
        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            Gamepad = e;
            GamepadListener();
        }

        private void MyItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            Start();
        }
        bool isStarted = false;
        private void Start()
        {
            if(!isStarted)
            {
                isStarted = true;
                Game.Start();
            }
        }

        private void MyController_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (Math.Abs(args.RotationDeltaInDegrees) >= 10)
            {

                bool isUp = args.RotationDeltaInDegrees < 0;
                if (isUp)
                    Game.Player.Up();
                else
                    Game.Player.Down();
            }
        }

        //private void BananaGenerator_Tick(object sender, object e)
        //{
        //    GenerateBanana();
        //}

        //private void EnemyGenerator_Tick(object sender, object e)
        //{
        //    GenerateEnemy();
        //}

        private async void ColissionTimer_Tick(object sender, object e)
        {
            //colissionTimer.Stop();
            //foreach (var enemy in Enemies.Where(a => a.IsActive).ToList())
            //{
            //    Rect eRect;
            //    //if (enemy.Row == p.Row)
            //    //{
            //    eRect = enemy.GetRect(gameCanvas);

            //    var playerRect = p.GetRect(gameCanvas);
            //    playerRect.Intersect(eRect);
            //    if (!playerRect.IsEmpty)
            //    {
            //        if (playerRect.Height >= 23 && playerRect.Width >= 20)
            //        {
            //            GenericFixedDrawable b = new GenericFixedDrawable("💥", enemy.Row);
            //            b.Start();
            //            Canvas.SetZIndex(b.UIElement, (int)(p.UIElement.GetValue(Canvas.ZIndexProperty))+1);
            //            gameCanvas.Children.Add(b.UIElement);

            //            Debug.WriteLine("DEAD!!!");
            //            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            //            await renderTargetBitmap.RenderAsync(maingrid, (int)maingrid.ActualWidth, (int)maingrid.ActualHeight);
            //            Debug.WriteLine(playerRect.Width);
            //            Frame.Navigate(typeof(BlankPage1), renderTargetBitmap);


            //        }
            //        else if (playerRect.Height >= 10 && playerRect.Width >= 10)
            //        {
            //            Debug.WriteLine("We got a badass!!");
            //        }
            //    }
            //    //  }

            //    eRect = enemy.GetRect(gameCanvas);

            //    foreach (var banana in Bananas.Where(b => /*b.Row == enemy.Row && */b.IsActive).ToList())
            //    {
            //        var pRect = p.GetRect(gameCanvas);
            //        pRect.Intersect(banana.GetRect(gameCanvas));
            //        if (!pRect.IsEmpty && pRect.Height >= 23)
            //        //if (banana.Row == p.Row)
            //        {
            //            Debug.WriteLine("YUMMIE");
            //            banana.Destroy();
            //            BananaCount++;
            //            continue;
            //        }

            //        var bRect = banana.GetRect(gameCanvas);
            //        bRect.Intersect(eRect);
            //        if (!bRect.IsEmpty)
            //        {
            //            Debug.WriteLine("HIT");
            //            banana.Destroy();
            //            continue;
            //        }
            //    }
            //}

            //Enemies.RemoveAll(en => !en.IsActive);
            //Bananas.RemoveAll(ba => !ba.IsActive);
            //colissionTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //private void GenerateEnemy()
        //{
        //    // var rand = new Random(1);
        //    var enemy = new Enemy(rand.Next(9), 2000);
        //    enemy.Destroyed += Enemy_Completed;
        //    gameCanvas.Children.Add(enemy.UIElement);
        //    enemy.Start();
        //    Enemies.Add(enemy);
        //}
        //Random rand = new Random();
        //private void GenerateBanana()
        //{
        //    //var rand = new Random();
        //    var row = rand.Next(9);

        //    while (Bananas.Any(b => b.Row == row) || p.Row == row)
        //    {
        //        row--;
        //        if (row == -1)
        //            row = 8;
        //    }
        //    var banana = new Banana(row, 3000);
        //    banana.Destroyed += Banana_Completed;
        //    gameCanvas.Children.Add(banana.UIElement);
        //    banana.Start();
        //    Bananas.Add(banana);
        //}

        //private void Banana_Completed(object sender, EventArgs e)
        //{
        //    ((Banana)sender).Destroyed -= Enemy_Completed;
        //    gameCanvas.Children.Remove(((Banana)sender).UIElement);
        //    ((Banana)sender).Dispose();
        //}

        //private void Enemy_Completed(object sender, EventArgs e)
        //{
        //    ((Enemy)sender).Destroyed -= Enemy_Completed;
        //    gameCanvas.Children.Remove(((Enemy)sender).UIElement);
        //    ((Enemy)sender).Dispose();
        //}
    }
}
