using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager S;

    public BeatMap selectedBeatMap;

    public GameObject creditsPanel;
    public GameObject tutorialPanel;

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

    public void btn_Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void btn_Credits()
    {
        creditsPanel.SetActive(true);
    }

    public void btn_Tutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void btn_Back()
    {
        creditsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }
}
