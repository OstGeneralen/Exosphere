using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Exploring
{
    public class Waypoint
    {

        Vector2 position;
        Waypoint followingWaypoint;
        Waypoint previousWaypoint;
        Rectangle collision;
        Texture2D texture;
        public bool isHome;

        #region Save/Load

        public WaypointSave save;

        #region Load Waypoint

        public void LoadWaypoint(WaypointSave saveFile)
        {
            position = saveFile.position;
            collision = saveFile.collision;

            texture = Game1.INSTANCE.Content.Load<Texture2D>(saveFile.assetName);

            previousWaypoint = new Waypoint(saveFile.previousWaypointPosition);
            if(saveFile.nextWaypointPosition != Vector2.Zero)
                followingWaypoint = new Waypoint(saveFile.nextWaypointPosition);
        }

        #endregion

        public void SaveWaypoint()
        {
            save.collision = collision;
            save.position = position;
            save.assetName = texture.Name;
            if (previousWaypoint != null)
            {
                save.previousWaypointPosition = previousWaypoint.position;
            }
            if (followingWaypoint != null)
            {
                save.nextWaypointPosition = followingWaypoint.position;
            }
            
        }

        #endregion

        /// <summary>
        /// Creates a new waypoint
        /// </summary>
        /// <param name="position">The position of the waypoint</param>
        public Waypoint(Vector2 position, bool isHome = false)
        {
            this.position = position;
            this.isHome = isHome;

            string assetName = "Res/PH/Planet View/waypointPH";
            texture = Game1.INSTANCE.Content.Load<Texture2D>(assetName);
            texture.Name = assetName;
            collision = new Rectangle((int)(position.X + texture.Width / 2), (int)(position.Y + texture.Height / 2), 5, 5);
        }

        public void SetFollowingWaypoint(Waypoint followingWaypoint)
        {
            this.followingWaypoint = followingWaypoint;
        }

        public void SetPreviousWayPoint(Waypoint previousWaypoint)
        {
            this.previousWaypoint = previousWaypoint;
        }

        public Waypoint GetFollowingWaypoint(bool haveReachedGoal)
        {
            if (!haveReachedGoal)
                return followingWaypoint;

            return previousWaypoint;
        }

        public bool HaveReached(Rectangle explorerCollision)
        {
            if (explorerCollision.Intersects(collision))
                return true;

            return false;
        }

        public bool IsGoal()
        {
            if (followingWaypoint == null)
                return true;

            return false;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Rectangle GetCollision()
        {
            return collision;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - texture.Width/2, position.Y - texture.Height * 0.75f), Color.White);
        }
    }

    public struct WaypointSave
    {
        public Vector2 position;
        public Rectangle collision;
        public string assetName;
        public Vector2 previousWaypointPosition;
        public Vector2 nextWaypointPosition;
    }
}
