using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {

    public Transform Target;

    [SerializeField]
    private Camera minimapCamera;
    private bool isMinimapOpen = true;
    

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if (!isMinimapOpen)
            {
                minimapCamera.enabled = true;
                isMinimapOpen = !isMinimapOpen;
            }
            else
            {
                minimapCamera.enabled = false;
                isMinimapOpen = !isMinimapOpen;
            }
        }

        transform.position = new Vector3(Target.position.x, transform.position.y, Target.position.z);    
    }
    
}
