using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class GarageManager
    {
        private const short k_LicenseNumberLength = 8;
        public static readonly Dictionary<string, VehicleInfo> r_GarageVehicles = new Dictionary<string, VehicleInfo>();

        public static bool IsVehicleInGarage(string i_LicenseNumber)
        {
            return r_GarageVehicles.ContainsKey(i_LicenseNumber);
        }

        public static bool IsValidVehicleLicenseNumber(string i_VehicleLicenseNumber)
        {
            bool validVehicleLicenseNumber = i_VehicleLicenseNumber.Length == k_LicenseNumberLength;

            if (validVehicleLicenseNumber)
            {
                foreach (char c in i_VehicleLicenseNumber)
                {
                    if (!char.IsDigit(c))
                    {
                        validVehicleLicenseNumber = false;
                    }
                }
            }

            return validVehicleLicenseNumber;
        }

        public static void AddNewVehicle(VehicleInfo i_NewGarageVehicle)
        {
            r_GarageVehicles.Add(i_NewGarageVehicle.LicenseNumber, i_NewGarageVehicle);
        }

        public static List<string> GetAllLicenseNumbers()
        {
            List<string> allLicenseNumbers = new List<string>();

            foreach (KeyValuePair<string, VehicleInfo> garageVehiclePair in r_GarageVehicles)
            {
                allLicenseNumbers.Add(garageVehiclePair.Key);
            }

            return allLicenseNumbers;
        }

        public static List<string> GetAllLicenseNumbersByStatus(eVehicleStatus i_VehicleStatus)
        {
            List<string> allLicenseNumbersByStatus = new List<string>();

            foreach (KeyValuePair<string, VehicleInfo> vehiclePairInGarage in r_GarageVehicles)
            {
                if (vehiclePairInGarage.Value.VehicleStatusInGarage == i_VehicleStatus)
                {
                    allLicenseNumbersByStatus.Add(vehiclePairInGarage.Key);
                }
            }

            return allLicenseNumbersByStatus;
        }

        public static void ChangeVehicleStatusByLicenseNumber(string i_LicenseNumber, eVehicleStatus i_SelectedVehicleStatus)
        {
            r_GarageVehicles[i_LicenseNumber].VehicleStatusInGarage = i_SelectedVehicleStatus;
        }

        public static void FillTiresAirToMaxByLicenseNumber(string i_LicenseNumber)
        {
            r_GarageVehicles[i_LicenseNumber].Vehicle.InflateAllToMax();
        }

        public static bool IsFuelVehicle(string i_LicenseNumber)
        {
            return r_GarageVehicles[i_LicenseNumber].Vehicle.Engine is FuelEngine;
        }

        public static List<Param> GetRefuelParams(string i_LicenseNumber)
        {
            if (!IsFuelVehicle(i_LicenseNumber))
            {
                throw new ArgumentException("This is not a fuel vehicle");
            }

            return r_GarageVehicles[i_LicenseNumber].Vehicle.Engine.GetRefuelParams();
        }

        public static void RefuelVehicleTankByLicenseNumber(string i_LicenseNumber, Dictionary<string, object> i_RefuelParams)
        {
            if (!(r_GarageVehicles[i_LicenseNumber].Vehicle.Engine is FuelEngine))
            {
                throw new ArgumentException("Cannot refuel an electric vehicle. Please use the charging method instead.");
            }

            eFuelType fuelType = (eFuelType)i_RefuelParams["FuelType"];
            float fuelAmount = (float)i_RefuelParams["FuelAmount"];

            r_GarageVehicles[i_LicenseNumber].Vehicle.Engine.Refuel(fuelType, fuelAmount);
            r_GarageVehicles[i_LicenseNumber].Vehicle.EnergyPercentage = r_GarageVehicles[i_LicenseNumber].Vehicle.Engine.GetEnergyPercentage();
        }

        public static bool IsElectricVehicle(string i_LicenseNumber)
        {
            return r_GarageVehicles[i_LicenseNumber].Vehicle.Engine is ElectricityEngine;
        }

        public static List<Param> GetChargingParams(string i_LicenseNumber)
        {
            if (!IsElectricVehicle(i_LicenseNumber))
            {
                throw new ArgumentException("This is not an electric vehicle");
            }

            return r_GarageVehicles[i_LicenseNumber].Vehicle.Engine.GetRefuelParams();
        }

        public static void ChargeVehicleByLicenseNumber(string i_LicenseNumber, Dictionary<string, object> i_ChargingParams)
        {
            if (!(r_GarageVehicles[i_LicenseNumber].Vehicle.Engine is ElectricityEngine))
            {
                throw new ArgumentException("Cannot charge a fuel-based vehicle. Please use the refuel method instead.");
            }

            float minutesToCharge = (float)i_ChargingParams["ChargingMinutes"];
            float hoursToCharge = minutesToCharge/60;

            r_GarageVehicles[i_LicenseNumber].Vehicle.Engine.Refuel(hoursToCharge);
            r_GarageVehicles[i_LicenseNumber].Vehicle.EnergyPercentage = r_GarageVehicles[i_LicenseNumber].Vehicle.Engine.GetEnergyPercentage();
        }

        public static string GetVehicleFullDetailsByLicenseNumber(string i_LicenseNumber)
        {
            VehicleInfo vehicleInfo = r_GarageVehicles[i_LicenseNumber];

            return vehicleInfo.ToString();
        }
    }
}