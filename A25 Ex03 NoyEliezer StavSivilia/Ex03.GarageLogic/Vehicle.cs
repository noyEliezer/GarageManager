using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        private readonly string r_LicenseNumber;
        private string m_ModelName;
        private float m_EnergyPercentage;
        protected Engine m_Engine;
        private readonly List<Tire> r_TiresList;

        protected Vehicle(string i_LicenseNumber, Engine i_Engine)
        {
            r_LicenseNumber = i_LicenseNumber;
            m_Engine = i_Engine;
            r_TiresList = new List<Tire>();
        }

        protected void InitializeTiresList(int i_NumberOfTires, float i_TiresMaxPressure)
        {
            r_TiresList.Clear();
            for (int i = 0; i < i_NumberOfTires; i++)
            {
                r_TiresList.Add(new Tire(i_TiresMaxPressure));
            }

            resetTiresAirPressure();
        }

        private void resetTiresAirPressure()
        {
            foreach (Tire tire in r_TiresList)
            {
                tire.CurrentAirPressure = 0f;
            }
        }

        protected void UpdateCommonParams(Dictionary<string, object> i_Params)
        {
            try
            {
                if (i_Params.ContainsKey("ModelName"))
                {
                    ModelName = (string)i_Params["ModelName"];
                }

                if (i_Params.ContainsKey("TiresManufacturer") &&
                    i_Params.ContainsKey("CurrentTiresPressure"))
                {
                    string manufacturer = (string)i_Params["TiresManufacturer"];
                    float pressure = (float)i_Params["CurrentTiresPressure"];

                    if (string.IsNullOrEmpty(manufacturer))
                    {
                        throw new ArgumentException("Manufacturer name cannot be empty");
                    }

                    setAllTiresManufacturer(manufacturer);
                    inflateAllTires(pressure);
                }

                m_Engine.UpdateParams(i_Params);
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Invalid parameter type", ex);
            }
        }

        public string GetTiresInfoToString()
        {
            StringBuilder tiresInfo = new StringBuilder();

            tiresInfo.AppendLine($"Manufacturer: {r_TiresList[0].ManufacturerName}, Pressure: {r_TiresList[0].CurrentAirPressure}/{r_TiresList[0].MaxAirPressure}");

            return tiresInfo.ToString();
        }

        private void setAllTiresManufacturer(string i_ManufacturerName)
        {
            foreach (Tire tire in r_TiresList)
            {
                tire.ManufacturerName = i_ManufacturerName;
            }
        }

        public void InflateAllToMax()
        {
            foreach (Tire tire in r_TiresList)
            {
                tire.InflateToMax();
            }
        }

        private void inflateAllTires(float i_TireAirPressure)
        {
            foreach (Tire tire in r_TiresList)
            {
                tire.Inflate(i_TireAirPressure);
            }
        }

        protected List<Param> GetCommonParams()
        {
            List<Param> parameters = new List<Param>{ new Param("ModelName", typeof(string)),
                                    new Param("TiresManufacturer", typeof(string)),
                                    new Param("CurrentTiresPressure", typeof(float)) };

            parameters.AddRange(m_Engine.GetEngineParams());

            return parameters;
        }

        public abstract string GetUniquePropertiesToString();

        public abstract List<Param> GetRequiredParams();

        public abstract void UpdateParams(Dictionary<string, object> i_Params);

        public virtual void ResetParameters()
        {
            ModelName = null;
            EnergyPercentage = 0f;
        }

        public string LicenseNumber
        {
            get
            {
                return r_LicenseNumber;
            }
        }

        public string ModelName
        {
            set
            {
                m_ModelName = value;
            }
            get
            {
                return m_ModelName;
            }
        }

        public float EnergyPercentage
        {
            get
            {
                return m_EnergyPercentage;
            }
            set
            {
                m_EnergyPercentage = value;
            }
        }

        public Engine Engine
        {
            set
            {
                m_Engine = value;
            }
            get
            {
                return m_Engine;
            }
        }
    }
}