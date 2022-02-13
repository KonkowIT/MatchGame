using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthOfSecond;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthOfSecond++;
            timeTextBlock.Text = (tenthOfSecond / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                string messageBoxText = "Do you want to play again?";
                string caption = "Match game";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                if (result == MessageBoxResult.Yes)
                {
                    SetUpGame();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void SetUpGame()
        {
            List<string> emojiList = new List<string>()
            {
                "🐶","🐶",
                "🦝","🦝",
                "🐮","🐮",
                "🐷","🐷",
                "🐱","🐱",
                "🐯","🐯",
                "🐨","🐨",
                "🐭","🐭"
            };
            Random random = new Random();
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Visibility == Visibility.Hidden)
                {
                    textBlock.Visibility = Visibility.Visible;
                }
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(emojiList.Count);
                    textBlock.Text = emojiList[index];
                    emojiList.RemoveAt(index);
                }
            }
            timer.Start();
            matchesFound = 0;
            tenthOfSecond = 0;
        }
        TextBlock lastClickResult;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (lastClickResult == null)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastClickResult = textBlock;
            }
            else if (textBlock.Text == lastClickResult.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                lastClickResult = null;
            }
            else
            {
                lastClickResult.Visibility = Visibility.Visible;
                lastClickResult = null;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {

                SetUpGame();
            }
        }
    }
}
