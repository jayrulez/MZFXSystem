using MZ.FXSystem.Services.FX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MZ.FXSystem
{
    public class FXApplication
    {
        private readonly FxService _fxService;
        private bool _quitRequested = false;

        public FXApplication(FxService fxService)
        {
            _fxService = fxService;
        }

        /// <summary>
        /// Utility method which prompts the user to select an option from a list of given options
        /// </summary>
        /// <param name="prompt">The prompt message to display to the user</param>
        /// <param name="options">The list of valid options</param>
        /// <param name="invalidSelectionMessage">The message to display if an invalid selection is made</param>
        /// <param name="caseSensitiveCompare">Whether or not the check for options is case sensitive</param>
        /// <returns></returns>
        private string GetOption(string prompt, List<string> options, string invalidSelectionMessage = null, bool caseSensitiveCompare = false)
        {
            Console.Write(prompt);
            string input;
            bool validInput = false;

            do
            {
                input = Console.ReadLine().Trim();

                if (!options.Any(o => o.Equals(input, caseSensitiveCompare ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)))
                    Console.Write(invalidSelectionMessage ?? "Invalid option selected. Select an option: ");
                else
                    validInput = true;

            } while (!validInput);

            return input;
        }

        /// <summary>
        /// Displays the main menu
        /// </summary>
        /// <returns></returns>
        private string MainMenu()
        {
            Console.WriteLine("1. FX Conversion");
            Console.WriteLine("2. Quit");

            return GetOption("Select an option: ", new List<string> { "1", "2" }, "Invalid option selected. Select an option: ");
        }

        /// <summary>
        /// Displays the conversion menu
        /// </summary>
        /// <returns></returns>
        private async Task ConversionMenu()
        {
            Console.WriteLine();
            Console.WriteLine("**********************************FX Converter**********************************");

            #region Currency input
            var supportedCurrencies = _fxService.GetSupportedCurrencies().ToList();

            Console.WriteLine();

            var sourceCurrencyCode = GetOption($"Available options: {string.Join(", ", supportedCurrencies)}\nSelect the currency to convert from: ", 
                supportedCurrencies, "Invalid selection. Select a currency from the available options: ")
                .ToUpper();

            // This ensures that source and target currencies cannot be the same
            // The lower level actually handles this but let us just prevent it at the UI level
            supportedCurrencies.Remove(sourceCurrencyCode);

            Console.WriteLine();
            var targetCurrencyCode = GetOption($"Available options: {string.Join(", ", supportedCurrencies)}\nSelect the currency to convert to: ", 
                supportedCurrencies, "Invalid selection. Select a currency from the available options: ")
                .ToUpper();
            #endregion

            #region Amount input
            bool validInput = false;
            decimal amount;

            Console.Write($"\nEnter the amount to convert ({sourceCurrencyCode}): ");

            do
            {
                var amountInput = Console.ReadLine();

                if (!decimal.TryParse(amountInput, out amount))
                {
                    Console.Write("Invalid amount entered. Enter a valid decimal: ");
                }
                else
                    validInput = true;

            } while (!validInput);
            #endregion

            #region Date input
            validInput = false;
            string conversionDate = DateTime.Now.ToString(_fxService.DateFormat);

            Console.WriteLine($"\nConversion date: {conversionDate}");
            Console.Write($"Press [Enter] to use conversion date above or input date({_fxService.DateFormat.ToUpper()}): ");

            do
            {
                var dateInput = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(dateInput))
                {
                    // No new date was entered, we can break here.
                    break;
                }
                else
                {
                    // Something was entered, let us try to validate it.

                    if (!_fxService.ValidateDate(dateInput))
                        Console.Write($"Invalid conversion date '{dateInput}' entered. Enter a valid date({_fxService.DateFormat.ToUpper()}): ");
                    else
                    {
                        conversionDate = dateInput;
                        validInput = true;
                    }
                }

            } while (!validInput);
            #endregion

            var response = await _fxService.Convert(sourceCurrencyCode, targetCurrencyCode, amount, conversionDate);

            Console.WriteLine($"\nConversion result:");
            if (response.IsSuccessful)
            {
                if (!response.Data.Direct)
                {
                    Console.WriteLine($"No direct pairing between '{sourceCurrencyCode}' and '{targetCurrencyCode}' is available. An indirect conversion was performed.");
                }

                Console.WriteLine(response.Data.ToString());
            }
            else
                Console.WriteLine(response.Error.ErrorMessage);
            Console.WriteLine();
        }

        /// <summary>
        /// Runs the main loop
        /// </summary>
        /// <returns></returns>
        public async Task Run()
        {
            Console.WriteLine("------------------------------FX Conversion System------------------------------");

            while (!_quitRequested)
            {
                var option = MainMenu();

                switch (option)
                {
                    case "1":
                        await ConversionMenu();
                        break;

                    case "2":
                        if (GetOption("Are you sure you want to quit the program? (Y/N): ", new List<string> { "Y", "N" }).Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            _quitRequested = true;
                        }
                        break;
                }
            }

            Console.WriteLine("\nExiting the program.\nPress any key to continue...");

            Console.ReadKey();
        }
    }
}
