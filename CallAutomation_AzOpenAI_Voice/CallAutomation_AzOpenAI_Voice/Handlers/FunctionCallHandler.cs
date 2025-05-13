using OpenAI.RealtimeConversation;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;


namespace CallAutomationOpenAI.Handlers
{
    public static class FunctionCallOutput
    {
        [Experimental("OPENAI002")]
        public static async Task<ConversationItem?> GetFunctionCallOutputAsync(
            this ConversationItemStreamingFinishedUpdate update, IReadOnlyList<ConversationFunctionTool> tools)
        {
            if (!string.IsNullOrEmpty(update.FunctionName) && tools.FirstOrDefault(t => t.Name == update.FunctionName) is ConversationFunctionTool functionTool)
            {
                try
                {
                    // Dispatch the function call to our implementation
                    string output = await FunctionImplementations.DispatchFunctionCallAsync(
                        update.FunctionName,
                        update.FunctionCallArguments);
                    
                    Console.WriteLine($"Function {update.FunctionName} returned: {output}");
                    
                    // Return the function call output to be sent back to the model
                    return ConversationItem.CreateFunctionCallOutput(update.FunctionCallId, output);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Invalid JSON in function arguments: {ex.Message}");
                    return ConversationItem.CreateFunctionCallOutput(update.FunctionCallId, "Invalid JSON in function arguments");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing function {update.FunctionName}: {ex.Message}");
                    return ConversationItem.CreateFunctionCallOutput(update.FunctionCallId, "Error executing function");
                }
            }

            // No function call or matching tool found
            return null;
        }
    }
}

