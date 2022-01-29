using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on: https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games (Yu Chao)
public class SongManager : MonoBehaviour
{
    public static SongManager S;

    private float songPosInSecs;
    public float songPosInBeats;
    private float secPerBeat;
    private float dspTimeSong;

    //beats per minute of a song
    public float bpm;
    //keep all the position-in-beats of notes in the song
    public float[] notes;
    private int numNotes;
    //the index of the next note to be spawned
    private int nextIndex = 0;
    public float beatsShownInAdvance;

    public AudioSource songSource;
    public GameObject notePrefab;

    public Transform track1Start;
    public Transform track1End;

    private float[] notesTest = new float[100];

    private void Awake()
    {
        S = this;
    }

    void Start()
    {
        numNotes = notes.Length;

        //calculate how many seconds is one beat
        secPerBeat = 60f / bpm;

        //record the time when the song starts
        dspTimeSong = (float) AudioSettings.dspTime;

        songSource.Play();

        for (int i = 0; i < 100; i++)
        {
            notesTest[i] = (float)i;
        }
    }

    void Update()
    {
        songPosInSecs = (float)(AudioSettings.dspTime - dspTimeSong);

        songPosInBeats = songPosInSecs / secPerBeat;

        if (nextIndex < 100 && notesTest[nextIndex] < songPosInBeats + beatsShownInAdvance)//(nextIndex < numNotes && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            MusicNote note = Instantiate(notePrefab).GetComponent<MusicNote>();
            note.beatOfThisNote = notesTest[nextIndex];
            note.startPos = track1Start.position;
            note.endPos = track1End.position;
            nextIndex++;
        }
    }
}
