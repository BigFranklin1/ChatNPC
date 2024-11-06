using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionUI; // UI提示的GameObject
    // public GameObject NPCChatManager;
    public Transform playerCamera;   // 玩家摄像机的Transform
    public float interactionAngle = 30f; // 允许交互的最大夹角
    public int dialogueLeft;
    public GameObject npc;
    public string fixedQuestions = "";

    private bool isEnterDialogue = false;
    private bool isPrepareEndDialog = false;
    private bool isQuitButtonPressed = false;
    private bool isTheLastQuestionAnswered = false;

    public void StartDialogue()
    {
        isEnterDialogue = true;
        interactionUI.SetActive(false); // 隐藏提示UI
        DialogueManager.Instance.StartDialogue(npc);

    }

    public IEnumerator BeforeEndDialogue()
    {
        // wait for 5s
        yield return new WaitForSeconds(5);
        // NPCChatManager.UpdateSystemMessage("the player is leaving, you should ask him some questions");
        transform.GetComponent<ChatGPTCharacter>().AskFixedQuestion(fixedQuestions);
        // isQuitButtonPressed = true;
    }
    public void EndDialogue()
    {
        isEnterDialogue = false;
        interactionUI.SetActive(true); // 显示提示UI
        DialogueManager.Instance.EndDialogue();

    }
    void Start()
    {
        interactionUI.SetActive(false); // 初始状态隐藏提示UI
        isQuitButtonPressed = false;
        isPrepareEndDialog = false;
        isTheLastQuestionAnswered = false;
        isEnterDialogue = false;
    }

    public void SetisTheLastQuestionAnswered(bool var)
    {
        if(isQuitButtonPressed){
            isTheLastQuestionAnswered = var;
        }
    }


    void Update()
    {

        // 计算玩家视角与NPC方向的夹角
        Vector3 directionToNPC = (transform.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, directionToNPC);

        // 当玩家朝向NPC并且在允许角度内时，显示提示UI
        if (angle <= interactionAngle && !isEnterDialogue)
        {
            // Debug.Log("show UI");
            interactionUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue(); // 玩家按下E键，开始对话
            }
        }
        else
        {
            // Debug.Log("hide UI");
            interactionUI.SetActive(false); // 玩家未朝向NPC时隐藏提示UI
        }

        // End Dialogue Logic related to dialogueLeft
        if(dialogueLeft == 1 && !isPrepareEndDialog && isEnterDialogue) 
        {
            Debug.Log("counter now is 1:");
            StartCoroutine(BeforeEndDialogue());
            isPrepareEndDialog = true;
        }
        if(dialogueLeft == 0 && isEnterDialogue) 
        {
            EndDialogue();
        }

        // Check whether should end dialogue
        // condition 1: 玩家点了离开对话的按钮
        // condition 2: 玩家回答了NPC最后问出来的这几个问题
        if(isQuitButtonPressed && isTheLastQuestionAnswered && isEnterDialogue)
        {
            EndDialogue();
        }
    }



}