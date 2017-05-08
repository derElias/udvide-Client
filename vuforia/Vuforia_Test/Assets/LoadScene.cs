using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void NextLevelButton(int index)
    {
        SceneManager.LoadScene("AR");
    }

    public void PreviousLevelButton(int levelName )
    {
        SceneManager.LoadScene("MainMenu");
    }
}