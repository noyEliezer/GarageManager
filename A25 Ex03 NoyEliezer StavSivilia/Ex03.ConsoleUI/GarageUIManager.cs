using Ex03.GarageLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.ConsoleUI
{
    public static class GarageUIManager
    {
        public static string GetUserLicenseNumber()
        {

            Console.Write("Please enter vehicle license number : ");
            string userInput = Console.ReadLine();

            while (!GarageManager.IsValidVehicleLicenseNumber(userInput))
            {
                Console.Write("Invalid license number. Please enter vehicle license number : ");
                userInput = Console.ReadLine();
            }

            Console.WriteLine();

            return userInput;
        }

        private static string getValidOwnerName()
        {
            Console.Write("Please enter vehicle owner's name : ");
            string ownerName = getValidStringInput();

            while (!isValidName(ownerName))
            {
                Console.Write("Invalid name. Please enter only letters and spaces : ");
                ownerName = getValidStringInput();
            }

            Console.WriteLine();

            return ownerName;
        }

        private static string getValidOwnerPhone()
        {
            Console.Write("Please enter vehicle owner's phone number : ");
            string ownerPhone = getValidStringInput();

            while (!isValidPhoneNumber(ownerPhone))
            {
                Console.Write("Invalid phone number. Please enter exactly 10 digits : ");
                ownerPhone = getValidStringInput();
            }

            Console.WriteLine();

            return ownerPhone;
        }

        private static bool isValidPhoneNumber(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && phone.All(char.IsDigit);
        }

        private static bool isValidName(string i_Name)
        {
            bool isValid = !string.IsNullOrWhiteSpace(i_Name);

            if (isValid)
            {
                for (int i = 0; i < i_Name.Length; i++)
                {
                    if (!char.IsLetter(i_Name[i]) && !char.IsWhiteSpace(i_Name[i]))
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }

        public static Dictionary<string, object> GetParametersFromUser(List<Param> i_Params)
        {
            Dictionary<string, object> paramValues = new Dictionary<string, object>();

            foreach (Param param in i_Params)
            {
                Console.Write($"Please enter {formatParamName(param.ParamName)} : ");
                object value = null;
                bool validParam = false;

                while (!validParam)
                {
                    try
                    {
                        value = getTypedParamValue(param);
                        validParam = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine("Please try again:");
                    }
                }

                Console.WriteLine();
                paramValues[param.ParamName] = value;
            }

            return paramValues;
        }

        public static void AddNewVehicle(string i_LicenseNumber)
        {
            try
            {
                Vehicle vehicle = createEmptyVehicle(i_LicenseNumber);
                List<Param> requiredParams = VehicleCreator.GetRequiredParams(vehicle);
                bool additionSuccessful = false;

                while (!additionSuccessful)
                {
                    try
                    {
                        Dictionary<string, object> paramValues = GetParametersFromUser(requiredParams);
                        string errorMessage;

                        if (VehicleCreator.UpdateVehicleWithParameters(vehicle, paramValues, out errorMessage))
                        {
                            string ownerName = getValidOwnerName();
                            string ownerPhone = getValidOwnerPhone();
                            VehicleInfo vehicleInfo = new VehicleInfo(vehicle);

                            vehicleInfo.InitializeOwnerDetails(ownerName, ownerPhone);
                            GarageManager.AddNewVehicle(vehicleInfo);
                            Console.WriteLine($"Vehicle added successfully!{Environment.NewLine}");
                            additionSuccessful = true;
                        }
                        else
                        {
                            Console.WriteLine($"Failed to update vehicle : {errorMessage}");
                            Console.WriteLine($"Please enter all parameters again{Environment.NewLine}");
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Invalid input : {ex.Message}");
                        Console.WriteLine($"Please enter all parameters again {Environment.NewLine}");
                    }
                    catch (ValueOutOfRangeException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine($"Please enter all parameters again {Environment.NewLine}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error : {ex.Message}");
                        Console.WriteLine($"Please enter all parameters again {Environment.NewLine}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating vehicle : {ex.Message}");
                throw;
            }
        }

        private static Vehicle createEmptyVehicle(string i_LicenseNumber)
        {
            eVehicleType vehicleType = (eVehicleType)Enum.GetValues(typeof(eVehicleType))
                .GetValue(GetEnumChoice(Enum.GetValues(typeof(eVehicleType)), "vehicle type"));

            return VehicleCreator.CreateVehicle(vehicleType, i_LicenseNumber);
        }

        private static object getTypedParamValue(Param i_Param)
        {
            object result;

            if (i_Param.ParamType.IsEnum)
            {
                bool validChoice = false;

                do
                {
                    result = Enum.GetValues(i_Param.ParamType).GetValue(GetEnumChoice(Enum.GetValues(i_Param.ParamType),
                             formatParamName(i_Param.ParamName)));
                    if (i_Param.AllowedEnumValue != null && !result.Equals(i_Param.AllowedEnumValue))
                    {
                        Console.WriteLine($"Invalid fuel type for this vehicle. Please choose {i_Param.AllowedEnumValue}");
                    }
                    else
                    {
                        validChoice = true;
                    }
                } while (!validChoice);
            }
            else
            {
                switch (i_Param.ParamType.Name)
                {
                    case "Single":
                        result = getValidFloatInput(i_Param.MinValue ?? float.MinValue, i_Param.MaxValue ?? float.MaxValue);
                        break;

                    case "Int32":
                        result = GetValidIntegerInput(i_Param.MinValue.HasValue ? (int)i_Param.MinValue.Value : int.MinValue,
                            i_Param.MaxValue.HasValue ? (int)i_Param.MaxValue.Value : int.MaxValue);
                        break;

                    case "String":
                        result = getValidStringInput();
                        break;

                    case "Boolean":
                        result = GetBooleanInput();
                        break;

                    default:
                        throw new ArgumentException($"Unsupported parameter type: {i_Param.ParamType}");
                }
            }

            return result;
        }

        private static string formatParamName(string i_ParamName)
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(i_ParamName))
            {
                result.Append(char.ToUpper(i_ParamName[0]));
                for (int i = 1; i < i_ParamName.Length; i++)
                {
                    if (char.IsUpper(i_ParamName[i]))
                    {
                        result.Append(' ');
                    }

                    result.Append(i_ParamName[i]);
                }
            }

            return result.ToString().Trim();
        }

        public static string HandleVehicleLicenseNumber(string i_LicenseNumber)
        {
            while (!GarageManager.IsVehicleInGarage(i_LicenseNumber))
            {
                Console.WriteLine("Vehicle not found in garage.");
                Console.WriteLine("1. Enter a different license number");
                Console.WriteLine("2. Return to main menu");
                int choice = GetValidIntegerInput(1, 2);

                if (choice == 2)
                {
                    i_LicenseNumber = null;
                    break;
                }

                i_LicenseNumber = GetUserLicenseNumber();
            }

            return i_LicenseNumber;
        }

        public static int GetValidIntegerInput(int i_MinValue, int i_MaxValue)
        {
            int result;

            while (true)
            {
                try
                {
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out result))
                    {
                        throw new FormatException("Input is not a valid integer.");
                    }

                    if (result < i_MinValue || result > i_MaxValue)
                    {
                        throw new ValueOutOfRangeException(i_MinValue, i_MaxValue, "");
                    }

                    return result;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static float getValidFloatInput(float i_MinValue, float i_MaxValue)
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();

                    if (!float.TryParse(input, out float result))
                    {
                        throw new FormatException("Input is not a valid number.");
                    }

                    if (result < i_MinValue || result > i_MaxValue)
                    {
                        throw new ValueOutOfRangeException(i_MinValue, i_MaxValue, "");
                    }

                    return result;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ValueOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static string getValidStringInput()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        throw new ArgumentException("Input cannot be empty or contain only whitespace.");
                    }

                    return input;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static bool GetBooleanInput()
        {
            Console.Write("Enter 'y' for yes or 'n' for no : ");
            string input = Console.ReadLine()?.ToLower().Trim();

            while (input != "y" && input != "n")
            {
                Console.Write("Invalid input. Please enter 'y' or 'n' : ");
                input = Console.ReadLine()?.ToLower().Trim();
            }

            Console.WriteLine();

            return input == "y";
        }

        public static int GetEnumChoice(Array i_EnumValues, string i_OptionRequired)
        {
            Console.WriteLine();
            for (int i = 0; i < i_EnumValues.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {formatParamName(i_EnumValues.GetValue(i).ToString())}");
            }

            Console.Write("Your choice: ");
            try
            {
                int choice = GetValidIntegerInput(1, i_EnumValues.Length) - 1;

                Console.WriteLine();

                return choice;
            }
            catch (ValueOutOfRangeException)
            {
                throw new ValueOutOfRangeException(1, i_EnumValues.Length, i_OptionRequired);
            }
        }
    }
}