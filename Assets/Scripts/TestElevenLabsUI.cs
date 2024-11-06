using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestElevenLabsUI : MonoBehaviour
{
    
    public Button sendButton;
    // public InputField inputField;
    public TMP_Text gptResponseText;
    //public Text gptResponseText;
    public ElevenlabsAPI tts;
    public DialogueManager dm;
    private bool gptResponsed = false;
    void Start()
    {
        // Add the PlayClip handler to the ElevenLabsAPI script
        tts.AudioReceived.AddListener(PlayClip);
        
        // Add the Button's onClick handler 
        sendButton.onClick.AddListener( () => 
        {
            gptResponsed = false;
            StartCoroutine(WaitForGPTResponse());

        });

    }
    IEnumerator WaitForGPTResponse()
    {
        // 等待直到输入框有文字
        yield return new WaitUntil(() => gptResponsed);
        
        // 当有文字时执行方法
        tts.GetAudio(gptResponseText.text);
    }
    public void PlayClip(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    void Update()
    {
        // 根据外部信息，重置gptresponse bool值
        gptResponsed = dm.gptResponsed;
    }
}