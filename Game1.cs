using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyMonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Ball ball;
    Paddle leftPaddle;
    Paddle rightPaddle;
    SpriteFont scoreFont;
    int rightPlayerScore;
    int leftPlayerScore;

    int frameCount = 0;
    float elapsedSeconds = 0;
    int currentFPS = 0;

    bool isGameOver = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Initialize game objects
        ball = new Ball(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), new Vector2(5, 5), 10);
        leftPaddle = new Paddle(30, GraphicsDevice.Viewport.Height / 2 - 50, 10, 100, 3, GraphicsDevice);
        rightPaddle = new Paddle(GraphicsDevice.Viewport.Width - 40, GraphicsDevice.Viewport.Height / 2 - 50, 10, 100, 3, GraphicsDevice, ball);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        scoreFont = Content.Load<SpriteFont>("ScoreFont");
        // TODO: use this.Content to load your game content here
    }

    private void CheckBallPaddleCollision(Paddle paddle, Ball ball)
    {
        if (ball.Position.X + ball.Radius * 2 >= paddle.Bounds.X && ball.Position.X <= paddle.Bounds.X + paddle.Bounds.Width)
        {
            if (ball.Position.Y + ball.Radius * 2 >= paddle.Bounds.Y && ball.Position.Y <= paddle.Bounds.Y + paddle.Bounds.Height)
            {
                var velocity = ball.Velocity;
                velocity.X = -ball.Velocity.X; // Reflect the ball off the top and bottom edges
                ball.Velocity = velocity;
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        // frames count
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        frameCount++;

        elapsedSeconds += elapsed;

        if (elapsedSeconds >= 1.0f)
        {
            currentFPS = frameCount;
            frameCount = 0;
            elapsedSeconds = 0;
        }

        // Exit logic
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (isGameOver && Keyboard.GetState().IsKeyDown(Keys.Enter)) {
            // Reset the game if Enter key is pressed
            leftPlayerScore = 0;
            rightPlayerScore = 0;
            isGameOver = false;
            ResetBall();
        }

        if (isGameOver) 
        {
            base.Update(gameTime);
            return;
        }

        // Game logic
        // Update game objects
        ball.Update();
        leftPaddle.Update();
        rightPaddle.Update();

        // Add collision detection and game logic here
        CheckBallPaddleCollision(leftPaddle, ball);
        CheckBallPaddleCollision(rightPaddle, ball);

        // Ensure the ball stays within the screen bounds
        if (ball.Position.Y < 0 || ball.Position.Y + ball.Radius * 2 > GraphicsDevice.Viewport.Height)
        {
            var velocity = ball.Velocity;
            velocity.Y = -ball.Velocity.Y; // Reflect the ball off the top and bottom edges
            ball.Velocity = velocity;
        }

        if (ball.Position.X < 0)
        {
            rightPlayerScore++;
            ResetBall();
        } 
        else if (ball.Position.X + ball.Radius * 2 > GraphicsDevice.Viewport.Width)
        {
            leftPlayerScore++;
            ResetBall();
        }

        if (leftPlayerScore >= 5 || rightPlayerScore >= 5)
        {
            isGameOver = true;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // Draw game objects
        _spriteBatch.Begin();

        if (!isGameOver) ball.Draw(_spriteBatch);
        leftPaddle.Draw(_spriteBatch);
        rightPaddle.Draw(_spriteBatch);

        _spriteBatch.DrawString(scoreFont, $"Player: {leftPlayerScore}", new Vector2(50, 50), Color.White);
        _spriteBatch.DrawString(scoreFont, $"AI: {rightPlayerScore}", new Vector2(GraphicsDevice.Viewport.Width - 200, 50), Color.White);
        
        if (isGameOver)
        {
            // Display "Game Over" message
            _spriteBatch.DrawString(scoreFont, "Game Over", new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 - 30), Color.Red);
            _spriteBatch.DrawString(scoreFont, "Press Enter to Restart", new Vector2(GraphicsDevice.Viewport.Width / 2 - 150, GraphicsDevice.Viewport.Height / 2 + 20), Color.White);
        }

        // Render FPS counter
        _spriteBatch.DrawString(scoreFont, $"FPS: {currentFPS}", new Vector2(10, 10), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ResetBall()
    {
        Random rand = new();
        int multiplierX = rand.Next(1, 3) % 2 == 0 ?  1 : -1;
        int multiplierY = rand.Next(1, 3) % 2 == 0 ?  1 : -1;
        ball.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
        ball.Velocity = new Vector2(5 * multiplierX, 5 * multiplierY);
    }
}
