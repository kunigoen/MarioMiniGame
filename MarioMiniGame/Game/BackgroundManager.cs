using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System.Collections.Generic;

namespace MarioMiniGame.Game
{
    public class BackgroundManager
    {
        private Canvas canvas;
        private List<Image> backgrounds = new();

        private double bgWidth = 800;
        private double lastBgX = 0;

        public BackgroundManager(Canvas canvas)
        {
            this.canvas = canvas;
            Init();
        }

        void Init()
        {
            for (int i = 0; i < 3; i++)
            {
                var bg = CreateBackground(lastBgX);
                backgrounds.Add(bg);
                lastBgX += bgWidth;
            }
        }

        Image CreateBackground(double worldX)
        {
            var bg = new Image
            {
                Source = new Bitmap("Image/BackGroundGame.png"),
                Width = bgWidth,
                Height = 450,
                Tag = worldX
            };
            canvas.Children.Insert(0, bg);
            return bg;
        }

        public void Update(double cameraX)
        {
            foreach (var bg in backgrounds)
            {
                double worldX = (double)bg.Tag;
                double screenX = worldX - cameraX * 0.3;

                Canvas.SetLeft(bg, screenX);
                Canvas.SetTop(bg, 0);
            }

            while (lastBgX - cameraX * 0.3 < bgWidth * 2)
            {
                var bg = CreateBackground(lastBgX);
                backgrounds.Add(bg);
                lastBgX += bgWidth;
            }
        }

        public void Reset()
        {
            backgrounds.Clear();
            lastBgX = 0;
            Init();
        }
    }
}