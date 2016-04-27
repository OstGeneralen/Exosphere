using Exosphere.Src.Basebuilding;
using Exosphere.Src.Basebuilding.Facilities;
using Exosphere.Src.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.HUD
{
    class FacilityChoice : Button
    {
        //The name of the facility
        string facilityName;

        //The texture that the button will have when pressed
        Texture2D pressedTexture;

        //The texture the buttun will have when released
        Texture2D releasedTexture;

        FacilityChoice facilityChoice;

        Facility facility;

        /// <summary>
        /// Creates a new Facility Choice Button
        /// </summary>
        /// <param name="releasedAssetName">The asset name of the released/up button</param>
        /// <param name="pressedAssetName">The asset name of the pressed button</param>
        /// <param name="position">The position of the button</param>
        /// <param name="facilityName">The name of the facility this button should represent</param>
        public FacilityChoice(string releasedAssetName, string pressedAssetName, Vector2 position, Facility facility)
            : base(releasedAssetName, position, facility.GetFacilityType())
        {
            //Sets the facilityname to equal the inputed facility name
            this.facilityName = facility.GetFacilityType();

            this.facility = facility;

            //Loads the font into the SpriteFont variable
            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");

            //Sets the released texture
            releasedTexture = texture;

            //Sets the pressed texture
            pressedTexture = Game1.INSTANCE.Content.Load<Texture2D>(pressedAssetName);

            facilityChoice = null;
        }

        public FacilityChoice GetFacilityChoice()
        {
            return facilityChoice;
        }

        /// <summary>
        /// The dimensions (width and height) of the button's texture
        /// </summary>
        /// <returns>Returns a vector2 with the dimensions</returns>
        public Vector2 GetTextureDimension()
        {
            return new Vector2(texture.Width, texture.Height);
        }

        /// <summary>
        /// Updates the Facility Choice button
        /// </summary>
        public override void Update()
        {
            //Runs the base update
            base.Update();

            if(Cursor.collision.Intersects(collision) && Core.currentMessageBox == null)
            {
                string message = "";

                

                message = message.Insert(message.Length, facilityName + "\n\n");
                message = message.Insert(message.Length, "Required Copper: " + facility.GetCostCopper().ToString() + "\n");
                message = message.Insert(message.Length, "Required Iron: " + facility.GetCostIron().ToString() + "\n");
                message = message.Insert(message.Length, "Required Carbon: " + facility.GetCostCarbon().ToString() + "\n");
                message = message.Insert(message.Length, "Requried Energy: " + facility.GetCostEnergy().ToString() + "\n");


                InfoBox ib = new InfoBox(0, message);
                Core.currentMessageBox = ib;
            }

            //If the facility is not the one currently in line to be built the texture is reset to the released texture
            if (ColonyScreen.facilityHold != facilityName)
                texture = releasedTexture;

            //If the button is pressed the global facilityHold will be updated to the facility name of the button and the texture will change
            if (Collision())
            {
                ColonyScreen.facilityHold = facilityName;
                facilityChoice = this;
                texture = pressedTexture;

                #region Facility Hold
                switch (ColonyScreen.facilityHold)
                {
                    case "Lab":
                        if (Core.showLabInfo)
                        {
                            Core.showLabInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Lab \n \n");

                            message = message.Insert(message.Length, "The Lab enables research possibilities within many different categories that either facilitates normal tasks or permits new actions that were previously impossible. \n \n");
                            message = message.Insert(message.Length, "This makes the Lab the foundation of all development from a general perspective. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "Gym":
                        if (Core.showGymInfo)
                        {
                            Core.showGymInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Gym \n \n");
                            message = message.Insert(message.Length, "The Gym enables the possibility to increase the inhabitants strength by placing them under harsh training. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "ComArray":
                        if (Core.showComArrayInfo)
                        {
                            Core.showComArrayInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Com Array \n \n");
                            message = message.Insert(message.Length, "The Com Array enables long distance communication by sending and decrypting hyper-waves between your bases. \n \n");
                            message = message.Insert(message.Length, "Observe: This means both bases need to have a functional com array available since you need both a transmitter and a receiver. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "EquipmentStores":
                        if (Core.showEquipmentStoresInfo)
                        {
                            Core.showEquipmentStoresInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Equipment Stores \n \n");
                            message = message.Insert(message.Length, "The Equipment Stores allows storing of manufactured items. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "Evaluation":
                        if (Core.showEvaluationInfo)
                        {
                            Core.showEvaluationInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Evaluation Facility \n \n");
                            message = message.Insert(message.Length, "The Evaluation Facility provides you with thorough information regarding the colonists statistics. \n \n");
                            message = message.Insert(message.Length, "The given intel depends on the facility's level. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "FoodStores":
                        if (Core.showFoodStoresInfo)
                        {
                            Core.showFoodStoresInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Food Stores \n \n");
                            message = message.Insert(message.Length, "The Food Stores expands the colony's base storage of 500 food and water units by 2500 units. \n \n");
                            message = message.Insert(message.Length, "Note that the supernumerary resources will be tossed out if the amount of food or water exceeds the colony's total storage limit. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "Garage":
                        if (Core.showGarageInfo)
                        {
                            Core.showGarageInfo = false;

                            string message = "";

                            message = message.Insert(message.Length, "Garage \n \n");
                            message = message.Insert(message.Length, "The garage stores, maintains and refuels a number of the colony's land vehicles based on its level. ");

                            MessageBox guide = new MessageBox(3, message);
                            Core.currentMessageBox = guide;
                        }
                        break;
                    case "Greenhouse":
                        if (Core.showGreenhouseInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Greenhouse \n \n");
                            message = message.Insert(message.Length, "The Greenhouse allows the conversion of water units to food units. This is as the water is used to grow plants that can be eaten. It will generate 1 food unit per 10 water units. \n");
                            message = message.Insert(message.Length, "However the energy demand is high due to the Greenhouse constantly needing to artificially project light on the plants in it.");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showGreenhouseInfo = false;
                        }
                        break;
                    case "Hangar":
                        if (Core.showHangarInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Hangar \n \n");
                            message = message.Insert(message.Length, "The hangar allows storing of ships. The size of the ships it can store depends on its level and while a low-leveled hangar may store only small ships and high-level hangar may store ships of all sizes. ");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showHangarInfo = false;
                        }
                        break;
                    case "Library":
                        if (Core.showLibraryInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Library \n \n");
                            message = message.Insert(message.Length, "The library enables the possibility to increase the inhabitants intelligence. \n");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showLibraryInfo = false;
                        }
                        break;
                    case "LivingQuarters":
                        if (Core.showLivingQuartersInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Living Quarters \n \n");
                            message = message.Insert(message.Length, "The living quarters will serve as each colonist's home. Here they can get their well deserved rest after a hard day of work. ");
                            message = message.Insert(message.Length, "Without a living quarter the colonists will die due to there being no space for them to live. ");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showLivingQuartersInfo = false;
                        }
                        break;
                    case "MedBay":
                        if (Core.showMedBayInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Med-Bay \n \n");
                            message = message.Insert(message.Length, "The Med-Bay will serve as each colony's infirmary. Here colonists who are hurt can regain their health and in the case of ");
                            message = message.Insert(message.Length, "lethal diseases the vaccination is done in the Med-Bay as well. \n \n");
                            message = message.Insert(message.Length, "Note: Before a vaccination is possible the vaccine must be researched in the lab. ");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showMedBayInfo = false;
                        }
                        break;
                    case "Mine":
                        if (Core.showMineInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Mine \n \n");
                            message = message.Insert(message.Length, "The mine allows the colony to mine for minerals which can be used to build and maintain facilities. \n \n");
                            message = message.Insert(message.Length, "Note: The mine will only search for minerals in specified 'mining areas'. These can be placed and activated through the mine's task screen. ");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showMineInfo = false;
                        }
                        break;
                    case "MineralStores":
                        if (Core.showMineralStoresInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Mineral Stores \n \n");
                            message = message.Insert(message.Length, "The mineral stores allows for mined minerals to be stored. Unless at least one mineral store is built, the colony will use ");
                            message = message.Insert(message.Length, "its basic storage of 300 units/mineral-type and after that throw the rest away.");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showMineralStoresInfo = false;
                        }
                        break;
                    case "SolarPanels":
                        if (Core.showSolarPanelsInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Solar Panels \n \n");
                            message = message.Insert(message.Length, "The solar panels absorbs the solar energy of nearby stars and converts it to electric power. The electricity can from there on ");
                            message = message.Insert(message.Length, "be used to power entire colonies and all of its facilities. \n \n");
                            message = message.Insert(message.Length, "Note that the electric power generated by the solar panels is constant and will not be drained or increased each day. ");
                            message = message.Insert(message.Length, "When a new facility is built it will convert some of the 'free energy' to 'used energy'. ");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showSolarPanelsInfo = false;
                        }
                        break;
                    case "Workshop":
                        if (Core.showWorkshopInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Workshop \n \n");
                            message = message.Insert(message.Length, "The workshop allows its workers to assemble the tools needed to get through the every day life of a colonist. \n \n");
                            message = message.Insert(message.Length, "With products varying from basic equipment to ships it can provide an entire galaxy with the goods its inhabitants need. ");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showWorkshopInfo = false;
                        }
                        break;
                    case "Pump":
                        if (Core.showPumpInfo)
                        {
                            MessageBox mb;
                            string message = "";

                            message = message.Insert(message.Length, "Pump \n \n");
                            message = message.Insert(message.Length, "The pump provides clear water to the colony by pumping up the subsoil water from the planet's foundation. \n \n");
                            message = message.Insert(message.Length, "As a low level pump may not be very efficient as it is upgraded its reach down into the planetary shell will extend and the amount of water it provides will increase greatly. \n");

                            mb = new MessageBox(3, message);
                            Core.currentMessageBox = mb;

                            Core.showPumpInfo = false;

                        }
                        break;
                    default:
                        break;
                }
                #endregion
            }
        }

        /// <summary>
        /// Draws the Facility Choice Button
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch used to draw</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Runs the base draw
            base.Draw(spriteBatch);
        }
    }

    class FacilityChoiceCategory : Button
    {

        List<FacilityChoice> facilityChoices;
        public bool showFacilityChoices;
        FacilityChoice facilityChoice;

        public FacilityChoiceCategory(string category, Vector2 position)
            : base("Res/PH/HUD/Buttons/Standard/ButtonUpPH", position, category)
        {

            font = Game1.INSTANCE.Content.Load<SpriteFont>("Res/Fonts/Message");
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/Standard/ButtonUpPH");
            facilityChoices = new List<FacilityChoice>();

            #region Categories
            if (category == "Misc")
            {
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new SolarPanels(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Pump(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new LivingQuarters(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new ComArray(Vector2.Zero, null, 0)));
            }

            if (category == "Workplaces")
            {
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Workshop(Vector2.Zero, null, 0)));
                //facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), "Refinery"));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Mine(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Lab(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Hangar(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Greenhouse(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Garage(Vector2.Zero, null, 0)));
            }

            if (category == "Training and Stats")
            {
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new MedBay(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Library(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Gym(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new Evaluation(Vector2.Zero, null, 0)));
            }

            if (category == "Stores")
            {
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new MineralStores(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new FoodStores(Vector2.Zero, null, 0)));
                facilityChoices.Add(new FacilityChoice("Res/PH/HUD/Buttons/Standard/ButtonUpPH", "Res/PH/HUD/Buttons/Standard/ButtonDownPH", positionChoice(), new EquipmentStores(Vector2.Zero, null, 0)));
            }
            #endregion

            facilityChoice = null;
            showFacilityChoices = false;
        }

        public override void Update()
        {
            base.Update();

            if (Collision() && !showFacilityChoices)
            {
                showFacilityChoices = true;
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/Standard/ButtonDownPH");
            }
            else if (Collision() && showFacilityChoices)
            {
                showFacilityChoices = false;
            }

            if (!showFacilityChoices)
            {
                texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/HUD/Buttons/Standard/ButtonUpPH");
            }

            if (showFacilityChoices)
            {
                FacilityChoiceUpdate();
            }
        }

        public Vector2 positionChoice()
        {
            position = new Vector2(
                ActionButtons.buildButton.GetTexture().Width + texture.Width,
                ActionButtons.buildButton.GetPosition().Y - (texture.Height * facilityChoices.Count));

            int i = (int)((ActionButtons.buildButton.GetPosition().Y + texture.Height) / texture.Height);

            if (position.Y < 0)
            {
                position.X += texture.Width;
                position.Y = ActionButtons.buildButton.GetPosition().Y - (texture.Height * (facilityChoices.Count - i));
            }

            return position;
        }

        public List<FacilityChoice> GetFacilityChoices()
        {
            return facilityChoices;
        }

        public FacilityChoice GetFacilitychoise()
        {
            return facilityChoice;
        }

        public void FacilityChoiceUpdate()
        {
            foreach (FacilityChoice fc in facilityChoices)
            {
                if (fc.GetFacilityChoice() != null)
                {
                    facilityChoice = fc.GetFacilityChoice();
                }
                fc.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);

            //spriteBatch.DrawString(font, text, position, Color.Black);

            if (showFacilityChoices)
            {
                foreach (FacilityChoice fc in facilityChoices)
                {
                    fc.Draw(spriteBatch);
                }
            }
        }

    }
}
