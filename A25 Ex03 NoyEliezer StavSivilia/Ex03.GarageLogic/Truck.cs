using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private const int k_TruckNumberOfTires = 14;
        private const float k_TruckMaxAirPressure = 29f;
        private bool m_Refrigerating;
        private float m_CargoVolume;

        public Truck(string i_LicenseNumber, Engine i_Engine)
                          : base(i_LicenseNumber, i_Engine)
        {
            InitializeTiresList(k_TruckNumberOfTires, k_TruckMaxAirPressure);
        }

        public override List<Param> GetRequiredParams()
        {
            List<Param> parameters = GetCommonParams();

            parameters.AddRange(new List<Param> {new Param("IsRefrigerating", typeof(bool)),
                                                 new Param("CargoVolume", typeof(float))});

            return parameters;
        }

        public override string GetUniquePropertiesToString()
        {
            StringBuilder truckProperties = new StringBuilder();

            truckProperties.AppendLine($"Cargo's Volume: {CargoVolume}");
            truckProperties.AppendLine($"Has Refrigerating ?: {(Refrigerating ? "Yes" : "No")}");

            return truckProperties.ToString();
        }

        public override void UpdateParams(Dictionary<string, object> i_Params)
        {
            UpdateCommonParams(i_Params);
            try
            {
                if (i_Params.ContainsKey("IsRefrigerating"))
                {
                    Refrigerating = (bool)i_Params["IsRefrigerating"];
                }

                if (i_Params.ContainsKey("CargoVolume"))
                {
                    float cargoVolume = (float)i_Params["CargoVolume"];
                    CargoVolume = cargoVolume;
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Invalid parameter type for Truck", ex);
            }
        }

        public override void ResetParameters()
        {
            Refrigerating = false;
            CargoVolume = 0;
            InitializeTiresList(k_TruckNumberOfTires, k_TruckMaxAirPressure);
            base.ResetParameters();
        }

        public bool Refrigerating
        {
            set
            {
                m_Refrigerating = value;
            }
            get
            {
                return m_Refrigerating;
            }
        }

        public float CargoVolume
        {
            set
            {
                if (value < 0.0f || value > 50000)
                {
                    throw new ValueOutOfRangeException(0, 50000, "Truck's Cargo Volume");
                }
                else
                {
                    m_CargoVolume = value;
                }
            }
            get
            {
                return m_CargoVolume;
            }
        }
    }
}
