using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private const int k_CarNumberOfTires = 5;
        private const float k_CarMaxAirPressure = 34f;
        private eColor m_Color;
        private eNumberOfDoors m_NumberOfDoors;

        public Car(string i_LicenseNumber, Engine i_Engine)
                          : base(i_LicenseNumber, i_Engine)
        {
            InitializeTiresList(k_CarNumberOfTires, k_CarMaxAirPressure);
        }

        public override List<Param> GetRequiredParams()
        {
            List<Param> parameters = GetCommonParams();

            parameters.AddRange(new List<Param> { new Param("Car color", typeof(eColor)),
                                                  new Param("Number of doors", typeof(eNumberOfDoors))});

            return parameters;
        }

        public override string GetUniquePropertiesToString()
        {
            StringBuilder carProperties = new StringBuilder();

            carProperties.AppendLine($"Color: {Color}");
            carProperties.AppendLine($"Number Of Doors: {NumberOfDoors}");

            return carProperties.ToString();
        }

        public override void UpdateParams(Dictionary<string, object> i_Params)
        {
            UpdateCommonParams(i_Params);

            try
            {
                if (i_Params.ContainsKey("Car color"))
                {
                    Color = (eColor)i_Params["Car color"];
                }

                if (i_Params.ContainsKey("Number of doors"))
                {
                    NumberOfDoors = (eNumberOfDoors)i_Params["Number of doors"];
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Invalid parameter type", ex);
            }
        }

        public override void ResetParameters()
        {
            Color = default(eColor);
            NumberOfDoors = default(eNumberOfDoors);
            InitializeTiresList(k_CarNumberOfTires, k_CarMaxAirPressure);
            base.ResetParameters();
        }

        public eColor Color
        {
            set
            {
                m_Color = value;
            }
            get
            {
                return m_Color;
            }
        }

        public eNumberOfDoors NumberOfDoors
        {
            set
            {
                m_NumberOfDoors = value;
            }
            get
            {
                return m_NumberOfDoors;
            }
        }
    }
}