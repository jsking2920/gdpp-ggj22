using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BeatMapRecorder : MonoBehaviour
{
    public static BeatMapRecorder S;

    // If you want to record over an existing beat map, reference it here (DESTRUCTIVE)
    [Header("Edit Recording")]
    [SerializeField][Tooltip("Record over this beat mep destructively")] BeatMap targetBeatMap;

    [Header("Create Recording")]
    public string songName;
    public int bpm;
    public AudioClip song;
    // Reference to each track you want to be recorded
    public ButtonController[] buttons;

    public bool quantizeEigth;
    public bool quantizeTriplet;

    private void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!targetBeatMap)
        {
            targetBeatMap = ScriptableObject.CreateInstance<BeatMap>();
            AssetDatabase.CreateAsset(targetBeatMap, "Assets/BeatMaps/" + songName + "Map.asset");
            AssetDatabase.SaveAssets();
        }
        targetBeatMap.song = song;
        targetBeatMap.bpm = bpm;
    }

    void Update()
    {
        if (GameManager.S.gameState == GameState.playing)
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
                RecordInput(SongManager.S.songPosInBeats, button.trackID);
            }
        }
    }

    public void RecordInput(float beat, int trackNumber)
    {
        switch (trackNumber)
        {
            case 1:
                targetBeatMap.track1Notes.Add(Quantize(beat));
                break;
            case 2:
                targetBeatMap.track2Notes.Add(Quantize(beat));
                break;
        }
    }

    private float Quantize(float rawInputBeat)
    {
        if (quantizeEigth)
        {
            return Mathf.Round(rawInputBeat * 2f) * 0.5f;
        }
        if (quantizeTriplet)
        {
            return Mathf.Round(rawInputBeat * 3f) * (1f / 3f);
        }
        else
        {
            return rawInputBeat;
        }
    }
}
