using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFX : MonoBehaviour
{
    private float speed = 1.1f;
    private float lifetime = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}
