using Avalonia.Controls;
using System;
using System.Collections.Generic;

namespace MarioMiniGame.Game
{
    public class GameController
    {
        public Player player;

        private List<Ground> grounds = new();
        private List<Enemy> enemies = new();
        private List<Coin> coins = new();
        private List<Win> keys = new();

        private Canvas canvas;

        private BackgroundManager bgManager;
        private HeartManager heartManager;

        private double cameraX = 0;
        private double lastGroundX = 2500;

        public bool moveLeft = false;
        public bool moveRight = false;

        private int playerHP = 3;
        private bool gameOver = false;

        public int Score { get; private set; } = 0;

        public Action? OnGameOver;
        public Action? OnWin;
        
        GameSound gameSound;
        // ================== CONSTRUCTOR ==================
        public GameController(Canvas canvas)
        {
            this.canvas = canvas;

            bgManager = new BackgroundManager(canvas);
            player = new Player(canvas);

            InitGround();
            SpawnEnemies();
            InitCoins();
            SpawnKey();

            heartManager = new HeartManager(canvas, playerHP);
            
            gameSound = new GameSound();
        }

        // ================== RESET ==================
        public void Reset()
        {
            canvas.Children.Clear();

            grounds.Clear();
            enemies.Clear();
            coins.Clear();
            keys.Clear();

            cameraX = 0;
            lastGroundX = 2500;
            playerHP = 3;
            gameOver = false;
            Score = 0;

            bgManager = new BackgroundManager(canvas);
            player = new Player(canvas);

            InitGround();
            SpawnEnemies();
            InitCoins();
            SpawnKey();

            heartManager = new HeartManager(canvas, playerHP);
            
            gameSound.Stop();
            gameSound = new GameSound();
            gameSound.Play();
        }

        // ================== INIT ==================
        void InitGround()
        {
            for (int i = 0; i < 50; i++)
            {
                var g = new Ground(canvas, i * 50,350);
                grounds.Add(g);
            }
        }

        void SpawnGround()
        {
            for (int i = 0; i < 10; i++)
            {
                var g = new Ground(canvas, lastGroundX,350);
                grounds.Add(g);
                lastGroundX += 50;
            }
        }

        void SpawnEnemies()
        {
            double groundY = 400;
            for (int i = 500; i < 4000; i += 500)
            {
                enemies.Add(new Enemy(canvas, i, groundY - 85));
            }
        }

        void SpawnKey()
        {
            keys.Add(new Win(canvas, 4500, 300));
        }

        void InitCoins()
        {
            double groundY = 400;

            for (int i = 300; i < 4000; i += 300)
            {
                var coin = new Coin(i, groundY - 150);
                var coin1 = new Coin(i + 30, groundY - 150);
                var coin2 = new Coin(i + 60, groundY - 150);
                coins.Add(coin);
                canvas.Children.Add(coin.Sprite);
                coins.Add(coin1);
                canvas.Children.Add(coin1.Sprite);
                coins.Add(coin2);
                canvas.Children.Add(coin2.Sprite);
            }
        }

        // ================== UPDATE ==================
        public void Update()
        {
            if (gameOver) return;

            // Spawn thêm ground nếu cần
            if (cameraX + 800 > lastGroundX - 200)
                SpawnGround();

            // Background
            bgManager.Update(cameraX);

            // Movement
            if (moveLeft) player.X -= 5;
            if (moveRight) player.X += 5;

            // Camera follow
            if (player.X > 300)
            {
                double targetCam = player.X - 300;

                if (moveLeft || moveRight)
                    cameraX += (targetCam - cameraX) * 0.15;
            }

            // Gravity
            player.ApplyGravity();

            // Check ground
            bool onGround = false;
            foreach (var g in grounds)
            {
                if (player.CheckGround(g))
                {
                    onGround = true;
                    break;
                }
            }

            player.SetGroundState(onGround);

            // Render player
            player.Render(cameraX);

            // Render ground
            foreach (var g in grounds)
                g.Render(cameraX);

            // Enemy
            foreach (var e in enemies)
            {
                e.Update();
                e.Render(cameraX);
            }

            // Coins
            foreach (var coin in coins)
                coin.Update(cameraX);

            // Key
            foreach (var k in keys)
                k.Render(cameraX);

            // Collision
            CheckCollision();

            // Win
            CheckWin();

            // UI
            heartManager.Update(playerHP);

            // Animation
            bool isMoving = moveLeft || moveRight;
            player.UpdateDirection(moveLeft, moveRight);
            player.UpdateAnimation(isMoving);
        }

        // ================== INPUT ==================
        public void Jump()
        {
            if (!gameOver)
                player.Jump();
        }

        // ================== COLLISION ==================
        void CheckCollision()
        {
            // Enemy
            foreach (var e in enemies)
            {
                if (player.X + player.Width > e.X && player.X < e.X + e.Width &&
                    player.Y + player.Height > e.Y && player.Y < e.Y + e.Height)
                {
                    if (playerHP > 0) 
                    {
                        playerHP--;
                        player.X -= 200; 
                    }
                    if (playerHP <= 0)
                    {
                        gameOver = true;
                        gameSound.Stop();
                        OnGameOver?.Invoke();
                    }
                    break;
                }
            }
            // Coin
            foreach (var coin in coins)
            {
                if (!coin.IsCollected &&
                    player.X + player.Width > coin.X && player.X < coin.X + 30 &&
                    player.Y + player.Height > coin.Y && player.Y < coin.Y + 30)
                {
                    coin.Collect();
                    Score++;
                }
            }
        }

        // ================== WIN ==================
        void CheckWin()
        {
            foreach (var k in keys)
            {
                if (player.X + player.Width > k.X && player.X < k.X + k.Width &&
                    player.Y + player.Height > k.Y && player.Y < k.Y + k.Height)
                {
                    gameOver = true;
                    OnWin?.Invoke();
                    break;
                }
            }
        }
        public void StartGame()
        {
            gameSound.Play(); // 🎵 chạy nhạc
        }
        public void StopGame()
        {
            gameSound.Stop(); // 🔇 tắt nhạc
        }
    }
}