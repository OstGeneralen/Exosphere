using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    public class ScrollBox
    {

        //A list containing the buttons that should be in the scroll box
        private List<Button> buttons;
        private List<Button> buttonsToDraw;

        //The texture of the box
        Texture2D texture;

        //The position of the box
        Vector2 position;

        //The rectangle that decides if a button should be drawn or not
        Rectangle drawingRectangle;

        //The buttons for going up and down
        Button up;
        Button down;
        Button close;

        public ScrollBox(List<Button> buttons)
        {
            //Load the inputed list into the local
            this.buttons = buttons;

            //Create a temp-string holding the assetName for further readability
            string assetName = "Res/PH/HUD/Message Boxes/MessageBox";

            //Load the texture of the box using the previously defined asset name 
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            //Create the buttons with temp position (0,0)
            up = new Button("Res/PH/HUD/Buttons/Standard/UpScrollButtonPH", Vector2.Zero);
            down = new Button("Res/PH/HUD/Buttons/Standard/DownScrollButtonPH", Vector2.Zero);
            close = new Button("Res/PH/HUD/Buttons/Standard/CloseButtonPH", Vector2.Zero);

            buttonsToDraw = new List<Button>();

            //TODO: Give the up and down-buttons their correct position

            //Positions all the objects that should be shown on the screen
            CenterBox();
            up.SetPosition(PositionButton("up"));
            down.SetPosition(PositionButton("down"));
            close.SetPosition(PositionButton("close"));
            CreateDrawingRectangle();
            PositionButtonsInList();

            foreach (var button in buttons)
            {
                button.Update();
            }
        }

        #region Positions
        /// <summary>
        /// Centers the box based on the screen resolution and the box's texture
        /// </summary>
        private void CenterBox()
        {
            int x = 0;
            int y = 0;

            x = (int)((Settings.GetScreenRes.X / 2) - (texture.Width / 2));
            y = (int)((Settings.GetScreenRes.Y / 2) - (texture.Height / 2));

            position = new Vector2(x, y);
        }

        /// <summary>
        /// Positions the button depending on which button it is
        /// </summary>
        /// <param name="buttonName">"up" or "down" is valid names in the current context</param>
        /// <returns>A vector 2 position</returns>
        private Vector2 PositionButton(string buttonName)
        {
            int x = 0;
            int y = 0;

            if (buttonName == "up")
            {
                x = (int)(position.X + texture.Width - up.GetTexture().Width * 3);
                y = (int)(position.Y + up.GetTexture().Height * 2);
            }
            else if (buttonName == "down")
            {
                x = (int)(position.X + texture.Width - down.GetTexture().Width * 3);
                y = (int)(position.Y + texture.Height - down.GetTexture().Height * 3);
            }
            else if (buttonName == "close")
            {
                x = (int)(position.X + texture.Width - close.GetTexture().Width * 1 - 8);
                y = (int)(position.Y + 8);// + up.GetTexture().Height);
            }
            else
            {
                throw new NullReferenceException("The button name does not exist in the current context");
            }


            return new Vector2(x, y);
        }

        private void CreateDrawingRectangle()
        {
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            int heightOffset = 12;

            if (buttons.Count > 0)
            {
                w = buttons[0].GetTexture().Width;
                h = texture.Height - (buttons[0].GetTexture().Height * 2) + heightOffset;
                x = (int)(position.X + (texture.Width / 2) - (w / 2));
                y = (int)(position.Y + (texture.Height / 2) - (h / 2));
            }

            drawingRectangle = new Rectangle(x, y, w, h);
        }

        private void PositionButtonsInList()
        {
            int amount = 0;

            foreach (var button in buttons)
            {
                int x = 0;
                int y = 0;

                x = drawingRectangle.X;
                y = drawingRectangle.Y + button.GetTexture().Height * amount;

                button.SetPosition(new Vector2(x, y));
                amount++;
            }
        }
        #endregion

        public void Update()
        {

            buttonsToDraw.Clear();

            //Update the up and down buttons
            up.Update();
            down.Update();
            close.Update();

            if (close.Collision())
            {
                Core.currentScrollBox = null;
            }

            //If the up button is pressed and the first button in the list's position is lesser than the position of the rectangle
            if (up.Collision() && buttons[0].GetPosition().Y < drawingRectangle.Y)
            {
                foreach (var button in buttons)
                {
                    int x = (int)button.GetPosition().X;
                    int y = (int)button.GetPosition().Y + button.GetTexture().Height;
                    button.SetPosition(new Vector2(x, y));
                    button.SetCollision(new Vector2(x, y));
                }
            }
            //If the down button is pressed and the last button in the list's position is greater or equal to the haight of the rectangle
            if (down.Collision() && buttons[buttons.Count - 1].GetPosition().Y >= drawingRectangle.Y + drawingRectangle.Height)
            {
                foreach (var button in buttons)
                {
                    int test = (int)button.GetPosition().Y;
                    int x = (int)button.GetPosition().X;
                    int y = (int)button.GetPosition().Y - button.GetTexture().Height;
                    button.SetPosition(new Vector2(x, y));
                    button.SetCollision(new Vector2(x, y));
                }
            }

            //Update each button in the button list
            foreach (var button in buttons)
            {
                //Use buttonToDraw to avoid texture-flickering
                if (button.collision.Intersects(drawingRectangle))
                    buttonsToDraw.Add(button);
            }

            foreach (var button in buttonsToDraw)
            {
                button.Update();
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            up.Draw(spriteBatch);
            down.Draw(spriteBatch);
            close.Draw(spriteBatch);
            foreach (var button in buttonsToDraw)
            {
                button.Draw(spriteBatch);
            }
            //spriteBatch.Draw(Game1.BLANK_TEX, new Vector2(drawingRectangle.X, drawingRectangle.Y), drawingRectangle, Color.Red*0.5f);
        }

        public void SetButtons(List<Button> buttons)
        {
            this.buttons = buttons;
            PositionButtonsInList();
        }
    }
}
