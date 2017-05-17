/*using UnityEngine;
using System.Collections;
using System;
using Vuforia;

public class CameraFocusMode : MonoBehaviour
{
    void Start()
    {



        VuforiaBehaviour.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);

        VuforiaBehaviour.Instance.RegisterOnPauseCallback(OnPaused);
    }



    private void OnVuforiaStarted()

    {

        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);



        Debug.Log("Focus ON");

    }
    private void OnPaused(bool paused)

    {

        if (!paused) // resumed

        {

            // Set again autofocus mode when app is resumed

            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

            

        }

    }
}*/