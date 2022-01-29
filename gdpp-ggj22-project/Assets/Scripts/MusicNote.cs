using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;

    public float beatOfThisNote;

    private void Start()
    {
        transform.position = startPos;
    }

    void Update()
    {
        transform.position = Vector2.Lerp(
            startPos,
            endPos,
            (SongManager.S.beatsShownInAdvance - (beatOfThisNote - SongManager.S.songPosInBeats)) / SongManager.S.beatsShownInAdvance
        );
    }
}
