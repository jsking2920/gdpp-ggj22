using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    [HideInInspector] public Vector2 startPos;
    [HideInInspector] public Vector2 endPos;
    private float interpolationVal;
    [HideInInspector] public float beatOfThisNote;

    private float notesShownInAdvance;

    private void Start()
    {
        notesShownInAdvance = SongManager.S.notesShownInAdvance;
    }

    void Update()
    {
        if ((interpolationVal = (notesShownInAdvance - (beatOfThisNote - SongManager.S.songPosInBeats)) / notesShownInAdvance) > 0.999f)
            Destroy(gameObject);
        else transform.position = Vector2.Lerp(startPos, endPos,
            (notesShownInAdvance - (beatOfThisNote - SongManager.S.songPosInBeats)) / notesShownInAdvance);
    }
}
