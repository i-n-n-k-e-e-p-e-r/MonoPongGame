using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyMonoGame;

public static class SpriteBatchExtensions
{
    private static Texture2D rect;
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, int thickness = 1)
    {
        Texture2D blankTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        blankTexture.SetData(new[] { Color.White });

        spriteBatch.Draw(blankTexture, new Rectangle(rectangle.Left, rectangle.Top, thickness, rectangle.Height), color); // Left
        spriteBatch.Draw(blankTexture, new Rectangle(rectangle.Right, rectangle.Top, thickness, rectangle.Height), color); // Right
        spriteBatch.Draw(blankTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, thickness), color); // Top
        spriteBatch.Draw(blankTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, thickness), color); // Bottom
    }
}