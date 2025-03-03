using System.Collections.Generic;
using System;

namespace Ex03.GarageLogic
{
    public static class VehicleCreator
    {
        private const float k_MotorcycleMaxBatteryTime = 2.9f;
        private const float k_MotorcycleMaxFuelTank = 6.2f;
        private const float k_CarMaxBatteryTime = 5.4f;
        private const float k_CarMaxFuelTank = 52f;
        private const float k_TruckMaxFuelTank = 125f;

        public static Vehicle CreateVehicle(eVehicleType i_VehicleType, string i_LicenceNumber)
        {
            Vehicle vehicle;
            Engine engine;

            switch (i_VehicleType)
            {
                case eVehicleType.ElectricCar:
                    engine = new ElectricityEngine(k_CarMaxBatteryTime);
                    vehicle = new Car(i_LicenceNumber, engine);
                    break;

                case eVehicleType.FuelCar:
                    engine = new FuelEngine(k_CarMaxFuelTank, eFuelType.Octan95);
                    vehicle = new Car(i_LicenceNumber, engine);
                    break;

                case eVehicleType.ElectricMotorcycle:
                    engine = new ElectricityEngine(k_MotorcycleMaxBatteryTime);
                    vehicle = new MotorCycle(i_LicenceNumber, engine);
                    break;

                case eVehicleType.FuelMotorcycle:
                    engine = new FuelEngine(k_MotorcycleMaxFuelTank, eFuelType.Octan98);
                    vehicle = new MotorCycle(i_LicenceNumber, engine);
                    break;

                case eVehicleType.Truck:
                    engine = new FuelEngine(k_TruckMaxFuelTank, eFuelType.Soler);
                    vehicle = new Truck(i_LicenceNumber, engine);
                    break;

                default:
                    throw new ArgumentException("Invalid vehicle type");
            }

            return vehicle;
        }

        public static List<Param> GetRequiredParams(Vehicle i_Vehicle)
        {
            return i_Vehicle.GetRequiredParams();
        }

        public static bool UpdateVehicleWithParameters(Vehicle i_Vehicle, Dictionary<string, object> i_UserParams, out string o_ErrorMessage)
        {
            o_ErrorMessage = string.Empty;
            bool isUpdateSuccessful = false;

            try
            {
                i_Vehicle.UpdateParams(i_UserParams);
                isUpdateSuccessful = true;
            }
            catch (ValueOutOfRangeException ex)
            {
                o_ErrorMessage = $"{ex.Message}";
            }
            catch (ArgumentException ex)
            {
                o_ErrorMessage = $"Invalid argument: {ex.Message}";
            }
            catch (FormatException ex)
            {
                o_ErrorMessage = $"Invalid format: {ex.Message}";
            }
            catch (Exception ex)
            {
                o_ErrorMessage = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                if(!isUpdateSuccessful)
                {
                    i_Vehicle.ResetParameters();
                }
            }

            return isUpdateSuccessful;
        }
    }
}
