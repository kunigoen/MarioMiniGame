using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Avalonia;

namespace MarioMiniGame.Game
{
    public class Enemy
    {
        public double X { get; set; }
        public double Y { get; set; }

        // Public property để GameController truy cập
        public double Width { get; private set; } = 40;
        public double Height { get; private set; } = 40;

        public Image Sprite { get; private set; }

        // Di chuyển qua lại
        private double speed = 2;
        private double minX;
        private double maxX;
        private bool movingRight = true;
        
        Bitmap[] frames;
        int currentFrame = 0;
        int frameDelay = 0;
        public Enemy(Canvas canvas, double x, double y, double moveRange = 100)
        {
            X = x;
            Y = y;
            minX = x;
            maxX = x + moveRange;

            frames = new Bitmap[]
            {
                new Bitmap("Image/Animation_Enemy/Enemy.png"),
                new Bitmap("Image/Animation_Enemy/Enemy1.png"),
                new Bitmap("Image/Animation_Enemy/Enemy2.png"),
                new Bitmap("Image/Animation_Enemy/Enemy3.png")
            };

            Sprite = new Image
            {
                Source = frames[0],
                Width = Width,
                Height = Height
            };
            Sprite.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);
            canvas.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        // Logic di chuyển
        public void Update()
        {
            if (movingRight)
            {
                X += speed;
                if (X >= maxX) movingRight = false;
            }
            else
            {
                X -= speed;
                if (X <= minX) movingRight = true;
            }
            UpdateDirection();
            UpdateAnimation();
        }
        void UpdateDirection()
        {
            Sprite.RenderTransform = new ScaleTransform
            {
                ScaleX = movingRight ? 1 : -1,
                ScaleY = 1
            };
        }
        void UpdateAnimation()
        {
            frameDelay++;
            if (frameDelay >= 15)
            {
                frameDelay = 0;
                currentFrame++;

                if (currentFrame >= frames.Length)
                    currentFrame = 0;

                Sprite.Source = frames[currentFrame];
            }
        }
        public void Render(double cameraX)
        {
            Canvas.SetLeft(Sprite, X - cameraX);
            Canvas.SetTop(Sprite, Y);
        }
    }
}