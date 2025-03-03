using System.Text;

namespace Ex03.GarageLogic
{
    public class VehicleInfo
    {
        private Vehicle m_Vehicle;
        private string m_OwnersName;
        private string m_OwnersPhoneNumber;
        private eVehicleStatus m_VehicleStatusInGarage;

        public VehicleInfo(Vehicle i_Vehicle)
        {
            m_Vehicle = i_Vehicle;
            m_VehicleStatusInGarage = eVehicleStatus.InFix;
        }

        public void InitializeOwnerDetails(string i_OwnerName, string i_OwnerPhoneNumber)
        {
            m_OwnersName = i_OwnerName;
            OwnersPhoneNumber = i_OwnerPhoneNumber;
        }

        public override string ToString()
        {
            StringBuilder vehicleDetails = new StringBuilder();

            vehicleDetails.AppendLine($"License Number: {m_Vehicle.LicenseNumber}");
            vehicleDetails.AppendLine($"Modele Name: {m_Vehicle.ModelName}");
            vehicleDetails.AppendLine($"Owner's Name: {m_OwnersName}");
            vehicleDetails.AppendLine($"Status: {m_VehicleStatusInGarage}");
            vehicleDetails.AppendLine(m_Vehicle.GetTiresInfoToString());
            vehicleDetails.Append(m_Vehicle.Engine.GetEngineDetailsToString());
            vehicleDetails.Append(m_Vehicle.GetUniquePropertiesToString());

            return vehicleDetails.ToString();
        }

        public Vehicle Vehicle
        {
            get
            {
                return m_Vehicle;
            }
        }

        public string LicenseNumber
        {
            get
            {
                return m_Vehicle.LicenseNumber;
            }
        }

        public string OwnersName
        {
            get
            {
                return m_OwnersName;
            }
            set
            {
                m_OwnersName = value;
            }
        }

        public string OwnersPhoneNumber
        {
            set
            {
                if (value.Length != 10)
                {
                    throw new ValueOutOfRangeException(10, 10, "Phone Number");
                }

                m_OwnersPhoneNumber = value;
            }
            get
            {
                return m_OwnersPhoneNumber;
            }
        }

        public eVehicleStatus VehicleStatusInGarage
        {
            set
            {
                m_VehicleStatusInGarage = value;
            }
            get
            {
                return m_VehicleStatusInGarage;
            }
        }
    }
}
