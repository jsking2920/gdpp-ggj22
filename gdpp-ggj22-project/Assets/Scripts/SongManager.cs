using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on: https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games (Yu Chao)
public class SongManager : MonoBehaviour
{
    public static SongManager S;

    private AudioSource songSource;
    private bool songPlaying = false;

    [SerializeField] private GameObject blackNotePrefab;
    [SerializeField] private GameObject whiteNotePrefab;

    private Transform track1NoteHolder;
    private Transform track2NoteHolder;
    [SerializeField] private Transform track1StartMarker;
    [SerializeField] private Transform track1ButtonMarker;
    [SerializeField] private Transform track1EndMarker;
    [SerializeField] private Transform track2StartMarker;
    [SerializeField] private Transform track2ButtonMarker;
    [SerializeField] private Transform track2EndMarker;

    private float bpm;
    private float songPosInSecs;
    [HideInInspector] public float songPosInBeats;
    private float secPerBeat;
    private float songStartTime;

    private float[] track1Notes;
    private float[] track2Notes;
    private int numNotes1;
    private int numNotes2;
    private int nextIndex1 = 0;
    private int nextIndex2 = 0;
    [HideInInspector] public float notesShownInAdvance;
    [HideInInspector] public int totalNotes;

    private void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
        songSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (songPlaying)
        {
            UpdatePosition();
            if (nextIndex1 < numNotes1 && track1Notes[nextIndex1] < songPosInBeats + notesShownInAdvance) 
                SpawnNote(track1Notes, whiteNotePrefab, track1NoteHolder, track1ButtonMarker, track1EndMarker, ref nextIndex1);
            if (nextIndex2 < numNotes2 && track2Notes[nextIndex2] < songPosInBeats + notesShownInAdvance)
                SpawnNote(track2Notes, blackNotePrefab, track2NoteHolder, track2ButtonMarker, track2EndMarker, ref nextIndex2);
            if (songPosInSecs >= songSource.clip.length)
            {
                GameManager.S.ClearedSong();
            }
        }
    }

    public void SetupBeatMap(BeatMap beatMap)
    {
        bpm = beatMap.bpm;
        notesShownInAdvance = beatMap.notesShownInAdvance;
        track1Notes = beatMap.track1Notes.ToArray();
        track2Notes = beatMap.track2Notes.ToArray();
        songSource.clip = beatMap.song;

        numNotes1 = track1Notes.Length;
        numNotes2 = track2Notes.Length;
        secPerBeat = 60f / bpm;

        totalNotes = track1Notes.Length + track2Notes.Length;
    }

    public void StartSong()
    {
        songStartTime = (float) AudioSettings.dspTime;
        songPosInBeats = 0f;
        songPosInSecs = 0f;

        track1NoteHolder = Instantiate(new GameObject(), track1StartMarker).transform;
        track2NoteHolder = Instantiate(new GameObject(), track2StartMarker).transform;

        songSource.Play();
        songPlaying = true;
    }

    private void UpdatePosition()
    {
        songPosInSecs = (float)(AudioSettings.dspTime - songStartTime);
        songPosInBeats = songPosInSecs / secPerBeat;
    }

    private void SpawnNote(float[] notes, GameObject notePrefab, Transform noteHolder, Transform button, Transform end, ref int index)
    {
        MusicNote note = Instantiate(notePrefab, noteHolder).GetComponent<MusicNote>();
        note.beatOfThisNote = notes[index];
        note.startPos = noteHolder.position;
        note.buttonPos = button.position;
        note.removePos = end.position;
        index++;
    }

    public void StopSong()
    {
        songSource.Stop();
        songPlaying = false;
        Destroy(track1NoteHolder.gameObject);
        Destroy(track2NoteHolder.gameObject);
    }
}
