using System;

namespace Ex03.GarageLogic
{
    public class Param
    {
        private readonly string r_ParamName;
        private readonly Type r_ParamType;
        private readonly float? r_MinValue;
        private readonly float? r_MaxValue;
        private readonly string r_Description;
        private readonly object r_AllowedEnumValue;

        public Param(string i_ParamName, Type i_ParamType, string i_Description = "", float? i_MinValue = null, float? i_MaxValue = null, object i_AllowedEnumValue = null)
        {
            r_ParamName = i_ParamName;
            r_ParamType = i_ParamType;
            r_Description = i_Description;
            r_MinValue = i_MinValue;
            r_MaxValue = i_MaxValue;
            if (i_AllowedEnumValue != null && i_ParamType.IsEnum && i_AllowedEnumValue.GetType() == i_ParamType)
            {
                r_AllowedEnumValue = i_AllowedEnumValue;
            }
        }

        public object AllowedEnumValue
        {
            get { return r_AllowedEnumValue; }
        }

        public string ParamName
        {
            get { return r_ParamName; }
        }

        public Type ParamType
        {
            get { return r_ParamType; }
        }

        public float? MinValue
        {
            get { return r_MinValue; }
        }

        public float? MaxValue
        {
            get { return r_MaxValue; }
        }

        public string Description
        {
            get { return r_Description; }
        } 
    }
}


