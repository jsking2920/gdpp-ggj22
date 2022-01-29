using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on: https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games (Yu Chao)
public class SongManager : MonoBehaviour
{
    private float songPosInSecs;
    private float songPosInBeats;
    private float secPerBeat;
    private float dspTimeSong;

    //beats per minute of a song
    private float bpm;
    //keep all the position-in-beats of notes in the song
    private float[] notes;
    private int numNotes;
    //the index of the next note to be spawned
    private int nextIndex = 0;
    private int beatsShownInAdvance;

    public AudioSource songSource;
    public GameObject notePrefab;

    void Start()
    {
        numNotes = notes.Length;

        //calculate how many seconds is one beat
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dspTimeSong = (float)AudioSettings.dspTime;

        songSource.Play();
    }

    void Update()
    {
        songPosInSecs = (float)(AudioSettings.dspTime - dspTimeSong);

        songPosInBeats = songPosInSecs / secPerBeat;

        if (nextIndex < numNotes && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            Instantiate(notePrefab);
            nextIndex++;
        }
    }
}
