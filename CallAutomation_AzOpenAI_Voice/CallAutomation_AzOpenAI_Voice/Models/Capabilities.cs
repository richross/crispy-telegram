using OpenAI.RealtimeConversation;
using System.Collections.Generic;

namespace CallAutomationOpenAI;

#pragma warning disable OPENAI002
public class Capabilities
{
    public List<ConversationTool> AvailableTools { get; set; } = new List<ConversationTool>();

    public Capabilities()
    {
        AvailableTools.Add(new ConversationFunctionTool
        {
            Name = "CallControl",
            Description = "Call Control API",
            Parameters = BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "action": {
                            "type": "string",
                            "description": "The action to perform"
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
    }
}
