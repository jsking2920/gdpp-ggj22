using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { setup, ready, playing };

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    [HideInInspector] public GameState gameState;
    private int notesPlayed;
    private int notesMissed;

    [SerializeField] private ButtonController[] buttons;

    [SerializeField] private GameObject startButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI missedNotesText;

    [SerializeField] private BeatMap defaultBeatMap;

    void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    private void Start()
    {
        gameState = GameState.setup;
        scoreText.text = "Notes: 0";
        missedNotesText.text = "Notes Missed: 0";
        notesPlayed = 0;
        notesMissed = 0;
        if (MenuManager.S) SongManager.S.SetupBeatMap(MenuManager.S.selectedBeatMap);
        else SongManager.S.SetupBeatMap(defaultBeatMap);
        gameState = GameState.ready;
    }

    void Update()
    {
        if (gameState == GameState.playing)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        foreach (ButtonController button in buttons)
        {
            if (Input.GetKeyDown(button.keyMapping))
            {
                button.Pressed();
            }
            if (Input.GetKeyUp(button.keyMapping))
            {
                button.Unpressed();
            }
        }
    }

    public void btn_StartSong()
    {
        SongManager.S.StartSong();
        gameState = GameState.playing;
        startButton.SetActive(false);
    }

    public void PlayedNote()
    {
        notesPlayed++;
        scoreText.text = "Notes: " + notesPlayed;
    }

    public void MissedNote()
    {
        notesMissed++;
        missedNotesText.text = "Notes Missed: " + notesMissed;
    }
}
