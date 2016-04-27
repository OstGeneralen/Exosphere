using Exosphere.Src.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src
{
    public abstract class BaseObject
    {
        #region Variables

        //Represents the position in pixels of the object
        protected Vector2 position;
        //Represents the object's collisionrectangle
        public Rectangle collision;
        //Represents the graphic-file of the object
        protected Texture2D texture;

        protected Color color;
        protected int layer;

        protected string label;
        protected bool hasLabel;
        Vector2 labelPosition;

        bool isGalaxyException;

        protected SpriteFont font;

        #endregion

        /// <summary>
        /// Implements the object in the project
        /// </summary>
        /// <param name="assetName">The assetName of the graphic-file that represents the object</param>
        /// <param name="position">The starting position of the object</param>
        public BaseObject(string assetName, Vector2 position, int layer = 0, bool isGalaxyException = false)
        {
            this.isGalaxyException = isGalaxyException;
            //Saves the texture of the file's "assetName" = users input
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
            //Sets the position value to your input in the constructor
            this.position = position;
            //Calculates the collisionrectangle's position, width and height
            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            this.layer = layer;

            color = Color.White;
        }

        public BaseObject(string assetName, Vector2 position, string label, int layer = 0, bool isGalaxyException = false)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            this.isGalaxyException = isGalaxyException;

            this.position = position;

            color = Color.White;

            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            this.label = label;

            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");

            hasLabel = true;

            this.layer = layer;
        }

        /// <summary>
        /// Implements the object in the project
        /// </summary>
        /// <param name="position">The starting position of the object</param>
        public BaseObject(Vector2 position, int layer = 0, bool isGalaxyException = false)
        {
            this.isGalaxyException = isGalaxyException;
            this.position = position;

            this.layer = layer;
            color = Color.White;
        }

        #region Temp Region(Hides Stuff)

        public void SetPosition(Vector2 position)
        {
            this.position = position;
            if (hasLabel)
            {
                labelPosition = new Vector2(
                    position.X + (texture.Width / 2) - (font.MeasureString(label).X / 2),
                    position.Y + (texture.Height / 2) - (font.MeasureString(label).Y / 2));
            }
        }

        public int GetLayer()
        {
            return layer;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Implements the object in the project
        /// </summary>
        public BaseObject()
        {

        }

        /// <summary>
        /// Updates the object
        /// </summary>
        public virtual void Update()
        {
            //Updates the object's collisionrectangle
            if (texture != null)
            {
                //Updates the collisionrectangle's position, width and height
                if (!isGalaxyException)
                    collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

                if (isGalaxyException && Core.currentScreen != Core.galaxyScreen)
                    collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

                if (isGalaxyException && Core.currentScreen == Core.galaxyScreen)
                    collision = new Rectangle((int)(position.X - Camera.position.X), (int)(position.Y - Camera.position.Y), texture.Width, texture.Height);

                if (hasLabel)
                {
                    labelPosition = new Vector2(
                        position.X + (texture.Width / 2) - (font.MeasureString(label).X / 2),
                        position.Y + (texture.Height / 2) - (font.MeasureString(label).Y / 2));
                }
            }
        }

        /// <summary>
        /// Use to override the standard collision code for position of the collision rectangle
        /// </summary>
        public void SetCollision(Vector2 newPosition)
        {
            collision.X = (int)newPosition.X;
            collision.Y = (int)newPosition.Y;
        }

        /// <summary>
        /// Draws the object's texture on the screen, on a specified position
        /// </summary>
        /// <param name="spriteBatch">Used to draw stuff on screen</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //Draws the object
            spriteBatch.Draw(texture, position, color);
            if (font != null)
            {
                spriteBatch.DrawString(font, label, labelPosition, Color.Black);
            }
        }

        /// <summary>
        /// Checks if the object collides with the cursor when you click the left mousebutton
        /// </summary>
        /// <param name="cursor">The cursor you check collision against</param>
        /// <returns>Returns true if the object collides with the cursor(else false)</returns>
        public virtual bool Collision()
        {
            if (collision.Intersects(Cursor.collision) && MouseHandler.LMBOnce())
                return true;
            return false;
        }

        /// <summary>
        /// Checks if an object collides with another object
        /// </summary>
        /// <param name="button">The object/button you check collision against</param>
        /// <returns>Returns true if they collide, returns false if they don't</returns>
        public virtual bool Collision(Button button)
        {
            if (collision.Intersects(button.collision))
                return true;
            return false;
        }

        #endregion
    }

    public class Button : BaseObject
    {
        /// <summary>
        /// Implements a button in the project
        /// </summary>
        /// <param name="assetName">The assetName of the graphic-file that represents the button</param>
        /// <param name="position">The starting position of the object</param>
        public Button(string assetName, Vector2 position, int layer = 0, bool isGalaxyException = false)
            : base(assetName, position, layer, isGalaxyException)
        {

        }

        /// <summary>
        /// Implements a button in the project
        /// </summary>
        /// <param name="position">The starting position of the object</param>
        public Button(Vector2 position, int layer = 0, bool isGalaxyException = false)
            : base(position, layer, isGalaxyException)
        {

        }

        /// <summary>
        /// Implements a button in the project
        /// </summary>
        public Button()
        {

        }

        public Button(string assetName, Vector2 position, string label, int layer = 0, bool isGalaxyException = false)
            : base(assetName, position, label, layer, isGalaxyException)
        {

        }

        public override bool Collision()
        {
            return base.Collision();
        }

        public override bool Collision(Button button)
        {
            return base.Collision(button);
        }
    }

    public class ActivatableButton : Button
    {
        protected bool activation;

        /// <summary>
        /// Implements a button in the project
        /// </summary>
        /// <param name="assetName">The assetName of the graphic-file that represents the button</param>
        /// <param name="position">The starting position of the object</param>
        public ActivatableButton(string assetName, Vector2 position, int layer = 0, bool isGalaxyException = false)
            : base(assetName, position, layer, isGalaxyException)
        {

        }

        /// <summary>
        /// Implements a button in the project
        /// </summary>
        /// <param name="position">The starting position of the object</param>
        public ActivatableButton(Vector2 position, int layer = 0, bool isGalaxyException = false)
            : base(position, layer, isGalaxyException)
        {

        }

        /// <summary>
        /// Implements a button in the project
        /// </summary>
        public ActivatableButton()
        {

        }

        public ActivatableButton(string assetName, Vector2 position, string label, int layer = 0, bool isGalaxyException = false)
            : base(assetName, position, label, layer, isGalaxyException)
        {

        }

        public virtual void ChangeColor()
        {
            if (collision.Intersects(Cursor.collision) && MouseHandler.LMBOnce() && !activation)
                activation = true;
            else if (collision.Intersects(Cursor.collision) && MouseHandler.LMBOnce() && activation)
                activation = false;
        }

        public bool GetActivation()
        {
            return activation;
        }

        public override void Update()
        {
            if (activation)
            {
                color = Color.Blue;
            }
            else
            {
                color = Color.White;
            }
            base.Update();
        }
    }
}
