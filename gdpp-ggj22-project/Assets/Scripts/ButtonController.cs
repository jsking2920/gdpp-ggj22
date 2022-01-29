using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{   
    private SpriteRenderer sr;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite pressedImage;

    private BoxCollider2D bc;
    private BoxCollider2D[] overlappingColliders = new BoxCollider2D[1];
    private ContactFilter2D contactFilter;

    // Key that maps to this input
    public KeyCode keyMapping;
    // Each button should have a unique and unchanging id for recording purposes
    [Range(1,2)] public int trackID;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        contactFilter = contactFilter.NoFilter();
    }

    public void Pressed()
    {
        sr.sprite = defaultImage;

        // Check to see if note is overlapping this button
        if (bc.OverlapCollider(contactFilter, overlappingColliders) > 0)
        {
            GameManager.S.PlayedNote();
            Destroy(overlappingColliders[0].gameObject);
        }
    }

    public void Unpressed()
    {
        sr.sprite = pressedImage;
    }
}
