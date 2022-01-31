using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFX : MonoBehaviour
{
    private float speed = 1.3f;
    private float lifetime = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }

    public void SetText(string text)
    {
        TextMeshPro tmp = GetComponent<TextMeshPro>();
        tmp.text = text;
        tmp.enabled = true;
    }
}
