using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneName;

    public void btn_LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void btn_Quit()
    {
        Application.Quit();
    }
}
