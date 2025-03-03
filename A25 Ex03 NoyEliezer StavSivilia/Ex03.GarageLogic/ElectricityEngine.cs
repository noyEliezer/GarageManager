using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class ElectricityEngine : Engine
    {
        public ElectricityEngine(float i_Capacity) : base(i_Capacity) { }

        public override void Refuel(params object[] args)
        {
            if (args == null || args.Length != 1)
            {
                throw new ArgumentException("For electricity type engine only! Required parameter: Charging Minutes");
            }

            if (RemainingEnergy == Capacity)
            {
                throw new ArgumentException("Battery is fully charged!");
            }

            float addedHours = (float)args[0];
            float availableSpace = Capacity - RemainingEnergy;

            if (addedHours > availableSpace || addedHours < 0)
            {
                throw new ValueOutOfRangeException(0, availableSpace * 60, "Battery's Charging Minutes");
            }
            else
            {
                RemainingEnergy += addedHours;
            }
        }

        public override string GetEngineDetailsToString()
        {
            StringBuilder engineDetails = new StringBuilder();

            engineDetails.AppendLine($"Engine Type: Electric");
            engineDetails.AppendLine($"Current Energy Level: {GetEnergyPercentage():F2}%");

            return engineDetails.ToString();
        }

        public override List<Param> GetEngineParams()
        {
            return new List<Param> { new Param("CurrentBatteryTime(InHours )", typeof(float)) };
        }

        public override void UpdateParams(Dictionary<string, object> i_Params)
        {
            try
            {
                if (i_Params.ContainsKey("CurrentBatteryTime(InHours )"))
                {
                    float batteryTime = (float)i_Params["CurrentBatteryTime(InHours )"];

                    if (batteryTime < 0 || batteryTime > Capacity)
                    {
                        throw new ValueOutOfRangeException(0, Capacity, "Current Battery Time (Hours)");
                    }

                    RemainingEnergy = batteryTime;
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Invalid parameter type for Electric Engine", ex);
            }
        }

        public override List<Param> GetRefuelParams()
        {
            float availableChargingTime = (Capacity - RemainingEnergy) * 60;

            return new List<Param> { new Param("ChargingMinutes", typeof(float), "charging time (in minutes)", 0, availableChargingTime) };
        }
    }
}
