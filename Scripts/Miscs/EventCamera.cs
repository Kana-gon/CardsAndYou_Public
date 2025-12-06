using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioListener mainCameraAudio;
    [SerializeField] private Camera eventCamera;
    [SerializeField] private AudioListener eventCameraAudio;

    void Start()
    {
        eventCamera.enabled = false;
        eventCameraAudio.enabled = false;
    }
    public void eventStart()
    {
        eventCamera.enabled = true;
        mainCamera.enabled = false;
        eventCameraAudio.enabled = true;
        mainCameraAudio.enabled = false;
    }
    public void eventEnd()
    {
        eventCamera.enabled = false;
        mainCamera.enabled = true;
        eventCameraAudio.enabled = false;
        mainCameraAudio.enabled = true;
    }
    
}
