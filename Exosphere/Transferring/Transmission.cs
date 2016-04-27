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

    public class Transmission
    {
        Waypoint waypoint;
        TransmissionShip transmissionShip;

        #region Save/Load

        public TransmissionSave save;

        #region Load Transmission

        public void LoadTransmission(TransmissionSave saveFile)
        {
            waypoint.LoadWaypoint(saveFile.waypointSave);
            transmissionShip.LoadTransmissionShip(saveFile.transmissionShipSave);
        }

        #endregion

        public void SaveTransmission()
        {
            waypoint.SaveWaypoint();
            save.waypointSave = waypoint.save;

            transmissionShip.SaveTransmissionShip();
            save.transmissionShipSave = transmissionShip.save;
        }

        #endregion

        public Transmission(Vector2 goal, Vector2 currentPosition, Vehicle vehicle)
        {
            waypoint = new Waypoint(goal);
            transmissionShip = new TransmissionShip(currentPosition, vehicle);
        }

        public void Movement()
        {
            transmissionShip.Update(waypoint);
        }

        public bool ReachedGoal()
        {
            if (transmissionShip.ReachedGoal(waypoint))
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            transmissionShip.Draw(spriteBatch);
            waypoint.Draw(spriteBatch);
        }
    }

    public struct TransmissionSave
    {
        public TransmissionShipSave transmissionShipSave;

        public WaypointSave waypointSave;
    }
}

