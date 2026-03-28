using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using System;
using MarioMiniGame.Game;
using Avalonia.Interactivity;

namespace MarioMiniGame;

public partial class MainWindow : Window
{
    DispatcherTimer timer = new DispatcherTimer();

    GameController game;
    bool isGameStarted = false;

    public MainWindow()
    {
        InitializeComponent();

        game = new GameController(GameCanvas);

        // 👉 nhận event Game Over từ controller
        game.OnGameOver += ShowGameOver;
        game.OnWin += ShowWin;
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;

        timer.Interval = TimeSpan.FromMilliseconds(16);
        timer.Tick += Update;
        timer.Start();
        ScoreText.Text = "Score 🪙: 0";
    }

    void Update(object? sender, EventArgs e)
    {
        if (!isGameStarted) return;

        game.Update();
        ScoreText.Text = $"Score 🪙: {game.Score}";
    }

    void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left) game.moveLeft = true;
        if (e.Key == Key.Right) game.moveRight = true;
        if (e.Key == Key.Space) game.Jump();
    }
    void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Left) game.moveLeft = false;
        if (e.Key == Key.Right) game.moveRight = false;
    }

    private void StartGame_Click(object? sender, RoutedEventArgs e)
    {
        MenuPanel.IsVisible = false;
        GameOverPanel.IsVisible = false; // reset Game Over
        WinPanel.IsVisible = false;
        GameCanvas.IsVisible = true;
        
        isGameStarted = true;
        game.Reset();
        game.StartGame();
        ScoreText.Text = "🪙 0";
    }

    private void ExitGame_Click(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
    // 👉 hiển thị Game Over
    void ShowGameOver()
    {
        GameCanvas.IsVisible = false;
        GameOverPanel.IsVisible = true;
        isGameStarted = false;
        game.StopGame(); 
    }
    void ShowWin()
    {
        isGameStarted = false;
        GameCanvas.IsVisible = false;
        MenuPanel.IsVisible = false;
        GameOverPanel.IsVisible = false;
        WinPanel.IsVisible = true;
        game.StopGame();
    }
}