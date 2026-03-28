using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace MarioMiniGame.Game
{
    public class Win
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; private set; } = 30;
        public double Height { get; private set; } = 30;

        Image sprite;

        public Win(Canvas canvas, double x, double y)
        {
            X = x;
            Y = y;

            sprite = new Image
            {
                Source = new Bitmap("Image/Animation_Key/Key.png"),
                Width = Width,
                Height = Height
            };

            canvas.Children.Add(sprite);
        }

        public void Render(double cameraX)
        {
            Canvas.SetLeft(sprite, X - cameraX);
            Canvas.SetTop(sprite, Y);
        }

        public void Remove(Canvas canvas)
        {
            canvas.Children.Remove(sprite);
        }
    }
}