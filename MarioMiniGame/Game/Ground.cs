using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Media;

namespace MarioMiniGame.Game;

public class Ground
{
    public double X;
    public double Y;

    Image sprite;

    public Ground(Canvas canvas, double x, double y)
    {
        X = x;
        Y = y;
        sprite = new Image
        {
            Source = new Bitmap("Image/ground.png"),
            Width = 50,
            Height = 50
        };

        RenderOptions.SetBitmapInterpolationMode(sprite, BitmapInterpolationMode.LowQuality);
        Canvas.SetLeft(sprite, X);
        Canvas.SetTop(sprite, Y);
        canvas.Children.Add(sprite);
    }

    public void Render(double cameraX)
    {
        Canvas.SetLeft(sprite, X - cameraX);
        Canvas.SetTop(sprite, Y);
    }
}