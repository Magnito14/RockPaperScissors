using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RockPaperScissors;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    private Dictionary<Winner, int> winCount = new();

    public MainWindow() {
        InitializeComponent();
    }

    private void ExitBtn_Click(object sender, RoutedEventArgs e)
        => Process.GetCurrentProcess().Kill();

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void MainBrdr_MouseDown(object sender, MouseButtonEventArgs e) {
        if (e.LeftButton == MouseButtonState.Pressed) {
            DragMove();
        }
    }

    private void RockBtn_Click(object sender, RoutedEventArgs e)
        => PlayChoice(Choice.Rock);

    private void PaperBtn_Click(object sender, RoutedEventArgs e)
        => PlayChoice(Choice.Paper);

    private void ScissorsBtn_Click(object sender, RoutedEventArgs e)
        => PlayChoice(Choice.Scissors);

    private void ResetBtn_Click(object sender, RoutedEventArgs e) {
        winCount.Clear();

        winCount = Enum.GetValues(typeof(Winner)).Cast<Winner>().ToDictionary(player => {
            return player;
        }, player => {
            return 0;
        });

        PlayerLbl.Content = "Player choice:";
        ComputerLbl.Content = "Computer choice:";
        WinnerLbl.Content = "Winner:";

        UpdateGame();
    }

    private void PlayChoice(Choice playerChoice) {
        Choice computerChoice = (Choice)new Random().Next(Enum.GetValues(typeof(Choice)).Length);
        Winner gameWinner = GetWinner(playerChoice, computerChoice);

        PlayerLbl.Content = $"Player choice: {playerChoice}";
        ComputerLbl.Content = $"Computer choice: {computerChoice}";
        WinnerLbl.Content = $"Winner: {gameWinner}";

        if (winCount.TryGetValue(gameWinner, out int wins)) {
            winCount.Remove(gameWinner);
        }

        winCount.Add(gameWinner, wins + 1);
        UpdateGame();
    }

    private void UpdateGame() {
        string winCountText = string.Join("\r\n", winCount.Select(x => {
            return $"{x.Key}: {x.Value}";
        }));

        ResultBox.Items.Clear();
        ResultBox.Items.Add(winCountText);
    }

    private static Winner GetWinner(Choice playerChoice, Choice computerChoice) {
        Winner[,] choices = new Winner[,] {
            { Winner.Tie, Winner.Computer, Winner.Player },
            { Winner.Player, Winner.Tie, Winner.Computer },
            { Winner.Computer, Winner.Player, Winner.Tie },
        };

        if (playerChoice == computerChoice) {
            return Winner.Tie;
        }

        return choices[(uint)playerChoice, (uint)computerChoice];
    }
}
