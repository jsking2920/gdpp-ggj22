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

    private float missedNoteBufferInBeats = 0.5f;
    private bool firedMissedFunction = false;

    private SpriteRenderer sr;

    private void Start()
    {
        notesShownInAdvance = SongManager.S.notesShownInAdvance;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Track for notes is centered at the button, so this equals 0.5 on the notes beat, which means it's at the button
        interpolationVal = ((notesShownInAdvance - (beatOfThisNote - SongManager.S.songPosInBeats)) / notesShownInAdvance) * 0.5f;
        transform.position = Vector2.Lerp(startPos, removePos, interpolationVal);

        if (!firedMissedFunction && beatOfThisNote - (SongManager.S.songPosInBeats - missedNoteBufferInBeats) < 0f)
            MissedNote();
    }

    private void MissedNote()
    {
        firedMissedFunction = true;
        GameManager.S.MissedNote();
        Destroy(gameObject, 0.5f);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (true)
        {
            sr.color -= new Color(0, 0, 0, 0.25f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
