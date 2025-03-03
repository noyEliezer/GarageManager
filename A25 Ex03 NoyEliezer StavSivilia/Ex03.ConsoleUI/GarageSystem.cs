using Ex03.GarageLogic;
using System;
using System.Collections.Generic;

namespace Ex03.ConsoleUI
{
    public class GarageSystem
    {
        public static void StartSystem()
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                displayMainMenu();
                int userChoice = GarageUIManager.GetValidIntegerInput(1, 8);

                Console.Clear();
                try
                {
                    switch (userChoice)
                    {
                        case 1:
                            handleNewVehicle();
                            break;
                        case 2:
                            displayVehicleList();
                            break;
                        case 8:
                            continueRunning = false;
                            break;
                        default:
                            string licenseNumber = GarageUIManager.GetUserLicenseNumber();

                            licenseNumber = GarageUIManager.HandleVehicleLicenseNumber(licenseNumber);
                            if (licenseNumber != null)
                            {
                                switch (userChoice)
                                {
                                    case 3:
                                        changeVehicleStatus(licenseNumber);
                                        break;
                                    case 4:
                                        inflateVehicleTires(licenseNumber);
                                        break;
                                    case 5:
                                        refuelVehicle(licenseNumber);
                                        break;
                                    case 6:
                                        chargeVehicle(licenseNumber);
                                        break;
                                    case 7:
                                        displayVehicleDetails(licenseNumber);
                                        break;
                                }
                            }

                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                catch (ValueOutOfRangeException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (continueRunning)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static void displayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Garage Management System");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Add new vehicle to garage");
            Console.WriteLine("2. Display vehicle license numbers");
            Console.WriteLine("3. Change vehicle status");
            Console.WriteLine("4. Inflate vehicle tires");
            Console.WriteLine("5. Refuel vehicle");
            Console.WriteLine("6. Charge electric vehicle");
            Console.WriteLine("7. Display vehicle full details");
            Console.WriteLine("8. Exit");
            Console.Write($"{Environment.NewLine}Please choose an option (1-8) : ");
        }

        private static void handleNewVehicle()
        {
            string licenseNumber = GarageUIManager.GetUserLicenseNumber();

            if (GarageManager.IsVehicleInGarage(licenseNumber))
            {
                GarageManager.ChangeVehicleStatusByLicenseNumber(licenseNumber, eVehicleStatus.InFix);
                Console.WriteLine($"Vehicle is already in the garage. Status changed to 'In Fix' {Environment.NewLine}");
            }
            else
            {
                GarageUIManager.AddNewVehicle(licenseNumber);
            }
        }

        private static void displayVehicleList()
        {
            eVehicleStatus? filterStatus = null;

            Console.Write("Would you like to filter by status? (y/n) : ");
            if (GarageUIManager.GetBooleanInput())
            {
                filterStatus = (eVehicleStatus)Enum.GetValues(typeof(eVehicleStatus)).
                               GetValue(GarageUIManager.GetEnumChoice(Enum.GetValues(typeof(eVehicleStatus)), "Filter vehicles by status"));
            }

            List<string> vehicleList = filterStatus.HasValue
                ? GarageManager.GetAllLicenseNumbersByStatus(filterStatus.Value)
                : GarageManager.GetAllLicenseNumbers();

            if (vehicleList.Count == 0)
            {
                Console.WriteLine($"No vehicles found matching the criteria{Environment.NewLine}");
            }
            else
            {
                Console.WriteLine("License Numbers :");
                foreach (string license in vehicleList)
                {
                    Console.WriteLine(license);
                }
            }
        }

        private static void changeVehicleStatus(string i_LicenseNumber)
        {
            eVehicleStatus newStatus = (eVehicleStatus)Enum.GetValues(typeof(eVehicleStatus)).
                                        GetValue(GarageUIManager.GetEnumChoice(Enum.GetValues(typeof(eVehicleStatus)), "new status"));

            GarageManager.ChangeVehicleStatusByLicenseNumber(i_LicenseNumber, newStatus);
            Console.WriteLine($"Status updated successfully {Environment.NewLine}");
        }

        private static void inflateVehicleTires(string i_LicenseNumber)
        {
            GarageManager.FillTiresAirToMaxByLicenseNumber(i_LicenseNumber);
            Console.WriteLine($"Tires inflated successfully {Environment.NewLine}");
        }

        private static void refuelVehicle(string i_LicenseNumber)
        {
            try
            {
                if (!GarageManager.IsFuelVehicle(i_LicenseNumber))
                {
                    Console.WriteLine("Error: This is an electric vehicle. Please use the charging option instead.");
                }
                else
                {
                    List<Param> fuelParams = GarageManager.GetRefuelParams(i_LicenseNumber);
                    Dictionary<string, object> paramValues = GarageUIManager.GetParametersFromUser(fuelParams);
                    
                    GarageManager.RefuelVehicleTankByLicenseNumber(i_LicenseNumber, paramValues);
                    Console.WriteLine($"Vehicle refueled successfully{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void chargeVehicle(string i_LicenseNumber)
        {
            try
            {
                if (!GarageManager.IsElectricVehicle(i_LicenseNumber))
                {
                    Console.WriteLine("Error: This is a fuel vehicle. Please use the refueling option instead.");
                }
                else
                {
                    List<Param> chargeParams = GarageManager.GetChargingParams(i_LicenseNumber);
                    Dictionary<string, object> paramValues = GarageUIManager.GetParametersFromUser(chargeParams);
                    
                    GarageManager.ChargeVehicleByLicenseNumber(i_LicenseNumber, paramValues);
                    Console.WriteLine($"Vehicle charged successfully{Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void displayVehicleDetails(string i_LicenseNumber)
        {
            string vehicleDetails = GarageManager.GetVehicleFullDetailsByLicenseNumber(i_LicenseNumber);

            Console.WriteLine(vehicleDetails);
        }
    }
}