using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Items.Vehicles
{
    class VehicleG60 : BasicVehicle
    {
        public VehicleG60()
        {
            itemType = "VehicleG60";
            vehicleType = "Ground";
            assemblingPointsNeeded = 15;
            costCarbon = 100;
            costCopper = 100;
            costIron = 100;
            maintenanceCopper = 0;
            maintenanceCarbon = 0;
            maintenanceIron = 0;
            speed = 5000;
            storageLimit = 250;
            passengerLimit = 1;
        }

    }
}
