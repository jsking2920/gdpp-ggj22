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
    private bool firedMissedFunction = false;

    private void Start()
    {
        notesShownInAdvance = SongManager.S.notesShownInAdvance;
    }

    void Update()
    {
        // Track for notes is centered at the button, so this equals 0.5 on the notes beat, which means it's at the button
        interpolationVal = ((notesShownInAdvance - (beatOfThisNote - SongManager.S.songPosInBeats)) / notesShownInAdvance) * 0.5f;
        transform.position = Vector2.Lerp(startPos, removePos, interpolationVal);

        if (!firedMissedFunction && beatOfThisNote - (SongManager.S.songPosInBeats - missedNoteBufferInBeats) < 0f)
            MissedNote();
        if (interpolationVal > 0.6f)
            Destroy(gameObject);
    }

    private void MissedNote()
    {
        firedMissedFunction = true;
        GameManager.S.MissedNote();
    }
}
