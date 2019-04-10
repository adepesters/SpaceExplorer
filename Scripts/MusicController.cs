using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] tracks;

    float[] volumeTrack = new float[4];

    // Start is called before the first frame update
    void Start()
    {
        volumeTrack[0] = 1f;
        volumeTrack[1] = 1f;
        volumeTrack[2] = 1f;
        volumeTrack[3] = 0f;

        GetComponent<AudioSource>().PlayOneShot(tracks[0], volumeTrack[0]);
        GetComponent<AudioSource>().PlayOneShot(tracks[1], volumeTrack[1]);
        GetComponent<AudioSource>().PlayOneShot(tracks[2], volumeTrack[2]);
        GetComponent<AudioSource>().PlayOneShot(tracks[3], volumeTrack[3]);
    }

    // Update is called once per frame
    void Update()
    {
        volumeTrack[0] = 1f;
        volumeTrack[1] = 1f;
        volumeTrack[2] = 1f;
        volumeTrack[3] = 0f;
    }

    public void SetVolumeTrack(int currentTrackIndex, float currentVolume)
    {
        volumeTrack[currentTrackIndex] = currentVolume;
    }
}
