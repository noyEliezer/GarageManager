using System;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private readonly float r_MinValue;
        private readonly float r_MaxValue;
        private readonly string r_ParameterName;

        public ValueOutOfRangeException(float i_MinValue, float i_MaxValue, string i_ParameterName)
               : base(string.IsNullOrEmpty(i_ParameterName)
               ? $"Input is out of range: Allowed range is {i_MinValue} to {i_MaxValue}."
               : $"Input is out of range for parameter '{i_ParameterName}': Allowed range is {i_MinValue} to {i_MaxValue}.")
        {
            r_MinValue = i_MinValue;
            r_MaxValue = i_MaxValue;
            r_ParameterName = i_ParameterName;
        }

        public float MinValue
        {
            get
            {
                return r_MinValue;
            }
        }

        public float MaxValue
        {
            get
            {
                return r_MaxValue;
            }
        }

        public string ParameterName
        {
            get
            {
                return r_ParameterName;
            }
        }
    }
}