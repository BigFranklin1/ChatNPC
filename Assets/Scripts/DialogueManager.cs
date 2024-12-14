using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class DialogueManager : MonoBehaviour
{
    public GameObject dialogUI;
    public TMP_Text responseText; 
    public TMP_Text chatText; 
    public Button sendBtn; 
    public Button quitBtn;
    public TMP_InputField systemInputField;
    public TMP_InputField userInputField;

    public bool gptResponsed;

    public static DialogueManager Instance { get; private set; }

    private GameObject currentNPC;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void UpdateResponseUI(string message)
    {
        Debug.Log("response message:" + message);
        // 可以在这里添加打字机效果
        responseText.text = message;
        // 或者使用协程实现打字机效果
        StartCoroutine(TypewriterEffect(message));
    }
    // 可选：打字机效果
    private IEnumerator TypewriterEffect(string message)
    {
        responseText.text = "";
        foreach (char c in message)
        {
            responseText.text += c;
            yield return new WaitForSeconds(0.01f);
        }
    }
    // 开始与NPC对话
    public void StartDialogue(GameObject npc)
    {
        // 初始化对话UI逻辑
        currentNPC = npc;
        Debug.Log("开始和 " + currentNPC + " 对话");
        
        dialogUI.SetActive(true);
        currentNPC.GetComponent<ChatGPTCharacter>().onResponse.AddListener(UpdateResponseUI);
        currentNPC.GetComponent<ChatGPTCharacter>().onFixedResponse.AddListener(UpdateResponseUI);

        sendBtn.onClick.AddListener(TaskOnSendBtnClick);
        quitBtn.onClick.AddListener(TaskOnQuitBtnClick);
        systemInputField.onEndEdit.AddListener(OnSystemInputFieldEndEdit); // 新增监听
        userInputField.onEndEdit.AddListener(TaskOnSendBtnClick);
        Cursor.lockState = CursorLockMode.None;

    }

    async void TaskOnSendBtnClick()
    {
        gptResponsed = false;
        Debug.Log("Send Btn clicked");
        currentNPC.GetComponent<ChatGPTCharacter>().UpdateUserInputMessage(chatText.text);
        // currentNPC.GetComponent<ChatGPTCharacter>().AskChatGPT();
        gptResponsed = await currentNPC.GetComponent<ChatGPTCharacter>().AskChatGPT();

    }
    async void TaskOnSendBtnClick(string txt)
    {
        gptResponsed = false;
        Debug.Log("Send Btn clicked");
        currentNPC.GetComponent<ChatGPTCharacter>().UpdateUserInputMessage(txt);
        // currentNPC.GetComponent<ChatGPTCharacter>().AskChatGPT();
        gptResponsed = await currentNPC.GetComponent<ChatGPTCharacter>().AskChatGPT();

    }
    void TaskOnQuitBtnClick()
    {
        currentNPC.GetComponent<NPCInteraction>().EndDialogue();
    }
    public void OnSystemInputFieldEndEdit(string systemMsg)
    {
        currentNPC.GetComponent<ChatGPTCharacter>().UpdateSystemMessage(systemMsg);
    }
    public void EndDialogue()
    {
        Debug.Log("结束和 " + currentNPC + " 对话");
        // currentNPC = none;
        // 初始化对话UI逻辑
        dialogUI.SetActive(false);
        sendBtn.onClick.RemoveAllListeners();
        quitBtn.onClick.RemoveAllListeners();

        currentNPC.GetComponent<ChatGPTCharacter>().onResponse.RemoveAllListeners();
        currentNPC.GetComponent<ChatGPTCharacter>().onFixedResponse.RemoveAllListeners();

        systemInputField.onEndEdit.RemoveAllListeners(); // 移除监听
        userInputField.onEndEdit.RemoveAllListeners();

        Cursor.lockState = CursorLockMode.Locked;
        responseText.text = "";
        chatText.text = "";
    }

    // public void PrepareEndDialogue()
    // {
    //     Debug.Log("准备结束和 " + currentNPC + " 对话");
    //     currentNPC.GetComponent<ChatGPTCharacter>().AskFixedQuestion(fixedQuestions);
    // }

    // Start is called before the first frame update
    void Start()
    {
        dialogUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
