using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { setup, ready, playing, won, lost };

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    [HideInInspector] public GameState gameState;

    private int notesStreak;
    private int notesMissed;
    private int maxMissedNotes;
    private int bestStreak;
    private int totalNotesHit = 0;

    [SerializeField] private ButtonController[] buttons;

    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject clearedText;
    [SerializeField] private TextMeshProUGUI endText;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI missedNotesText;

    [SerializeField] private BeatMap defaultBeatMap;

    private float maxOffsetGood = 0.07f;
    private float maxOffsetOkay = 0.13f;

    void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    private void Start()
    {
        gameState = GameState.setup;
        Setup();
    }

    void Update()
    {
        if (gameState == GameState.playing) HandleInput();
    }

    private void Setup()
    {
        foreach (ButtonController button in buttons)
        {
            button.SetupTrack();
        }

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
        bestStreak = 0;
        endScreen.SetActive(false);
        gameState = GameState.ready;

        StartCoroutine(IntroDelay());
    }

    private IEnumerator IntroDelay()
    {
        yield return new WaitForSeconds(1f);
        StartSong();
    }

    public void StartSong()
    {
        SongManager.S.StartSong();
        gameState = GameState.playing;
    }

    private void HandleInput()
    {
        foreach (ButtonController button in buttons)
        {
            if (Input.GetKeyDown(button.keyMapping) || Input.GetKeyDown(button.altKeyMapping) || Input.GetButtonDown(button.inputButtonName))
            {
                button.Pressed();
            }
            else if (Input.GetKeyUp(button.keyMapping) || Input.GetKeyUp(button.altKeyMapping) || Input.GetButtonUp(button.inputButtonName))
            {
                button.Unpressed();
            }
        }
    }

    // Positive offset means the note was played late
    public void PlayedNote(ButtonController button, float offset)
    {
        if (offset < -maxOffsetOkay)
        {
            sfxManager.S.PlaySound(sfxManager.S.badNoteSFX);
            button.PlayBadFX(false);
            notesStreak = 0;
        }
        else if (-maxOffsetOkay < offset && offset < -maxOffsetGood)
        {
            sfxManager.S.PlaySound(sfxManager.S.okayNoteSFX);
            button.PlayOkayFX(false);
            notesStreak++;
        }
        else if (-maxOffsetGood < offset && offset < maxOffsetGood)
        {
            sfxManager.S.PlaySound(sfxManager.S.goodNoteSFX);
            button.PlayGoodFX();
            notesStreak++;
        }
        else if (maxOffsetGood< offset && offset < maxOffsetOkay)
        {
            sfxManager.S.PlaySound(sfxManager.S.okayNoteSFX);
            button.PlayOkayFX(true);
            notesStreak++;
        }
        else
        {
            sfxManager.S.PlaySound(sfxManager.S.badNoteSFX);
            button.PlayBadFX(true);
            notesStreak = 0;
        }

        totalNotesHit++;
        if (notesStreak > bestStreak) bestStreak = notesStreak;
        scoreText.text = "Streak: " + notesStreak;
    }

    public void MissedNote()
    {
        notesStreak = 0;
        scoreText.text = "Streak: " + notesStreak;
        notesMissed++;
        missedNotesText.text = "Missed: " + notesMissed + " / " + maxMissedNotes;
        sfxManager.S.PlaySound(sfxManager.S.missedNoteSFX);

        if (notesMissed == maxMissedNotes)
        {
            GameLost();
        }
    }

    public void ClearedSong()
    {
        gameState = GameState.won;
        SongManager.S.StopSong();
        sfxManager.S.PlaySound(sfxManager.S.winSound);
        endText.text = "Notes: " + totalNotesHit + " / " + SongManager.S.totalNotes + "\n" + "Best Streak: " + bestStreak;
        clearedText.SetActive(true);
        endScreen.SetActive(true);
    }

    private void GameLost()
    {
        gameState = GameState.lost;
        SongManager.S.StopSong();
        sfxManager.S.PlaySound(sfxManager.S.loseSound);
        endText.text = "Notes: " + totalNotesHit + " / " + SongManager.S.totalNotes + "\n" + "Best Streak: " + bestStreak;
        clearedText.SetActive(false);
        endScreen.SetActive(true);
    }

    public void btn_Reset()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        MenuManager.S.btn_LoadMain(MenuManager.S.selectedBeatMap);
    }

    public void btn_Menu()
    {
        sfxManager.S.PlaySound(sfxManager.S.uiClick);
        MenuManager.S.btn_Menu();
    }
}
