using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace CallAutomationOpenAI.Handlers
{
    /// <summary>
    /// Implements the actual function logic for function calls from the OpenAI Realtime API
    /// </summary>
    public static class FunctionImplementations
    {
        // HttpClient should typically be a static singleton in production
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Handles the AccountInfo function call
        /// </summary>
        /// <param name="args">The function arguments from the AI model</param>
        /// <returns>A response string to be returned to the AI model</returns>
        public static async Task<string> HandleAccountInfoAsync(JsonElement args)
        {
            try
            {
                // Extract the action from the arguments
                string action = args.GetProperty("action").GetString() ?? "get balance";
                
                // This would normally query a database or other service
                // Simulated account information
                if (action.ToLower().Contains("balance"))
                {
                    return "Your account balance is $12,345.67";
                }
                else if (action.ToLower().Contains("transaction"))
                {
                    return "Your last transaction was a deposit of $500.00 on May 10th, 2025";
                }
                else
                {
                    return "I couldn't find information for that action. Please try asking about your balance or last transaction.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing AccountInfo function: {ex.Message}");
                return "I encountered an issue accessing your account information. Please try again later.";
            }
        }

        /// <summary>
        /// Handles the GetStockQuote function call
        /// </summary>
        /// <param name="args">The function arguments from the AI model</param>
        /// <returns>A response string to be returned to the AI model</returns>
        public static async Task<string> HandleGetStockQuoteAsync(JsonElement args)
        {
            try
            {
                // Extract the stock symbol from the arguments
                string symbol = args.GetProperty("symbol").GetString() ?? string.Empty;
                
                if (string.IsNullOrWhiteSpace(symbol))
                {
                    return "No stock symbol was provided. Please specify a valid stock symbol.";
                }

                // In a real implementation, you would call a financial API
                // For demonstration purposes, we'll simulate an API call
                await Task.Delay(500); // Simulate network latency
                
                // Simulated stock data
                // In production, this would be replaced with a real API call
                var random = new Random();
                double price = Math.Round(random.NextDouble() * 1000, 2);
                double change = Math.Round((random.NextDouble() * 10) - 5, 2);
                
                return $"Current quote for {symbol.ToUpper()}: ${price:F2}, Change: {change:+0.00;-0.00}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing GetStockQuote function: {ex.Message}");
                return "I encountered an issue retrieving the stock quote. Please try again later.";
            }
        }

        /// <summary>
        /// Routes function calls to the appropriate handler based on function name
        /// </summary>
        /// <param name="functionName">The name of the function to call</param>
        /// <param name="argsJson">The JSON arguments as a string</param>
        /// <returns>The function output as a string</returns>
        public static async Task<string> DispatchFunctionCallAsync(string functionName, string argsJson)
        {
            try
            {
                // Deserialize the arguments
                JsonElement args = JsonSerializer.Deserialize<JsonElement>(argsJson);
                
                // Dispatch to the appropriate function handler
                switch (functionName)
                {
                    case "AccountInfo":
                        return await HandleAccountInfoAsync(args);
                    
                    case "GetStockQuote":
                        return await HandleGetStockQuoteAsync(args);
                    
                    default:
                        return $"Function {functionName} is not implemented.";
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing function arguments: {ex.Message}");
                return "Invalid function arguments format.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error dispatching function call: {ex.Message}");
                return "An error occurred while processing your request.";
            }
        }
    }
}
