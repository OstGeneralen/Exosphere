using Exosphere.Src.Exploring;
using Exosphere.Src.Handlers;
using Exosphere.Src.Items;
using Exosphere.Src.Items.Vehicles;
using Exosphere.Src.Transferring;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Basebuilding.Facilities
{
    class Hangar : Facility
    {
        #region Variables

        #region Transferring

        int foodTransferAmount;
        int waterTransferAmount;
        int carbonTransferAmount;
        int copperTransferAmount;
        int ironTransferAmount;

        #endregion

        #region Vehicle Storage Limit

        //The amount of small vehicles the hangar can store
        int smallVehicleLimit;

        //The amount of normal vehicles the hangar can store
        int normalVehicleLimit;

        //The amount of large vehicles the hangar can store
        int largeVehicleLimit;

        //The amount of huge vehicles the hangar can store
        int hugeVehicleLimit;

        #endregion

        #region Vehicles Stored

        //The amount of small vehicles the hangar stores
        int smallVehicleAmount;

        //The amount of normal vehicles the hangar stores
        int normalVehicleAmount;

        //The amount of large vehicles the hangar stores
        int largeVehicleAmount;

        //The amount of huge vehicles the hangar stores
        int hugeVehicleAmount;

        #endregion

        #region Launch Information

        //The list containing the hangar's stored vehicles
        public List<Vehicle> vehicles;

        //The colony your currently transferring resources to
        Colony currentColony;

        //The vehicle your sending resources with
        Vehicle currentVehicle;

        //Checks if you have confirmed a shipment or not
        bool launch;

        #endregion

        #endregion

        #region Load/Save
        public override void LoadFacility(HangarSave load)
        {

            List<string> vehicleTypes;
            vehicles = new List<Vehicle>();

            this.position = load.position;

            this.colonyID = load.colonyID;

            this.level = load.level;

            this.storageLevel = load.storageLevel;

            this.housingLimit = load.housingLimit;

            this.finished = load.finished;

            this.timeUnderConstruction = load.timeUnderConstruction;

            vehicleTypes = load.vehicles;

            foreach (var vehicle in vehicleTypes)
            {
                if (vehicle == "Vehicle C60")
                    vehicles.Add(new VehicleC60());
            }

            this.ID = load.ID;

            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/HangarPH");

            for (int i = 0; i < colony.inhabitants.Count; i++)
            {
                if (colony.inhabitants[i].occupied && colony.inhabitants[i].facilityID == ID)
                {
                    workers.Add(colony.inhabitants[i]);
                    colony.inhabitants[i].workPlace = this;
                }
            }
        }

        public override void SaveFacility()
        {

            List<string> vehicleTypes = new List<string>();
            foreach (var v in vehicles)
            {
                vehicleTypes.Add(v.GetItemType());
            }

            hangarSave.position = position;
            hangarSave.colonyID = colonyID;
            hangarSave.level = level;
            hangarSave.storageLevel = storageLevel;
            hangarSave.housingLimit = housingLimit;
            hangarSave.finished = finished;
            hangarSave.timeUnderConstruction = timeUnderConstruction;
            hangarSave.vehicles = vehicleTypes;
            hangarSave.ID = ID;
        }
        #endregion

        /// <summary>
        /// Constucts the hangar
        /// </summary>
        /// <param name="position">The facility's grid position in pixels</param>
        /// <param name="colony">The colony the facility resides in</param>
        public Hangar(Vector2 position, Colony colony, int ID)
            : base(position, colony, ID)
        {
            texture = Game1.INSTANCE.Content.Load<Texture2D>("Res/PH/Colony View/Facilities/HangarPH");

            facilityType = "Hangar";

            vehicles = new List<Vehicle>();

            vehicles.Add(new VehicleC60());
            vehicles.Add(new VehicleC60());

            finished = true;

            level = 1;

            smallVehicleLimit = 8;
            normalVehicleLimit = 4;
            largeVehicleLimit = 2;
            hugeVehicleLimit = 1;

            costCopper = 500;
            costIron = 400;
            costCarbon = 50;

            copperValue = 3.75f;
            carbonValue = 2.75f;
            ironValue = 4;

            requiredEnergy = 3000;

            maxLevel = 3;

            CreateFacilityButton();

            if(ID > 0)
                taskScreen = new HangarTaskScreen(colony, this);

            SetDebugValues();
        }

        /// <summary>
        /// Performs the actions made by the hangar
        /// </summary>
        /// <param name="value">Should contain ""</param>
        public override void Task(string value)
        {
            StoreVehicle();

            Launch();
        }

        #region Store Vehicles

        /// <summary>
        /// Stores the colony's vehicles in the hangar
        /// </summary>
        private void StoreVehicle()
        {
            VehicleAmount();

            List<Vehicle> removeVehicle = new List<Vehicle>();

            foreach (var vehicle in colony.vehicles)
            {
                switch (vehicle.GetVehicleSize())
                {
                    case "Small":
                        if (smallVehicleAmount < GetVehicleStorageLimit("Small"))
                        {
                            vehicles.Add(vehicle);
                            removeVehicle.Add(vehicle);
                        }
                        break;
                    case "Normal":
                        if (normalVehicleAmount < GetVehicleStorageLimit("Normal"))
                        {
                            vehicles.Add(vehicle);
                            removeVehicle.Add(vehicle);
                        }
                        break;
                    case "Large":
                        if (largeVehicleAmount < GetVehicleStorageLimit("Large"))
                        {
                            vehicles.Add(vehicle);
                            removeVehicle.Add(vehicle);
                        }
                        break;
                    case "Huge":
                        if (hugeVehicleAmount < GetVehicleStorageLimit("Huge"))
                        {
                            vehicles.Add(vehicle);
                            removeVehicle.Add(vehicle);
                        }
                        break;
                    default: throw new Exception("This is not a valid vehicle size");
                }
            }

            foreach (var vehicle in removeVehicle)
            {
                colony.vehicles.Remove(vehicle);
            }
        }

        /// <summary>
        /// Gets the hangar's storage limit for a specific vehicle size
        /// </summary>
        /// <param name="vehicleSize">The vehicle size("Small", "Normal", "Large" or "Huge")</param>
        /// <returns>Returns the storage limit</returns>
        private int GetVehicleStorageLimit(string vehicleSize)
        {
            switch (vehicleSize)
            {
                case "Small":
                    return smallVehicleLimit * level;
                case "Normal":
                    return normalVehicleLimit * level;
                case "Large":
                    return largeVehicleLimit * level;
                case "Huge":
                    return hugeVehicleLimit;
                default: throw new Exception("This is not a valid vehicle size");
            }
        }

        /// <summary>
        /// Calculates how many vehicles you have of the different sizes
        /// </summary>
        private void VehicleAmount()
        {
            smallVehicleAmount = 0;
            normalVehicleAmount = 0;
            largeVehicleAmount = 0;
            hugeVehicleAmount = 0;

            foreach (var vehicle in vehicles)
            {
                switch (vehicle.GetVehicleSize())
                {
                    case "Small":
                        smallVehicleAmount++;
                        break;
                    case "Normal":
                        normalVehicleAmount++;
                        break;
                    case "Large":
                        largeVehicleAmount++;
                        break;
                    case "Huge":
                        hugeVehicleAmount++;
                        break;
                    default: throw new Exception("This is not a valid vehicle size");
                }
            }
        }

        #endregion

        #region Launch

        private void Launch()
        {
            if (launch)
            {
                StoreResources();

                SetOff();
            }
        }

        /// <summary>
        /// Stores the vehicle with all resources you decided to transer and removes them from the base
        /// </summary>
        private void StoreResources()
        {
            #region Load the Vehicle

            //Stores the resources you decided to transer in the vehicle
            currentVehicle.SetStoredResources(foodTransferAmount, "food");
            currentVehicle.SetStoredResources(waterTransferAmount, "water");
            currentVehicle.SetStoredResources(ironTransferAmount, "iron");
            currentVehicle.SetStoredResources(copperTransferAmount, "copper");
            currentVehicle.SetStoredResources(carbonTransferAmount, "carbon");

            #endregion

            #region Remove Resources From The Colony

            colony.GetGrid().resourceManager.food -= foodTransferAmount;
            colony.GetGrid().resourceManager.clearwater -= waterTransferAmount;
            colony.GetGrid().resourceManager.iron -= ironTransferAmount;
            colony.GetGrid().resourceManager.copper -= copperTransferAmount;
            colony.GetGrid().resourceManager.carbon -= carbonTransferAmount;

            #endregion
        }

        /// <summary>
        /// Performs the actual transmission
        /// </summary>
        private void SetOff()
        {
            launch = false;

            //Launch the vehicle that performs the transmission
            Core.galaxyScreen.galaxy.transmissions.Add(new Transmission(currentColony.GetPlanet().GetPosition(),
                colony.GetPlanet().GetPosition(), currentVehicle));

            //Removes the vehicle from the hangar
            vehicles.Remove(currentVehicle);

            currentColony = null;
            currentVehicle = null;
        }

        #endregion

        #region Set Values

        /// <summary>
        /// Sets current colony to the colony you wish to transfer resources
        /// </summary>
        /// <param name="colony">The colony you should transfer resources to</param>
        public void SetCurrentColony(Colony colony)
        {
            currentColony = colony;
        }

        /// <summary>
        /// Sets the bool that confirms the shipment to true
        /// </summary>
        public void SetLaunch()
        {
            launch = true;
        }

        /// <summary>
        /// Sets the vehicle you transfer resources with to (item)
        /// </summary>
        /// <param name="colony">The vehicle you transfer resources with</param>
        public void SetCurrentVehicle(Vehicle vehicle)
        {
            this.currentVehicle = vehicle;
        }

        #endregion

        #region Get Vehicle

        /// <summary>
        /// Gets the vehicle that is about to transfer resources or colonist's
        /// </summary>
        /// <returns>Returns the vehicle that is about to transfer resources or colonist's</returns>
        public Vehicle GetCurrentVehicle()
        {
            return currentVehicle;
        }

        /// <summary>
        /// Gets the list containing vehicles
        /// </summary>
        /// <returns>Resturns the list containing vehicles</returns>
        public List<Vehicle> GetVehicles()
        {
            return vehicles;
        }

        #endregion

        #region Transmission

        #region Get/Set Transfer Amount

        /// <summary>
        /// Changes the transfer amount of a certain resource
        /// </summary>
        /// <param name="change">An int with the change value</param>
        /// <param name="resourceType">Represents the type of resource you want to transfer</param>
        /// <param name="addOrRemove">A bool representing if you remove or add from the amount of resources you want to transfer</param>
        public void SetTransferAmount(string resourceType, bool addOrRemove, int change = 10)
        {
            if (!addOrRemove)
            {
                change *= -1;
            }

            switch (resourceType)
            {
                case "Water":
                    waterTransferAmount += change;
                    if (waterTransferAmount < 0)
                    {
                        waterTransferAmount = 0;
                    }
                    else if (waterTransferAmount > colony.GetGrid().resourceManager.clearwater)
                    {
                        waterTransferAmount = colony.GetGrid().resourceManager.clearwater;
                    }
                    break;

                case "Food":
                    foodTransferAmount += change;
                    if (foodTransferAmount < 0)
                    {
                        foodTransferAmount = 0;
                    }
                    else if (foodTransferAmount > colony.GetGrid().resourceManager.food)
                    {
                        foodTransferAmount = colony.GetGrid().resourceManager.food;
                    }
                    break;

                case "Iron":
                    ironTransferAmount += change;
                    if (ironTransferAmount < 0)
                    {
                        ironTransferAmount = 0;
                    }
                    else if (ironTransferAmount > colony.GetGrid().resourceManager.iron)
                    {
                        ironTransferAmount = colony.GetGrid().resourceManager.iron;
                    }
                    break;

                case "Copper":
                    copperTransferAmount += change;
                    if (copperTransferAmount < 0)
                    {
                        copperTransferAmount = 0;
                    }
                    else if (copperTransferAmount > colony.GetGrid().resourceManager.copper)
                    {
                        copperTransferAmount = colony.GetGrid().resourceManager.copper;
                    }
                    break;

                case "Carbon":
                    carbonTransferAmount += change;
                    if (carbonTransferAmount < 0)
                    {
                        carbonTransferAmount = 0;
                    }
                    else if (carbonTransferAmount > colony.GetGrid().resourceManager.carbon)
                    {
                        carbonTransferAmount = colony.GetGrid().resourceManager.carbon;
                    }
                    break;

                default: throw new Exception("This kind of resource does not exist");
            }
        }

        /// <summary>
        /// Returns the resource transfer amount
        /// </summary>
        /// <param name="resourceType">Represents the type of resource you want to transfer</param>
        /// <returns>An int with the resource transfer amount</returns>
        public int GetTransferAmount(string resourceType)
        {
            switch (resourceType)
            {
                case "Water":
                    return waterTransferAmount;
                case "Food":
                    return foodTransferAmount;
                case "Iron":
                    return ironTransferAmount;
                case "Copper":
                    return copperTransferAmount;
                case "Carbon":
                    return carbonTransferAmount;
                default: throw new Exception("This kind of resource does not exist");
            }
        }

        #endregion

        /// <summary>
        /// Checks if you can transfer more/if the vehicle have more free storage space
        /// </summary>
        /// <returns>Returns true if the vehicle's storage isn't full</returns>
        public bool CanTransferMore()
        {
            if ((foodTransferAmount + waterTransferAmount + ironTransferAmount + copperTransferAmount + carbonTransferAmount) >= currentVehicle.GetStorageLimit())
            {
                return false;
            }
            else if (currentVehicle == null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Basic Facility Information

        /// <summary>
        /// Gets the facility type(Hangar)
        /// </summary>
        /// <returns>Returns the facility type(Hangar)</returns>
        public override string GetFacilityType()
        {
            return base.GetFacilityType();
        }

        #endregion
    }

    class HangarTaskScreen : FacilityTaskScreen
    {
        Colony colony;
        Hangar hangar;

        #region Screens

        public string currentScreen;

        TransferringScreen transferringScreen;
        ChooseColonyScreen chooseColonyScreen;
        ChooseVehicleScreen chooseVehicleScreen;
        ShipmentScreen shipmentScreen;

        #endregion

        public HangarTaskScreen(Colony colony, Hangar hangar)
        {
            this.colony = colony;
            this.hangar = hangar;

            chooseColonyScreen = new ChooseColonyScreen(this);
            chooseVehicleScreen = new ChooseVehicleScreen(this);
            shipmentScreen = new ShipmentScreen(this);
            transferringScreen = new TransferringScreen(this);

            currentScreen = "chooseColonyScreen";
        }

        public override void Update()
        {
            switch (currentScreen)
            {
                case "chooseColonyScreen":
                    chooseColonyScreen.Update();
                    break;
                case "chooseVehicleScreen":
                    chooseVehicleScreen.Update();
                    break;
                case "transferringScreen":
                    transferringScreen.Update();
                    break;
                case "shipmentScreen":
                    shipmentScreen.Update();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets the current colony
        /// </summary>
        /// <returns>Returns the current colony</returns>
        public Colony GetColony()
        {
            return colony;
        }

        /// <summary>
        /// Gets the active hangar
        /// </summary>
        /// <returns>Returns the active hangar</returns>
        public Hangar GetHangar()
        {
            return hangar;
        }

        /// <summary>
        /// Draws all buttons in the hangar task screen
        /// </summary>
        /// <param name="spriteBatch">Used to draw stuff on screen</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (currentScreen)
            {
                case "chooseColonyScreen":
                    chooseColonyScreen.Draw(spriteBatch);
                    break;
                case "chooseVehicleScreen":
                    chooseVehicleScreen.Draw(spriteBatch);
                    break;
                case "transferringScreen":
                    transferringScreen.Draw(spriteBatch);
                    break;
                case "shipmentScreen":
                    shipmentScreen.Draw(spriteBatch);
                    break;
                default:
                    break;
            }
        }
    }

    class TransferringScreen
    {
        HangarTaskScreen hangarTaskScreen;
        Button nextButton;

        #region Transferring

        //Buttons for adding and removing resources
        List<TransferButton> add;
        List<TransferButton> remove;

        #endregion

        /// <summary>
        /// The screen where you decide which colonists and resources your supposed to send to the other base
        /// </summary>
        /// <param name="colony">The current colony</param>
        /// <param name="hangar">The current facility</param>
        public TransferringScreen(HangarTaskScreen hangarTaskScreen)
        {
            this.hangarTaskScreen = hangarTaskScreen;

            #region Transferring

            #region Create List

            //Create the buttons
            add = new List<TransferButton>();
            remove = new List<TransferButton>();

            #endregion

            #region Add

            add.Add(new TransferButton(hangarTaskScreen.GetColony(), "Food", true, hangarTaskScreen.GetHangar()));
            add.Add(new TransferButton(hangarTaskScreen.GetColony(), "Water", true, hangarTaskScreen.GetHangar()));
            add.Add(new TransferButton(hangarTaskScreen.GetColony(), "Iron", true, hangarTaskScreen.GetHangar()));
            add.Add(new TransferButton(hangarTaskScreen.GetColony(), "Copper", true, hangarTaskScreen.GetHangar()));
            add.Add(new TransferButton(hangarTaskScreen.GetColony(), "Carbon", true, hangarTaskScreen.GetHangar()));

            #endregion

            #region Remove

            remove.Add(new TransferButton(hangarTaskScreen.GetColony(), "Food", false, hangarTaskScreen.GetHangar()));
            remove.Add(new TransferButton(hangarTaskScreen.GetColony(), "Water", false, hangarTaskScreen.GetHangar()));
            remove.Add(new TransferButton(hangarTaskScreen.GetColony(), "Iron", false, hangarTaskScreen.GetHangar()));
            remove.Add(new TransferButton(hangarTaskScreen.GetColony(), "Copper", false, hangarTaskScreen.GetHangar()));
            remove.Add(new TransferButton(hangarTaskScreen.GetColony(), "Carbon", false, hangarTaskScreen.GetHangar()));

            #endregion

            #region Position

            #region Add

            for (int i = 0; i < add.Count; i++)
            {
                add[i].SetPosition(new Vector2(600, 400 + i * add[i].GetTexture().Height));
            }

            #endregion

            #region Remove

            for (int i = 0; i < remove.Count; i++)
            {
                remove[i].SetPosition(new Vector2(600 - remove[i].GetTexture().Width, 400 + i * remove[i].GetTexture().Height));
            }

            #endregion

            #endregion

            #endregion

            nextButton = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Next");

            nextButton.SetPosition(new Vector2((Settings.GetScreenRes.X - nextButton.GetTexture().Width),
                (Settings.GetScreenRes.Y - nextButton.GetTexture().Height)));
        }

        /// <summary>
        /// Updates the transferring screen(Handles all calculations regarding transmissions)
        /// </summary>
        public void Update()
        {
            foreach (var increase in add)
            {
                if (hangarTaskScreen.GetHangar().CanTransferMore())
                {
                    //Updates the button that increases the amount of resources you want to transfer when pressed 
                    increase.Update();
                }
            }

            foreach (var decrease in remove)
            {
                //Updates the button that decreases the amount of resources you want to transfer when pressed 
                decrease.Update();
            }
            //Updates the button that lets you proceed to the next screen
            nextButton.Update();

            //If you press the backbutton
            if (HUD.ActionButtons.backButton.Collision())
            {
                //Return to previous screen
                hangarTaskScreen.currentScreen = "chooseVehicleScreen";
            }

            //If you press the next button
            if (nextButton.Collision())
            {
                //Proceed to nex screen
                hangarTaskScreen.currentScreen = "shipmentScreen";
            }
        }

        /// <summary>
        /// Draws all buttons in the transferring screen
        /// </summary>
        /// <param name="spriteBatch">Used to draw stuff on screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var increase in add)
            {
                //Draws the button that increases the amount of resources you want to transfer when pressed 
                increase.Draw(spriteBatch);
            }

            foreach (var decrease in remove)
            {
                //Draws the button that decreases the amount of resources you want to transfer when pressed 
                decrease.Draw(spriteBatch);
            }
            //Draws the button that lets you proceed to the next screen
            nextButton.Draw(spriteBatch);
        }
    }

    class ChooseColonyScreen
    {
        Button nextButton;
        List<ColonyButton> colonyButtons;
        HangarTaskScreen hangarTaskScreen;

        public ChooseColonyScreen(HangarTaskScreen hangarTaskScreen)
        {
            this.hangarTaskScreen = hangarTaskScreen;

            nextButton = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Next");
            colonyButtons = new List<ColonyButton>();

            nextButton.SetPosition(new Vector2((Settings.GetScreenRes.X - nextButton.GetTexture().Width),
                (Settings.GetScreenRes.Y - nextButton.GetTexture().Height)));

            CreateButtons();
        }

        /// <summary>
        /// Creates the colony buttons
        /// </summary>
        private void CreateButtons()
        {

            int amount = 0;

            //Clears the list containing the assembling buttons
            colonyButtons.Clear();

            foreach (var colony in hangarTaskScreen.GetColony().GetColonies())
            {
                colonyButtons.Add(new ColonyButton(amount, colony));
                amount++;
            }
        }

        /// <summary>
        /// Updates the choose colony screen
        /// </summary>
        public void Update()
        {
            if (colonyButtons.Count == 0)
            {
                CreateButtons();
            }

            #region Colony Buttons

            foreach (var button in colonyButtons)
            {
                //Updates the button
                button.Update();

                //Checks if you press a colony button
                if (button.Collision())
                {
                    //Sets the transmission goal to the colony held by the button
                    hangarTaskScreen.GetHangar().SetCurrentColony(button.GetColony());
                }
            }

            #endregion

            //Creates the colony buttons
            if (TimeHandler.newTurn)
                CreateButtons();

            //Updates the button that lets you proceed to the next screen
            nextButton.Update();

            //If you press the next button
            if (nextButton.Collision())
            {
                //Proceedes to the next screen
                hangarTaskScreen.currentScreen = "chooseVehicleScreen";
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in colonyButtons)
            {
                //Draws the colony button
                button.Draw(spriteBatch);
            }
            //Draws the next button
            nextButton.Draw(spriteBatch);
        }
    }

    class ChooseVehicleScreen
    {
        Button nextButton;
        List<VehicleButton> vehicleButtons;
        HangarTaskScreen hangarTaskScreen;

        public ChooseVehicleScreen(HangarTaskScreen hangarTaskScreen)
        {
            this.hangarTaskScreen = hangarTaskScreen;
            nextButton = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Next");
            vehicleButtons = new List<VehicleButton>();

            nextButton.SetPosition(new Vector2((Settings.GetScreenRes.X - nextButton.GetTexture().Width),
                (Settings.GetScreenRes.Y - nextButton.GetTexture().Height)));

            CreateButtons();
        }

        /// <summary>
        /// Creates the colony buttons
        /// </summary>
        private void CreateButtons()
        {

            int amount = 0;

            //Clears the list containing the assembling buttons
            vehicleButtons.Clear();

            foreach (var vehicle in hangarTaskScreen.GetHangar().GetVehicles())
            {
                vehicleButtons.Add(new VehicleButton(amount, vehicle));
                amount++;
            }
        }

        /// <summary>
        /// Updates the choose vehicle screen
        /// </summary>
        public void Update()
        {
            #region Vehicle Buttons

            foreach (var button in vehicleButtons)
            {
                //Updates the button
                button.Update();

                //Checks if you press a vehicle button
                if (button.Collision())
                {
                    //Sets the transmission vehicle to the vehicle held by the button
                    hangarTaskScreen.GetHangar().SetCurrentVehicle(button.GetVehicle());
                }
            }

            #endregion

            //Creates the vehicle buttons
            if (TimeHandler.newTurn)
                CreateButtons();

            //Updates the button that lets you proceed to the next screen
            nextButton.Update();

            //If you press the backbutton
            if (HUD.ActionButtons.backButton.Collision())
            {
                //Return to previous screen
                hangarTaskScreen.currentScreen = "chooseColonyScreen";
            }

            //If you press the next button
            if (nextButton.Collision())
            {
                //Proceed to nex screen
                hangarTaskScreen.currentScreen = "transferringScreen";
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in vehicleButtons)
            {
                //Draws the colony button
                button.Draw(spriteBatch);
            }
            //Draws the next button
            nextButton.Draw(spriteBatch);
        }
    }

    class ShipmentScreen
    {
        Button shipmentButton;
        HangarTaskScreen hangarTaskScreen;

        public ShipmentScreen(HangarTaskScreen hangarTaskScreen)
        {
            this.hangarTaskScreen = hangarTaskScreen;
            shipmentButton = new Button("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "Confirm Shipment");

            shipmentButton.SetPosition(new Vector2((Settings.GetScreenRes.X - shipmentButton.GetTexture().Width),
                (Settings.GetScreenRes.Y - shipmentButton.GetTexture().Height)));
        }

        public void Update()
        {
            //Updates the shipment button
            shipmentButton.Update();

            //If you press the backbutton
            if (HUD.ActionButtons.backButton.Collision())
            {
                //Return to previous screen
                hangarTaskScreen.currentScreen = "transferringScreen";
            }

            //If you press the shipment button
            if (shipmentButton.Collision())
            {
                hangarTaskScreen.currentScreen = "chooseColonyScreen";
                Core.facilityScreen.showTask = false;
                Core.facilityScreen.showWorkers = false;
                hangarTaskScreen.GetHangar().SetLaunch();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Updates the shipment button
            shipmentButton.Draw(spriteBatch);
        }
    }

    class ColonyButton : Button
    {

        Colony colony;

        /// <summary>
        /// Creates a colony button
        /// </summary>
        /// <param name="amount">Contains the amount of buttons you want to add(in this case its representing the number)</param>
        /// <param name="colony">The colony the facility resides in</param>
        public ColonyButton(int amount, Colony colony)
            : base("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "")
        {

            position = SetPosition(amount);

            this.colony = colony;

            label = colony.GetName();
        }

        /// <summary>
        /// Sets the buttons's position
        /// </summary>
        /// <returns>Returns the button's position in pixels</returns>
        private Vector2 SetPosition(int amount)
        {
            int x = 0;
            int y;


            y = HUD.HUD.informationList.GetTexture().Height + texture.Height * amount;
            x = (int)(Settings.GetScreenRes.X / 2 - texture.Width / 2);


            return new Vector2(x, y);

        }

        public Colony GetColony()
        {
            return colony;
        }

        /// <summary>
        /// Updates the colony button
        /// </summary>
        public override void Update()
        {
            base.Update();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    class TransferButton : Button
    {
        Colony colony;
        string resourceType;
        bool addOrRemove;
        Hangar hangar;

        /// <summary>
        /// Creates a Transfer button
        /// </summary>
        /// <param name="colony">The colony the facility resides in</param>
        public TransferButton(Colony colony, string resourceType, bool addOrRemove, Hangar hangar)
            : base("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "")
        {
            this.colony = colony;
            this.resourceType = resourceType;
            this.addOrRemove = addOrRemove;
            this.hangar = hangar;

            label = resourceType;
        }

        public override void Update()
        {
            if (Collision())
            {
                hangar.SetTransferAmount(resourceType, addOrRemove);
            }

            if (addOrRemove)
            {
                label = resourceType + ": " + hangar.GetTransferAmount(resourceType);
            }
            else
            {
                label = "";
            }

            base.Update();
        }

        /// <summary>
        /// Gets the resourceType held by the button
        /// </summary>
        /// <returns>Gets the resourceType held by the button</returns>
        public string GetResourceType()
        {
            return resourceType;
        }

        /// <summary>
        /// Gets addOrRemove
        /// </summary>
        /// <returns>Returns addOrRemove</returns>
        public bool GetAddOrRemove()
        {
            return addOrRemove;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    class VehicleButton : Button
    {
        Vehicle vehicle;

        /// <summary>
        /// Creates a vehicle button
        /// </summary>
        /// <param name="amount">Contains the amount of buttons you want to add(in this case its representing the number)</param>
        /// <param name="colony">The colony the facility resides in</param>
        public VehicleButton(int amount, Vehicle vehicle)
            : base("Res/PH/HUD/Buttons/Standard/ButtonUpPH", Vector2.Zero, "")
        {

            position = SetPosition(amount);

            this.vehicle = vehicle;

            label = vehicle.GetItemType();
        }

        /// <summary>
        /// Sets the buttons's position
        /// </summary>
        /// <returns>Returns the button's position in pixels</returns>
        private Vector2 SetPosition(int amount)
        {
            int x = 0;
            int y;


            y = HUD.HUD.informationList.GetTexture().Height + texture.Height * amount;
            x = (int)(Settings.GetScreenRes.X / 2 - texture.Width / 2);


            return new Vector2(x, y);

        }

        public Vehicle GetVehicle()
        {
            return vehicle;
        }

        /// <summary>
        /// Updates the colony button
        /// </summary>
        public override void Update()
        {
            base.Update();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }

    public struct HangarSave
    {
        public Vector2 position;

        public int colonyID;

        public int level;

        public int storageLevel;

        public int housingLimit;

        public bool finished;

        public int timeUnderConstruction;

        public List<String> vehicles;

        public int ID;
    }
}