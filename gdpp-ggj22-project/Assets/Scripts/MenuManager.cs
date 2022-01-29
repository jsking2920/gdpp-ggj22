using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager S;

    public BeatMap selectedBeatMap;

    private void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;

        DontDestroyOnLoad(this);
    }

    public void btn_LoadMain(BeatMap map)
    {
        selectedBeatMap = map;
        SceneManager.LoadScene("Main");
    }

    public void btn_Quit()
    {
        Application.Quit();
    }
}
