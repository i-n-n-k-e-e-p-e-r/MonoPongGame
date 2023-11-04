using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyMonoGame;

public class Ball
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public int Radius { get; set; }
    public Color Color { get; set; } = Color.White;

    public Ball(Vector2 position, Vector2 velocity, int radius)
    {
        Position = position;
        Velocity = velocity;
        Radius = radius;
    }

    public void Update()
    {
        Position += Velocity;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(new Rectangle((int)Position.X, (int)Position.Y, Radius * 2, Radius * 2), Color);
    }
}