using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordButtonSwitch : MonoBehaviour
{
    public GameObject startBtn;
    public GameObject stopBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Switch()
    {
        Debug.Log("clicked");
        if (startBtn.activeSelf)
        {
            startBtn.SetActive(false);
            stopBtn.SetActive(true);
        }
        else
        {
            startBtn.SetActive(true);
            stopBtn.SetActive(false);
        }
    }
}
