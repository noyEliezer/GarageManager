using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        private float m_Capacity;
        private float m_RemainingEnergy;

        public Engine(float i_Capacity)
        {
            m_Capacity = i_Capacity;
            RemainingEnergy = 0;
        }

        public abstract void Refuel(params object[] args);

        public abstract string GetEngineDetailsToString();

        public abstract void UpdateParams(Dictionary<string, object> i_Params);

        public abstract List<Param> GetEngineParams();

        public abstract List<Param> GetRefuelParams();

        public float Capacity
        {
            get
            {
                return m_Capacity;
            }
        }

        public float RemainingEnergy
        {
            set
            {
                if (value < 0 || value > m_Capacity)
                {
                    throw new ValueOutOfRangeException(0, Capacity, "Remaining Energy");
                }

                m_RemainingEnergy = value;
            }
            get
            {
                return m_RemainingEnergy;
            }
        }

        public float GetEnergyPercentage()
        {
            return (m_RemainingEnergy / m_Capacity) * 100f;
        }
    }
}