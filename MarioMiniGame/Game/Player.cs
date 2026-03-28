using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Media;
using Avalonia;
using NAudio.Wave;

namespace MarioMiniGame.Game;
public class Player
{
    public double X = 100;
    public double Y = 200;

    double velocityY = 0;
    bool isJumping = false;

    double width = 50;
    double height = 50;
    
    public double Width => width;
    public double Height => height;

    Image sprite;
    bool facingRight = true;
    
    Bitmap idle,run1,run2,run3;
    Bitmap jump1, jump2, jump3, jump4, jump5;
    
    int frame = 0;
    int frameCounter = 0;
    
    private WaveOutEvent jumpOutput;
    private AudioFileReader jumpAudio;
    
    public Player(Canvas canvas)
    {
        idle = new Bitmap("Image/Animation_Player/Player_Idle.png");
        run1 = new Bitmap("Image/Animation_Player/Player_run1.png");
        run2 = new Bitmap("Image/Animation_Player/Player_run2.png");
        run3 = new Bitmap("Image/Animation_Player/Player_run3.png");

        jump1 = new Bitmap("Image/Animation_Jump/Jump1.png");
        jump2 = new Bitmap("Image/Animation_Jump/Jump2.png");
        jump3 = new Bitmap("Image/Animation_Jump/Jump3.png");
        jump4 = new Bitmap("Image/Animation_Jump/Jump4.png");
        jump5 = new  Bitmap("Image/Animation_Player/Player_run1.png");
        
        sprite = new Image
        {
            Source = idle,
            Width = width,
            Height = height
        };

        RenderOptions.SetBitmapInterpolationMode(sprite, BitmapInterpolationMode.LowQuality);
        sprite.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);
        canvas.Children.Add(sprite);
        
        jumpAudio = new AudioFileReader("Image/Game_Sound/Jump.wav");
        jumpOutput = new WaveOutEvent();
        jumpOutput.Init(jumpAudio);
    }

    public void ApplyGravity()
    {
        velocityY += 0.5;
        Y += velocityY;
    }

    public bool CheckGround(Ground g)
    {
        if (X + width > g.X && X < g.X + 50)
        {
            if (Y + height >= g.Y && velocityY >= 0)
            {
                Y = g.Y - height;
                velocityY = 0;
                isJumping = false;
                return true;
            }
        }
        return false;
    }

    public void SetGroundState(bool onGround)
    {
        if (!onGround)
            isJumping = true;
    }

    public void Render(double cameraX)
    {
        Canvas.SetLeft(sprite, X - cameraX);
        Canvas.SetTop(sprite, Y);
    }

    public void Jump()
    {
        if (!isJumping)
        {
            velocityY = -9;
            isJumping = true;
            // 🔥 reset animation khi jump
            frame = 0;
            frameCounter = 0;
            
            jumpAudio.Position = 0;
            jumpOutput.Play();
        }
    }
    public void UpdateDirection(bool moveLeft, bool moveRight)
    {
        if (moveLeft)
            facingRight = false;
        else if (moveRight)
            facingRight = true;

        sprite.RenderTransform = new ScaleTransform
        {
            ScaleX = facingRight ? 1 : -1,
            ScaleY = 1
        };
    }
    public void UpdateAnimation(bool isMoving)
    {
        // 🔥 JUMP animation (4 frame)
        if (isJumping)
        {
            frameCounter++;
            if (frameCounter > 3.5)
            {
                frame++;
                frameCounter = 0;
            }
            if (frame > 4)
                frame = 4;
            if (frame == 0)
                sprite.Source = jump1;
            else if (frame == 1)
                sprite.Source = jump2;
            else if (frame == 2)
                sprite.Source = jump3;
            else if (frame == 3)
                sprite.Source = jump4;
            else 
                sprite.Source = jump5;
            return;
        }
        // 🔥 IDLE
        if (!isMoving)
        {
            sprite.Source = idle;
            return;
        }
        // 🔥 RUN animation
        frameCounter++;

        if (frameCounter > 6)
        {
            frame++;
            frameCounter = 0;
        }
        int runFrame = frame % 3;
        if (runFrame == 0)
            sprite.Source = run1;
        else if (runFrame == 1)
            sprite.Source = run2;
        else
            sprite.Source = run3;
    }
}