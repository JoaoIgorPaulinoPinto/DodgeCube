using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorller : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        transform.LookAt(PlayerMovimentation.instance.transform);
    }
}
