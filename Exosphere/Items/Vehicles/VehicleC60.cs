using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exosphere.Src.Items.Vehicles
{
    class VehicleC60 : Vehicle
    {
        public VehicleC60()
        {
            itemType = "Vehicle C60";
            assemblingPointsNeeded = 15;
            costCarbon = 100;
            costCopper = 100;
            costIron = 100;
            maintenanceCopper = 3;
            maintenanceCarbon = 3;
            maintenanceIron = 3;
            speed = 500;
            storageLimit = 1000;
            passengerLimit = 2;
        }
    }
}
