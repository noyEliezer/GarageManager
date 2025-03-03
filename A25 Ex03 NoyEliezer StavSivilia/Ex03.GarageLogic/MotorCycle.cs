using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class MotorCycle : Vehicle
    {
        private const int k_MotorcycleNumberOfTires = 2;
        private const float k_MotorcycleMaxAirPressure = 32f;
        private eLicenseType m_LicenseType;
        private int m_EngineVolume;

        public MotorCycle(string i_LicenseNumber, Engine i_Engine)
                          : base(i_LicenseNumber, i_Engine)
        {
            InitializeTiresList(k_MotorcycleNumberOfTires, k_MotorcycleMaxAirPressure);
        }

        public override List<Param> GetRequiredParams()
        {
            List<Param> parameters = GetCommonParams();

            parameters.AddRange(new List<Param> { new Param("License Type", typeof(eLicenseType)),
                                                  new Param("EngineVolume", typeof(int))});

            return parameters;
        }

        public override string GetUniquePropertiesToString()
        {
            StringBuilder motorcycleProperties = new StringBuilder();

            motorcycleProperties.AppendLine($"License Type: {LicenseType}");
            motorcycleProperties.AppendLine($"Engine's Volume: {EngineVolume}");

            return motorcycleProperties.ToString();
        }

        public override void UpdateParams(Dictionary<string, object> i_Params)
        {
            UpdateCommonParams(i_Params);
            try
            {
                if (i_Params.ContainsKey("LicenseType"))
                {
                    eLicenseType licenseType = (eLicenseType)i_Params["LicenseType"];

                    if (!Enum.IsDefined(typeof(eLicenseType), licenseType))
                    {
                        throw new ArgumentException("Invalid license type");
                    }

                    LicenseType = licenseType;
                }

                if (i_Params.ContainsKey("EngineVolume"))
                {
                    EngineVolume = (int)i_Params["EngineVolume"];
                }
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException("Invalid parameter type for Motorcycle", ex);
            }
        }

        public override void ResetParameters()
        {
            LicenseType = default(eLicenseType);
            EngineVolume = 0;
            InitializeTiresList(k_MotorcycleNumberOfTires, k_MotorcycleMaxAirPressure);
            base.ResetParameters();
        }

        public eLicenseType LicenseType
        {
            set
            {
                m_LicenseType = value;
            }
            get
            {
                return m_LicenseType;
            }
        }

        public int EngineVolume
        {
            set
            {
                if (value < 0 || value > 1000)
                {
                    throw new ValueOutOfRangeException(0, 1000, "Engine's Volume");
                }
                m_EngineVolume = value;
            }
            get
            {
                return m_EngineVolume;
            }
        }
    }
}