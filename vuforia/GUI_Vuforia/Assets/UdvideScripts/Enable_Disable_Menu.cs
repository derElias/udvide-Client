using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable_Disable_Menu : MonoBehaviour {
    public GameObject Enable_Disable;
   public void PopUp()
    {
        if(!Enable_Disable.activeInHierarchy)
        {
            Enable_Disable.SetActive(true);
        }
        else {
            Enable_Disable.SetActive(false);
                }
    }
}