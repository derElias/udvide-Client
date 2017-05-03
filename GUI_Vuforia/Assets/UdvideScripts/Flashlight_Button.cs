using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Flashlight_Button : MonoBehaviour {
    private bool mFlashEnabled = false;

    public void flashlight_on() {

            if (!mFlashEnabled)
            {
                // Turn on flash if it is currently disabled.
                CameraDevice.Instance.SetFlashTorchMode(true);
                mFlashEnabled = true;
            }
            else
            {
                // Turn off flash if it is currently enabled.
                CameraDevice.Instance.SetFlashTorchMode(false);
                mFlashEnabled = false;
            }

        }

    }

