using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Collections.Generic;


namespace MarioMiniGame.Game
{
    public class HeartManager
    {
        private Canvas canvas;
        private List<Image> hearts = new();

        private int maxHP;

        public HeartManager(Canvas canvas, int hp)
        {
            this.canvas = canvas;
            this.maxHP = hp;
            Init();
        }

        void Init()
        {
            for (int i = 0; i < maxHP; i++)
            {
                var heart = new Image
                {
                    Source = new Bitmap("Image/Heart.png"),
                    Width = 30,
                    Height = 30
                };

                hearts.Add(heart);
                canvas.Children.Add(heart);
            }
        }

        public void Update(int currentHP)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                Canvas.SetLeft(hearts[i], 10 + i * 35);
                Canvas.SetTop(hearts[i], 10);
                hearts[i].IsVisible = i < currentHP;
            }
        }

        public void Reset(int hp)
        {
            hearts.Clear();
            maxHP = hp;
            Init();
        }
    }
}