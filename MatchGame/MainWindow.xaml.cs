using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MatchGame
{
    using System.Windows.Threading;
    using System.IO;
    using Microsoft.VisualBasic;
    using System.Diagnostics;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch sw = new Stopwatch();
        DispatcherTimer timer = new();
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
            timeTextBlock.Text = (tenthOfSecond / 10F).ToString("0.0");
            if (matchesFound == 8)
            {
                timer.Stop();
                sw.Stop();
                TimeSpan resolvedTime = sw.Elapsed;
                CheckIfWinner(resolvedTime);
                CheckIfPlayAgain();
            }
        }

        private void CheckIfPlayAgain()
        {
            string messageBoxText = "Do you want to play again?";
            string caption = "Match game";
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                SetUpGame();
            }
            else
            {
                this.Close();
            }
        }

        private void CheckIfWinner(TimeSpan Time)
        {
            if (Time > TimeSpan.Parse(bestScoreTime.Text))
            {
                BestPlayer player = new BestPlayer();
                string stringTime = String.Format("{0}:{1}:{2}.{3}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds);
                bestScoreTime.Text = stringTime;
                string winnerName = Interaction.InputBox("You got the best time! What's your name?", "Congratulations!", "Name");
                bestScoreName.Text = winnerName;
                player.Time = stringTime;
                player.Name = winnerName;
                File.WriteAllText("BestScore.json", Newtonsoft.Json.JsonConvert.SerializeObject(player));
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
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "bestScoreTitle" && textBlock.Name != "bestScoreName" && textBlock.Name != "bestScoreTime")
                {
                    int index = random.Next(emojiList.Count);
                    textBlock.Text = emojiList[index];
                    emojiList.RemoveAt(index);
                }
            }
            if (File.Exists("BestScore.json"))
            {
                BestPlayer bestPlayerScore = Newtonsoft.Json.JsonConvert.DeserializeObject<BestPlayer>(File.ReadAllText("BestScore.json"));
                bestScoreTime.Text = bestPlayerScore.Time;
                bestScoreName.Text = bestPlayerScore.Name;
            }
            timer.Start();
            if (sw.IsRunning)
            {
                sw.Restart();
            }
            else
            {
                sw.Start();
            }
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
    }
}
