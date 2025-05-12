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
                Dictionary<string, object?>? jsonArgs = null;
                try
                {
                    jsonArgs = JsonSerializer.Deserialize<Dictionary<string, object?>>(update.FunctionCallArguments);
                    var output = ""; // TODO: need to understand how AIFunction Invoke translates to the RealtimeConversation object.
                    return ConversationItem.CreateFunctionCallOutput(update.FunctionCallId, output?.ToString() ?? "");
                }
                catch (JsonException)
                {
                    return ConversationItem.CreateFunctionCallOutput(update.FunctionCallId, "Invalid JSON");
                }
                catch
                {
                    return ConversationItem.CreateFunctionCallOutput(update.FunctionCallId, "Error Calling Tool!");
                }
            }

            //TODO: there must be a better return object?
            return null;
        }
    }
}

