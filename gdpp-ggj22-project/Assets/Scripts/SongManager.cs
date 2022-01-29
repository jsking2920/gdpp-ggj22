using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on: https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games (Yu Chao)
public class SongManager : MonoBehaviour
{
    public static SongManager S;

    [SerializeField] private float bpm;
    [SerializeField] private AudioSource songSource;
    [SerializeField] private GameObject notePrefab;

    public float[] track1Notes;
    public float[] track2Notes;

    private float songPosInSecs;
    [HideInInspector] public float songPosInBeats;
    private float secPerBeat;
    private float songStartTime;
    
    private int numNotes1;
    private int numNotes2;
    private int nextIndex1 = 0;
    private int nextIndex2 = 0;
    public float notesShownInAdvance;

    public Transform track1Start;
    public Transform track1End;
    public Transform track2Start;
    public Transform track2End;

    private bool songPlaying = false;

    private void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    void Start()
    {
        numNotes1 = track1Notes.Length;
        numNotes2 = track2Notes.Length;
        secPerBeat = 60f / bpm;
    }

    void Update()
    {
        if (songPlaying)
        {
            UpdatePosition();
            if (nextIndex1 < numNotes1 && track1Notes[nextIndex1] < songPosInBeats + notesShownInAdvance) 
                SpawnNote(track1Notes, track1Start, track1End, ref nextIndex1);
            if (nextIndex2 < numNotes2 && track2Notes[nextIndex2] < songPosInBeats + notesShownInAdvance)
                SpawnNote(track2Notes, track2Start, track2End, ref nextIndex2);
        }
    }

    public void StartSong()
    {
        songStartTime = (float)AudioSettings.dspTime;
        songSource.Play();
        songPlaying = true;
    }

    private void UpdatePosition()
    {
        songPosInSecs = (float)(AudioSettings.dspTime - songStartTime);
        songPosInBeats = songPosInSecs / secPerBeat;
    }

    private void SpawnNote(float[] notes, Transform start, Transform end, ref int index)
    {
        MusicNote note = Instantiate(notePrefab, track1Start.position, Quaternion.identity).GetComponent<MusicNote>();
        note.beatOfThisNote = notes[index];
        note.startPos = start.position;
        note.endPos = end.position;
        index++;
    }
}
