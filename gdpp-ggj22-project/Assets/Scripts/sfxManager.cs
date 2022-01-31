using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    public static sfxManager S;

    private AudioSource audioSource;

    public AudioClip goodNoteSFX;
    public AudioClip okayNoteSFX;
    public AudioClip badNoteSFX;
    public AudioClip missedNoteSFX;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip uiClick;

    private void Awake()
    {
        if (S) Destroy(S.gameObject);
        S = this;

        DontDestroyOnLoad(this);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
