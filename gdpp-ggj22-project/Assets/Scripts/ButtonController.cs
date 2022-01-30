using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{   
    private SpriteRenderer sr;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite pressedImage;
    private ParticleSystem ps;

    private BoxCollider2D bc;
    private BoxCollider2D[] overlappingColliders = new BoxCollider2D[1];
    private ContactFilter2D contactFilter;

    // Key that maps to this input
    public KeyCode keyMapping;
    // Each button should have a unique and unchanging id for recording purposes
    [Range(1,2)] public int trackID;

    [SerializeField] private SpriteRenderer trackSR;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform buttonPos;
    [SerializeField] private GameObject linePrefab;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        ps = GetComponent<ParticleSystem>();

        contactFilter = contactFilter.NoFilter();
    }

    public void SetupTrack()
    {
        trackSR.transform.position = buttonPos.position;
        trackSR.size = new Vector2(Mathf.Abs(trackSR.transform.position.x - startPos.position.x), 1);

        float step = trackSR.size.x / SongManager.S.notesShownInAdvance;

        for (int i = 1; i <= SongManager.S.notesShownInAdvance; i++)
        {
            Instantiate(linePrefab, new Vector2(startPos.position.x - (step * i), startPos.position.y), Quaternion.identity);
        }
    }

    public void Pressed()
    {
        sr.sprite = pressedImage;

        if (bc.OverlapCollider(contactFilter, overlappingColliders) > 0)
        {
            Collider2D collider = overlappingColliders[0];
            GameManager.S.PlayedNote(this, SongManager.S.songPosInBeats - collider.transform.GetComponent<MusicNote>().beatOfThisNote);
            Destroy(overlappingColliders[0].gameObject);
        }
    }

    public void Unpressed()
    {
        sr.sprite = defaultImage;
    }

    public void PlayParticles()
    {
        ps.Play();
    }
}
