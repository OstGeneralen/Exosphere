using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Exosphere.Src.Handlers;
using Exosphere.Src;
using Exosphere.Src.Basebuilding;
using Exosphere.Src.Generators;
using Exosphere.Src.HUD;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.ResearchProject;

namespace Exosphere
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Game1 INSTANCE = null;
        public static Texture2D BLANK_TEX;

        public static Colony colonySaveTest;

        public static bool SAVE_STATE;
        public static bool LOAD_STATE;

        public static bool DEBUG_MODE;

        private static Texture2D overlay;

        public static SaveGameData savedColonists;

        #region First time bools
        public static bool galaxyViewTutorial;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            INSTANCE = this;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            DEBUG_MODE = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.IsFullScreen = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {     
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            overlay = Content.Load<Texture2D>("Res/Overlay");

            BLANK_TEX = Content.Load<Texture2D>("Res/blankTex");

            TimeHandler.CreateTime();

            Cursor.CreateCursor();

            Settings.SetValues();

            Core.CreateCore();

            HUD.CreateHud();

            //TODO: Remove comments to make the game load when the game is started
            /*
            IAsyncResult tempResult;
            tempResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            StorageDevice device = StorageDevice.EndShowSelector(tempResult);
            if(LoadGame(device) == 1)
            {
                galaxyViewTutorial = true;
            }
            */
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        IAsyncResult tempResult;
        protected override void Update(GameTime gameTime)
        {
            tempResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            // Allows the game to exit
            if (KeyboardHandler.PressedOnce(Keys.Escape))
            {
                Quit();
            }
            if(KeyboardHandler.PressedOnce(Keys.S))
            {
                StorageDevice device = StorageDevice.EndShowSelector(tempResult);
                SaveGame(device);
            }
            if(KeyboardHandler.PressedOnce(Keys.L))
            {
                StorageDevice device = StorageDevice.EndShowSelector(tempResult);
                LoadGame(device);
            }

            base.Update(gameTime);

            // TODO: Add your update logic here
            KeyboardHandler.Update();
            
            MouseHandler.Update();
            
            Cursor.Update();

            HUD.Update();

            Core.Update();      
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Core.Draw(spriteBatch);

            #region Draw cursor
            spriteBatch.Begin();

            HUD.Draw(spriteBatch);
            if(Core.currentScreen.GetScreen() == "Planet Screen")
            PlanetHUD.Draw(spriteBatch);
            if (Core.currentScrollBox != null)
                Core.currentScrollBox.Draw(spriteBatch);
            if(Core.currentMessageBox != null)
                Core.currentMessageBox.Draw(spriteBatch);
            Cursor.Draw(spriteBatch);
            spriteBatch.Draw(overlay, Vector2.Zero, Color.White * 0.15f);

            spriteBatch.End();
            #endregion
            
            base.Draw(gameTime);
        }

        public void Quit()
        {
                Exit();
        }

        public struct SaveGameData
        {
            public TimeHandlerSave timeSave;
            public GalaxySave galaxySave;
            public CameraSave cameraSave;
            public TutorialsSave tutorialsSave;
            //public ColonySave colonySave;
        }

        private static void SaveGame(StorageDevice device)
        {
            SaveGameData data = new SaveGameData();


            TimeHandler.SaveTime();
            data.timeSave = TimeHandler.save;

            Core.galaxyScreen.galaxy.SaveGalaxy();
            data.galaxySave = Core.galaxyScreen.galaxy.save;

            Camera.SaveCamera();
            data.cameraSave = Camera.save;

            Core.SaveTutorials();
            data.tutorialsSave = Core.save;

            IAsyncResult result = device.BeginOpenContainer("Exosphere Saves", null, null);

            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            result.AsyncWaitHandle.Close();

            string filename = "exosave.sav";

            if (container.FileExists(filename))
                container.DeleteFile(filename);

            Stream stream = container.CreateFile(filename);

            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, data);

            

            stream.Close();

            container.Dispose();
        }

        private static int LoadGame(StorageDevice device)
        {
            IAsyncResult result = device.BeginOpenContainer("Exosphere Saves", null, null);

            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            result.AsyncWaitHandle.Close();

            string filename = "exosave.sav";

            if(!container.FileExists(filename))
            {
                container.Dispose();
                return 1;
            }

            Stream stream = container.OpenFile(filename, FileMode.Open);

            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            SaveGameData data = (SaveGameData)serializer.Deserialize(stream);

            //TimeHandler.CreateTime(data.timeSave);

            stream.Close();

            Core.galaxyScreen.galaxy.LoadGalaxy(data.galaxySave);
            Camera.LoadCamera(data.cameraSave);
            Core.LoadTutorials(data.tutorialsSave);

            container.Dispose();


            savedColonists = data;

            return 0;
        }
      
    }
}
