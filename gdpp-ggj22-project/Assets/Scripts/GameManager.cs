using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { ready, playing };

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public GameState gameState;
    private int notesPlayed;
    private int notesMissed;

    [SerializeField] private ButtonController[] buttons;

    [SerializeField] private GameObject startButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI missedNotesText;

    void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    private void Start()
    {
        gameState = GameState.ready;
        scoreText.text = "Notes: 0";
        missedNotesText.text = "Misssed: 0";
        notesPlayed = 0;
        notesMissed = 0;
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
