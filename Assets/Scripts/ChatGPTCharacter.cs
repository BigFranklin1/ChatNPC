using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;
using System.Threading.Tasks;
using System;

public class ChatGPTCharacter : MonoBehaviour
{
    public NPCInteraction NPCInteraction;
    public OnResponseEvent onResponse;
    public OnResponseEvent onFixedResponse;
    public bool responsed = false;
    public string initialPrompt;
    [System.Serializable]
    public class OnResponseEvent: UnityEvent<string> {}

    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();
    int counter;
    public async void UpdateUserInputMessage(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";
        messages.Add(newMessage);
    }
   
    public async Task<bool> AskChatGPT()
    {
        foreach(ChatMessage m in messages){
            Debug.Log(m.Content);
        }
        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4o-mini";

        try
        {
            var response = await openAI.CreateChatCompletion(request);
            if(response.Choices != null && response.Choices.Count > 0)
            {
                var chatResponse = response.Choices[0].Message;
                messages.Add(chatResponse);
                Debug.Log(chatResponse.Content);
                onResponse.Invoke(chatResponse.Content);

                // decrease counter
                Debug.Log("dialogueLeft--:");
                counter = NPCInteraction.GetComponent<NPCInteraction>().dialogueLeft--;
                Debug.Log("dialogueLeft:" + NPCInteraction.GetComponent<NPCInteraction>().dialogueLeft);

                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in AskChatGPT: {e.Message}");
        }

        return false;
    }
    public async void AskFixedQuestion(string text)
    {
        responsed = false;
        Debug.Log("fixed questions: "+ text);
        onFixedResponse.Invoke(text);
        responsed = true;
        
    }   

    public async void UpdateSystemMessage(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "system";
        messages.Add(newMessage);
    } 

    // Start is called before the first frame update
    void Start()
    {
        Initialize(initialPrompt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearChatHistory()
    {
        messages.Clear();
    }

    // Initialize the NPC with a starting prompt
    public void Initialize(string startingPrompt) 
    {
        ChatMessage initialMessage = new ChatMessage();
        initialMessage.Content = startingPrompt;
        initialMessage.Role = "system";
        messages.Add(initialMessage);
    }
}
