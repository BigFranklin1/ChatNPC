using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour
{
    public Transform playerCamera;   // 玩家摄像机的Transform

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerCamera.rotation;        
    }
}
