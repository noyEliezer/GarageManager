using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class FuelEngine : Engine
    {
        private readonly eFuelType r_FuelType;

        public FuelEngine(float i_Capacity, eFuelType i_FuelType) : base(i_Capacity)
        {
            r_FuelType = i_FuelType;
            i_FuelType = default(eFuelType);
        }
        public override void Refuel(params object[] args)
        {
            if (args == null || args.Length != 2)
            {
                throw new ArgumentException("For fuel type engine only! Required parameters: fuel type and amount");
            }

            eFuelType fuelType = (eFuelType)args[0];
            float addedliters = (float)args[1];

            if (r_FuelType != fuelType)
            {
                throw new ArgumentException("Incorrect fuel type");
            }

            if (RemainingEnergy == Capacity)
            {
                throw new ArgumentException("Tank is full!");
            }

            float availableSpace = Capacity - RemainingEnergy;
            
            if (addedliters > availableSpace || addedliters < 0)
            {
                throw new ValueOutOfRangeException(0, availableSpace, "Tank's Added Liters");
            }

            RemainingEnergy += addedliters;
        }
        public override string GetEngineDetailsToString()
        {
            StringBuilder engineDetails = new StringBuilder();

            engineDetails.AppendLine($"Engine Type: Fuel ({FuelType})");
            engineDetails.AppendLine($"Current Energy Level: {GetEnergyPercentage():F2}%");

            return engineDetails.ToString();
        }

        public override List<Param> GetEngineParams()
        {
            return new List<Param> { new Param("CurrentFuelAmount", typeof(float), "fuel type", null, null, r_FuelType) };
        }

        public override void UpdateParams(Dictionary<string, object> i_Params)
        {
            try
            {
                if (i_Params.ContainsKey("FuelType"))
                {
                    eFuelType fuelType = (eFuelType)i_Params["FuelType"];

                    if (!Enum.IsDefined(typeof(eFuelType), fuelType))
                    {
                        throw new ArgumentException("Invalid fuel type");
                    }

                }

                if (i_Params.ContainsKey("CurrentFuelAmount"))
                {
                    float fuelAmount = (float)i_Params["CurrentFuelAmount"];

                    if (fuelAmount < 0)
                    {
                        throw new ValueOutOfRangeException(0, Capacity, "Current Fuel Amount");
                    }

                      RemainingEnergy = fuelAmount;         
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Invalid parameter type for Fuel Engine", ex);
            }
        }

        public override List<Param> GetRefuelParams()
        {
            return new List<Param>{new Param("FuelType", typeof(eFuelType), "fuel type", null, null, r_FuelType),
                                   new Param("FuelAmount", typeof(float), "fuel to add (in liters)", 0, Capacity - RemainingEnergy)};
        }

        public eFuelType FuelType
        {

            get
            {
                return r_FuelType;
            }
        }
    }
}