using Exosphere.Src.Basebuilding;
using Exosphere.Src.Exploring;
using Exosphere.Src.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Transferring
{

    public class TransmissionShip
    {
        Vehicle vehicle;
        Vector2 position;
        Vector2 newPosition;
        Texture2D texture;
        Rectangle collision;
        string assetName;

        #region Save/Load

        public TransmissionShipSave save;

        #region Load TransmissionShip

        public void LoadTransmissionShip(TransmissionShipSave load)
        {
            vehicle.LoadVehicle(load.vehicleSave);
            position = load.position;
            collision = load.collision;
            assetName = load.assetName;
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
        }

        #endregion

        public void SaveTransmissionShip()
        {
            vehicle.SaveVehicle();
            save.vehicleSave = vehicle.save;
            save.collision = collision;
            save.position = position;
            save.assetName = assetName;
        }

        #endregion

        public TransmissionShip(Vector2 position, Vehicle vehicle)
        {
            this.position = position;
            this.vehicle = vehicle;

            assetName = "Res/PH/Planet View/explorerMarkerPH";
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);

            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(Waypoint waypoint)
        {
            newPosition = position;

            for (int i = 0; i < vehicle.GetSpeed(); i++)
            {
                //If the X position of the waypoint is bigger than the current position's X value, raise the current X by the speed
                if (newPosition.X < waypoint.GetPosition().X)
                    newPosition.X += 1;
                //Do the opposite if the X position of the waypoint is lower instead
                else if (newPosition.X > waypoint.GetPosition().X)
                    newPosition.X -= 1;

                //Do the same thing for the Y axis
                if (newPosition.Y < waypoint.GetPosition().Y)
                    newPosition.Y += 1;
                else if (newPosition.Y > waypoint.GetPosition().Y)
                    newPosition.Y -= 1;
            }

            collision = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            position = newPosition;
        }

        public bool ReachedGoal(Waypoint waypoint)
        {
            if (collision.Intersects(waypoint.GetCollision()))
            {
                foreach (var colony in Core.GetColonies())
                {
                    if (colony.GetPlanet().GetPosition() == waypoint.GetPosition())
                    {
                        colony.GetGrid().resourceManager.clearwater += vehicle.GetStoredResources("water");
                        colony.GetGrid().resourceManager.food += vehicle.GetStoredResources("food");
                        colony.GetGrid().resourceManager.iron += vehicle.GetStoredResources("iron");
                        colony.GetGrid().resourceManager.copper += vehicle.GetStoredResources("copper");
                        colony.GetGrid().resourceManager.carbon += vehicle.GetStoredResources("carbon");

                        colony.vehicles.Add(vehicle);
                        break;
                    }
                }
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }

    public struct TransmissionShipSave
    {
        public VehicleSave vehicleSave;
        public Vector2 position;
        public Texture2D texture;
        public Rectangle collision;
        public string assetName;
    }
}
