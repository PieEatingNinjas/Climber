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
                        Game.Player.Climb(ClimbingDirection.Up);
                    else if (reading.Buttons.HasFlag(GamepadButtons.DPadDown))
                        Game.Player.Climb(ClimbingDirection.Down);
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
                    Game.Player.Climb(ClimbingDirection.Up);
                else
                    Game.Player.Climb(ClimbingDirection.Down);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
