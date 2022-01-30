using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { setup, ready, playing };

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    [HideInInspector] public GameState gameState;
    private int notesStreak;
    private int notesMissed;
    private int maxMissedNotes;

    [SerializeField] private ButtonController[] buttons;

    [SerializeField] private GameObject startButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI missedNotesText;

    [SerializeField] private BeatMap defaultBeatMap;

    [Header("Score Paremeters")]
    [SerializeField] private float maxOffsetGood = 0.06f;
    [SerializeField] private float maxOffsetOkay = 0.13f;

    void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    private void Start()
    {
        gameState = GameState.setup;

        if (MenuManager.S)
        {
            SongManager.S.SetupBeatMap(MenuManager.S.selectedBeatMap);
            maxMissedNotes = MenuManager.S.selectedBeatMap.maxNotesMissed;
        }
        else
        {
            SongManager.S.SetupBeatMap(defaultBeatMap);
            maxMissedNotes = defaultBeatMap.maxNotesMissed;
        }
        scoreText.text = "Streak: 0";
        missedNotesText.text = "Missed: 0 / " + maxMissedNotes;
        notesStreak = 0;
        notesMissed = 0;

        foreach (ButtonController button in buttons)
        {
            button.SetupTrack();
        }
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

    // Positive offset means the note was played late
    public void PlayedNote(ButtonController button, float offset)
    {
        
        if (offset < -maxOffsetOkay)
        {
            print("Bad: early");
            sfxManager.S.PlaySoundWithRandomizedPitch(sfxManager.S.badNoteSFX);
            notesStreak = 0;
        }
        else if (-maxOffsetOkay < offset && offset < -maxOffsetGood)
        {
            print("Okay: early");
            sfxManager.S.PlaySoundWithRandomizedPitch(sfxManager.S.okayNoteSFX);
            notesStreak++;
        }
        else if (-maxOffsetGood < offset && offset < maxOffsetGood)
        {
            print("Good!");
            sfxManager.S.PlaySoundWithRandomizedPitch(sfxManager.S.goodNoteSFX);
            button.PlayParticles();
            notesStreak++;
        }
        else if (maxOffsetGood< offset && offset < maxOffsetOkay)
        {
            print("Okay: late");
            sfxManager.S.PlaySoundWithRandomizedPitch(sfxManager.S.okayNoteSFX);
            notesStreak++;
        }
        else
        {
            print("Bad: late");
            sfxManager.S.PlaySoundWithRandomizedPitch(sfxManager.S.badNoteSFX);
            notesStreak = 0;
        }

        scoreText.text = "Streak: " + notesStreak;
    }

    public void MissedNote()
    {
        notesStreak = 0;
        scoreText.text = "Streak: " + notesStreak;
        notesMissed++;
        missedNotesText.text = "Notes Missed: " + notesMissed + " / " + maxMissedNotes;
        sfxManager.S.PlaySoundWithRandomizedPitch(sfxManager.S.missedNoteSFX);
    }
}
