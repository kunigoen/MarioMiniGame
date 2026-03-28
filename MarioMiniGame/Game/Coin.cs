using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia;
using NAudio.Wave;

namespace MarioMiniGame.Game
{
    public class Coin
    {
        public Image Sprite;

        public double X;
        public double Y;

        public bool IsCollected = false;

        // 🔥 animation frames
        Bitmap c1, c2, c3, c4, c5, c6, c7;

        int frame = 0;
        int frameCounter = 0;

        // 🔊 SOUND
        private static WaveOutEvent soundOutput;
        private static AudioFileReader soundAudio;

        static Coin()
        {
            // load 1 lần duy nhất (tối ưu)
            soundAudio = new AudioFileReader("Image/Game_Sound/Coin.wav");
            soundOutput = new WaveOutEvent();
            soundOutput.Init(soundAudio);
        }

        public Coin(double x, double y)
        {
            X = x;
            Y = y;

            c1 = new Bitmap("Image/Animation_Coin/Coin.png");
            c2 = new Bitmap("Image/Animation_Coin/Coin1.png");
            c3 = new Bitmap("Image/Animation_Coin/Coin2.png");
            c4 = new Bitmap("Image/Animation_Coin/Coin3.png");
            c5 = new Bitmap("Image/Animation_Coin/Coin4.png");
            c6 = new Bitmap("Image/Animation_Coin/Coin5.png");
            c7 = new Bitmap("Image/Animation_Coin/Coin6.png");

            Sprite = new Image
            {
                Width = 30,
                Height = 30,
                Source = c1
            };

            Canvas.SetLeft(Sprite, X);
            Canvas.SetTop(Sprite, Y);
        }

        public void Update(double cameraX)
        {
            Canvas.SetLeft(Sprite, X - cameraX);
            UpdateAnimation();
        }

        void UpdateAnimation()
        {
            if (IsCollected) return;

            frameCounter++;

            if (frameCounter > 2)
            {
                frame++;
                frameCounter = 0;
            }

            int current = frame % 7;

            Sprite.Source = current switch
            {
                0 => c1,
                1 => c2,
                2 => c3,
                3 => c4,
                4 => c5,
                5 => c6,
                _ => c7
            };
        }

        public Rect GetBounds()
        {
            return new Rect(X, Y, Sprite.Width, Sprite.Height);
        }

        public void Collect()
        {
            if (IsCollected) return;

            IsCollected = true;
            Sprite.IsVisible = false;

            // 🔊 PLAY SOUND
            soundAudio.Position = 0;
            soundOutput.Play();
        }
    }
}