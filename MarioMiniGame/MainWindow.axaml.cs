using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using System;
using MarioMiniGame.Game;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace MarioMiniGame;

public partial class MainWindow : Window
{
    DispatcherTimer timer = new DispatcherTimer();

    GameController game;
    bool isGameStarted = false;
    private int frameCount = 0;
    private Stopwatch fpsTimer = new Stopwatch();

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
        
        // 👉 FPS 
        frameCount++;

        if (fpsTimer.ElapsedMilliseconds >= 1000)
        {
            int fps = frameCount;

            FPSText.Text = $"FPS: {fps}";

            frameCount = 0;
            fpsTimer.Restart();
        }
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
        ScoreText.IsVisible = true;
        
        fpsTimer.Restart();
        frameCount = 0;
        FPSText.IsVisible = true;
        
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
        FPSText.IsVisible = false;
        game.StopGame(); 
    }
    void ShowWin()
    {
        isGameStarted = false;
        GameCanvas.IsVisible = false;
        MenuPanel.IsVisible = false;
        GameOverPanel.IsVisible = false;
        FPSText.IsVisible = false;
        WinPanel.IsVisible = true;
        game.StopGame();
    }
}