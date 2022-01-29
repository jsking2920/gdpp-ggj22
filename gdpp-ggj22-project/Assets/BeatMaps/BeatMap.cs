using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Config", order = 0)]
public class BeatMap : ScriptableObject
{
    public List<float> track1Notes = new List<float>();
    public List<float> track2Notes = new List<float>();

    public AudioClip song;
    public float bpm;
}
