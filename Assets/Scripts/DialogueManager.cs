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

    // 开始与NPC对话
    public void StartDialogue(GameObject npc)
    {
        // 初始化对话UI逻辑
        currentNPC = npc;
        Debug.Log("开始和 " + currentNPC + " 对话");
        
        dialogUI.SetActive(true); 

        sendBtn.onClick.AddListener(TaskOnSendBtnClick);
        quitBtn.onClick.AddListener(TaskOnQuitBtnClick);
        systemInputField.onEndEdit.AddListener(OnSystemInputFieldEndEdit); // 新增监听

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
        systemInputField.onEndEdit.RemoveListener(OnSystemInputFieldEndEdit); // 移除监听

        Cursor.lockState = CursorLockMode.Locked;
        responseText.text = "";
        Debug.Log("chatText: " + chatText.text);
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
