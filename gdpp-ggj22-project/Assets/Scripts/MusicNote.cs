using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    // removePos needs to be startPos mirrored by buttonPos
    [HideInInspector] public Vector2 startPos;
    [HideInInspector] public Vector2 buttonPos;
    [HideInInspector] public Vector2 removePos;
    private float interpolationVal;
    [HideInInspector] public float beatOfThisNote;

    private float notesShownInAdvance;

    private float missedNoteBufferInBeats = 0.25f;
    private bool passedButtonPos = false;
    private bool firedMissedFunction = false;

    private void Start()
    {
        notesShownInAdvance = SongManager.S.notesShownInAdvance;
    }

    void Update()
    {
        if (!passedButtonPos)
        {
            interpolationVal = (notesShownInAdvance - (beatOfThisNote - SongManager.S.songPosInBeats)) / notesShownInAdvance;
            if (interpolationVal > 0.99f) 
                passedButtonPos = true;
            transform.position = Vector2.Lerp(startPos, buttonPos, interpolationVal);
        }
        else
        {
            if (!firedMissedFunction && beatOfThisNote - (SongManager.S.songPosInBeats - missedNoteBufferInBeats) < 0f) 
                MissedNote();

            interpolationVal = (notesShownInAdvance - ((beatOfThisNote + notesShownInAdvance) - SongManager.S.songPosInBeats)) / notesShownInAdvance;
            if (interpolationVal > 0.4f) 
                Destroy(gameObject);
            transform.position = Vector2.Lerp(buttonPos, removePos, interpolationVal);
        }
    }

    private void MissedNote()
    {
        firedMissedFunction = true;
        GameManager.S.MissedNote();
    }
}
