namespace Ex03.GarageLogic
{
    public class Tire
    {
        private string m_ManufacturerName;
        private readonly float r_MaxAirPressure;
        private float m_CurrentAirPressure;

        public Tire(float i_MaxAirPressure)
        {
            r_MaxAirPressure = i_MaxAirPressure;
            m_CurrentAirPressure = 0f;
        }
        public void Inflate(float i_AirToAdd)
        {
            if (m_CurrentAirPressure + i_AirToAdd <= r_MaxAirPressure && i_AirToAdd > 0)
            {
                CurrentAirPressure += i_AirToAdd;
            }
            else
            {
                throw new ValueOutOfRangeException(0, r_MaxAirPressure, "Current Tires Pressure");
            }
        }

        public void InflateToMax()
        {
            CurrentAirPressure = MaxAirPressure;
        }

        public string ManufacturerName
        {
            set
            {
                m_ManufacturerName = value;
            }
            get
            {
                return m_ManufacturerName;
            }
        }

        public float MaxAirPressure
        {
            get
            {
                return r_MaxAirPressure;
            }
        }

        public float CurrentAirPressure
        {
            set
            {
                if (value >= 0f && value <= r_MaxAirPressure)
                {
                    m_CurrentAirPressure = value;
                }
                else
                {
                    throw new ValueOutOfRangeException(0, r_MaxAirPressure, "Tire's Max Air Pressure");
                }
            }
            get
            {
                return m_CurrentAirPressure;
            }
        }
    }
}