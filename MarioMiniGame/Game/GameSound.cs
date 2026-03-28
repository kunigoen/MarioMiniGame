using NAudio.Wave;

namespace MarioMiniGame.Game
{
    public class GameSound
    {
        private WaveOutEvent output;
        private AudioFileReader audio;

        private bool isLooping = false; // 🔥 control loop

        public GameSound()
        {
            audio = new AudioFileReader("Image/Game_Sound/MusicBackGround.wav");
            output = new WaveOutEvent();
            output.Init(audio);

            output.PlaybackStopped += (s, e) =>
            {
                if (isLooping) // ✅ chỉ loop khi cho phép
                {
                    audio.Position = 0;
                    output.Play();
                }
            };
        }

        public void Play()
        {
            isLooping = true;     // 🔥 bật loop
            audio.Position = 0;   // chạy từ đầu
            output.Play();
        }

        public void Stop()
        {
            isLooping = false;    // 🔥 tắt loop
            output.Stop();
        }
    }
}