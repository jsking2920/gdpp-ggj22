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
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        SceneManager.LoadScene("Main");
    }

    public void btn_Quit()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        Application.Quit();
    }

    public void btn_Menu()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        SceneManager.LoadScene("Menu");
    }

    public void btn_Credits()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        creditsPanel.SetActive(true);
    }

    public void btn_Tutorial()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        tutorialPanel.SetActive(true);
    }

    public void btn_Back()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        creditsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }
}
