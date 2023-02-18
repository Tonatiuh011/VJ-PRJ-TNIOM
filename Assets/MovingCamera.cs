using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    public GameObject target;
    public float distanceFromTarget = 15f;
    public CinemachineVirtualCameraBase initial;
    public CinemachineVirtualCameraBase switchTo;

    private CinemachineBrain brain;

    // Start is called before the first frame update
    void Start()
    {
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SwitchCam(CinemachineVirtualCameraBase vcam)
    {
        if(brain == null || vcam == null)
            return;
        // Check If camera is already set down
        if (brain.ActiveVirtualCamera != (ICinemachineCamera)vcam)
            vcam.MoveToTopOfPrioritySubqueue();
    }
}
