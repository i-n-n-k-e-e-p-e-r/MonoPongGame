using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyMonoGame;

public class Paddle
{
    public Rectangle Bounds { get; set; }
    public int Speed { get; set; }
    public Ball Ball { get; set; }

    private GraphicsDevice Device { get; set; }

    public Paddle(int x, int y, int width, int height, int speed, GraphicsDevice graphicsDevice, Ball ball = null)
    {
        Bounds = new Rectangle(x, y, width, height);
        Speed = speed;
        Device = graphicsDevice;
        Ball = ball;
    }

    public void Update()
    {
        if (Ball is not null) AutoUpdate();
        else ManualUpdate();
    }

    private void AutoUpdate()
    {
        if (Ball.Position.Y == Bounds.Y) return;
        if (Ball.Position.Y > Bounds.Y)
        {
            Bounds = new() { X = Bounds.X, Y = Bounds.Y + Speed, Width = Bounds.Width, Height = Bounds.Height };
        } else {
            Bounds = new() { X = Bounds.X, Y = Bounds.Y - Speed, Width = Bounds.Width, Height = Bounds.Height };
        }

         Bounds = new() { X = Bounds.X, Y = MathHelper.Clamp(Bounds.Y, 0, Device.Viewport.Height - Bounds.Height), Width = Bounds.Width, Height = Bounds.Height };
    }

    private void ManualUpdate()
    {
        // Get the current keyboard state
        KeyboardState keyboardState = Keyboard.GetState();

        // Move the paddle up
        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            Bounds = new() { X = Bounds.X, Y = Bounds.Y - Speed, Width = Bounds.Width, Height = Bounds.Height };
        }

        // Move the paddle down
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            Bounds = new() { X = Bounds.X, Y = Bounds.Y + Speed, Width = Bounds.Width, Height = Bounds.Height };
        }

        // Ensure the paddle stays within the screen bounds
        Bounds = new() { X = Bounds.X, Y = MathHelper.Clamp(Bounds.Y, 0, Device.Viewport.Height - Bounds.Height), Width = Bounds.Width, Height = Bounds.Height };
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(Bounds, Color.White);
    }
}