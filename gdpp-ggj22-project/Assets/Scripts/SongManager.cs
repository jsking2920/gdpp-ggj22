using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on: https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games (Yu Chao)
public class SongManager : MonoBehaviour
{
    public static SongManager S;

    [SerializeField] private AudioSource songSource;
    [SerializeField] private GameObject blackNotePrefab;
    [SerializeField] private GameObject whiteNotePrefab;

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
    public int totalNotes;

    public Transform track1StartMarker;
    public Transform track1ButtonMarker;
    public Transform track1EndMarker;
    public Transform track2StartMarker;
    public Transform track2ButtonMarker;
    public Transform track2EndMarker;

    private bool songPlaying = false;

    private void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;
    }

    void Update()
    {
        if (songPlaying)
        {
            UpdatePosition();
            if (nextIndex1 < numNotes1 && track1Notes[nextIndex1] < songPosInBeats + notesShownInAdvance) 
                SpawnNote(track1Notes, whiteNotePrefab, track1StartMarker, track1ButtonMarker, track1EndMarker, ref nextIndex1);
            if (nextIndex2 < numNotes2 && track2Notes[nextIndex2] < songPosInBeats + notesShownInAdvance)
                SpawnNote(track2Notes, blackNotePrefab, track2StartMarker, track2ButtonMarker, track2EndMarker, ref nextIndex2);
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
        songStartTime = (float)AudioSettings.dspTime;
        songPosInBeats = 0f;
        songPosInSecs = 0f;
        songSource.Play();
        songPlaying = true;
    }

    private void UpdatePosition()
    {
        songPosInSecs = (float)(AudioSettings.dspTime - songStartTime);
        songPosInBeats = songPosInSecs / secPerBeat;
    }

    private void SpawnNote(float[] notes, GameObject notePrefab, Transform start, Transform button, Transform end, ref int index)
    {
        MusicNote note = Instantiate(notePrefab, start.position, Quaternion.identity).GetComponent<MusicNote>();
        note.beatOfThisNote = notes[index];
        note.startPos = start.position;
        note.buttonPos = button.position;
        note.removePos = end.position;
        index++;
    }

    public void StopSong()
    {
        songSource.Stop();
        songPlaying = false;
        foreach (GameObject note in GameObject.FindGameObjectsWithTag("Note"))
        {
            Destroy(note);
        }
    }
}
