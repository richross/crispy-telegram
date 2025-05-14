using OpenAI.RealtimeConversation;
using System.Collections.Generic;

namespace CallAutomationOpenAI;

#pragma warning disable OPENAI002
public class Capabilities
{
    public List<ConversationFunctionTool> AvailableTools { get; set; } = new List<ConversationFunctionTool>();

    public Capabilities()
    {
        AvailableTools.Add(new ConversationFunctionTool
        {
            Name = "AccountInfo",
            Description = "Find the account information for the caller",
            Parameters = BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "action": {
                            "type": "string",
                            "description": "The action to perform (e.g., 'get balance', 'get Last Transaction')"
                        },
                        "parameters": {
                            "type": "object",
                            "description": "The parameters for the action"
                        }
                    },
                    "required": ["action"]
                }
                """)
        });

        AvailableTools.Add(new ConversationFunctionTool
        {
            Name = "GetStockQuote",
            Description = "Get the stock quote for a given stock symbol",
            Parameters = BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "symbol": {
                            "type": "string",
                            "description": "The stock symbol to look up"
                        }
                    },
                    "required": ["symbol"]
                }
                """)
        });
    }
}
