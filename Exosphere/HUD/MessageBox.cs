using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    abstract class TextBox
    {
        private string measureString;
        protected string message;
        protected SpriteFont font;
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 stringPosition;
        private Texture2D buttonTexture;
        protected string buttonAssetName;
        protected bool hasButton;

        protected string[] strings;


        public TextBox(int size, string message)
        {
            this.message = message;

            hasButton = true;

            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");

            buttonAssetName = "Res/PH/HUD/Buttons/Standard/ButtonUpPH";

            buttonTexture = Game1.INSTANCE.Content.Load<Texture2D>(buttonAssetName);

            if (size == 0)
            {
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Message Boxes/MessageBoxMinimal");
            }
            if (size == 1)
            {
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Message Boxes/MessageBoxSmall");
            }

            if (size == 2)
            {
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Message Boxes/MessageBoxMedium");
            }

            if (size == 3)
            {
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Message Boxes/MessageBox");
            }


            strings = new string[message.Split('\n').Length];

            position = CenterMessageBox();
            message = FormMessage();
            CenterMessage();

        }

        protected Vector2 CenterMessageBox()
        {
            int x = 0;
            int y = 0;

            x = (int)((Settings.GetScreenRes.X / 2) - (texture.Width / 2));
            y = (int)((Settings.GetScreenRes.Y / 2) - (texture.Height / 2));

            return new Vector2(x, y);
        }

        protected void CenterMessage()
        {
            int x = 0;
            int y = 0;

            if (hasButton)
            {
                x = (int)(position.X + (texture.Width / 2) - (font.MeasureString(message).X / 2) - (font.MeasureString(" ").X) / 2);
                y = (int)(position.Y + (texture.Height / 2) - (font.MeasureString(message).Y / 2) - (buttonTexture.Height) + (font.MeasureString("\n").Y / 2));
            }
            else if (!hasButton)
            {
                x = (int)(position.X + (texture.Width / 2) - (font.MeasureString(message).X / 2) - (font.MeasureString(" ").X) / 2);
                y = (int)(position.Y + (texture.Height / 2) - (font.MeasureString(message).Y / 2));
            }

            stringPosition = new Vector2(x, y);
        }

        /// <summary>
        /// Implements a messagebox containing a message
        /// </summary>
        /// <param name="size">The size of the messagebox("Big", "Small")</param>
        private string FormMessage()
        {

            int zero = 0;
            int distanceFromEdge = 150;
            int rowLength = texture.Width - distanceFromEdge;

            //Saves the original message to measureString, so it can be used to measure where to break the message apart

            message = message.Insert(zero, "");

            strings = message.Split('\n');
            message = "";

            for (int y = 0; y < strings.Length; y++)
            {

                //Breaks message apart
                if (font.MeasureString(strings[y]).X > rowLength)
                {

                    measureString = strings[y];
                    strings[y] = strings[y].Insert(strings[y].Length, " ");

                    for (int i = 1; i <= (int)(font.MeasureString(measureString).X / rowLength); i++)
                    {

                        //Inserts \n when the message goes outside the messagebox
                        strings[y] = strings[y].Insert(strings[y].IndexOf(" ",
                            (int)(((float)(strings[y].Length) / font.MeasureString(measureString).X) * (float)(rowLength) * i)), "\n");


                    }

                }
                message = message.Insert(message.Length, "\n " + strings[y]);
            }

            return message;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.DrawString(font, message, stringPosition, new Color(204, 200, 80));
        }
    }

    class MessageBox : TextBox
    {

        Button okay;

        /// <summary>
        /// Creates a new message box
        /// </summary>
        /// <param name="size">The size of the message box. 1 Small, 2 Medium, 3 Big</param>
        /// <param name="message">The message that should be written</param>
        public MessageBox(int size, string message)
            : base(size, message)
        {

            okay = new Button(buttonAssetName, Vector2.Zero, "Okay", 0, true);

            okay.SetPosition(SetOkayButtonPosition());
        }

        public Vector2 SetOkayButtonPosition()
        {
            int x = 0;
            int y = 0;

            x = (int)(position.X + (texture.Width / 2) - (okay.GetTexture().Width / 2));
            y = (int)((position.Y + texture.Height) - (okay.GetTexture().Height + 8));

            return new Vector2(x, y);
        }

        public override void Update()
        {
            okay.Update();

            if (okay.Collision())
                Core.currentMessageBox = null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            okay.Draw(spriteBatch);
        }
    }

    class ChoiceBox : TextBox
    {

        Button yes;
        Button no;

        public ChoiceBox(int size, string message)
            : base(size, message)
        {
            //Creates two new buttons labeled yes and no
            yes = new Button(buttonAssetName, Vector2.Zero, "Yes");
            no = new Button(buttonAssetName, Vector2.Zero, "No");

            yes.SetPosition(PositionButtons("Yes"));
            no.SetPosition(PositionButtons("No"));
        }

        /// <summary>
        /// Positions the buttons
        /// </summary>
        /// <param name="label">The label of the button</param>
        /// <returns>A vector2 position</returns>
        public Vector2 PositionButtons(string label)
        {
            int x = 0;
            int y = 0;

            y = (int)((position.Y + texture.Height) - (yes.GetTexture().Height + 8));

            if (label == "Yes")
            {
                x = (int)(position.X + (texture.Width * 0.75f) - (yes.GetTexture().Width / 2));
            }


            if (label == "No")
            {
                x = (int)(position.X + (texture.Width * 0.25f) - (yes.GetTexture().Width / 2));
            }

            return new Vector2(x, y);
        }

        public override void Update()
        {
            if (Collision() == 1)
            {
                Core.choiceBoxChoice = true;
            }
            else if (Collision() == 2)
            {
                Core.choiceBoxChoice = false;
            }
            base.Update();
        }

        /// <summary>
        /// Gets the choice the player makes in the choice box
        /// </summary>
        /// <returns>0 means no desicion is made, 1 means yes and 2 means no</returns>
        private int Collision()
        {
            yes.Update();
            no.Update();

            if (yes.Collision())
            {
                Remove();
                return 1;
            }

            else if (no.Collision())
            {
                Remove();
                return 2;
            }

            return 0;
        }

        private void Remove()
        {
            Core.currentMessageBox = null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            yes.Draw(spriteBatch);
            no.Draw(spriteBatch);
        }
    }

    class InfoBox : TextBox
    {
        Button button;

        /// <summary>
        /// Creates a new info box
        /// </summary>
        /// <param name="size">The size of the message box 1. Small, 2. Medium, 3. Large</param>
        /// <param name="message">The message that should be written in the message box</param>
        /// <param name="button">A button if the info box should have one (optional)</param>
        public InfoBox(int size, string message, Button button = null)
            : base(size, message)
        {
            this.button = button;

            if (button == null)
                hasButton = false;

            CenterMessage();

            if (this.button != null)
                PositionButton();
        }

        public void OverridePosition(Vector2 newPosition)
        {
            position = newPosition;

            CenterMessage();

            if (button != null)
                PositionButton();
        }

        public void OverrideMessage(string newMessage)
        {
            message = newMessage;
        }

        private void PositionButton()
        {
            int x = 0;
            int y = 0;

            x = (int)(position.X + (texture.Width / 2) - (button.GetTexture().Width / 2));
            y = (int)((position.Y + texture.Height) - (button.GetTexture().Height + 8));

            button.SetPosition(new Vector2(x, y));
        }

        public Button GetButton()
        {
            return button;
        }

        public override void Update()
        {
            base.Update();
            if (button != null)
                button.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (button != null)
                button.Draw(spriteBatch);
        }
    }
}
