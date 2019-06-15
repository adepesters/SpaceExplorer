using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientRumbleSound : MonoBehaviour
{

    [SerializeField] AudioClip[] ambientRumbleSound;
    float volumeAmbientSound = 0.7f;

    AudioSource audiosource;

    float counter = 0f;

    int newIdx;
    int oldIdx = 10;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.PlayOneShot(ambientRumbleSound[0], volumeAmbientSound);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > Random.Range(1, 10))
        {
            newIdx = UnityEngine.Random.Range(0, ambientRumbleSound.Length - 1);
            if (oldIdx != newIdx)
            {
                Debug.Log(newIdx);
                audiosource.PlayOneShot(ambientRumbleSound[newIdx], Random.Range(0.1f, 0.7f));
            }
            counter = 0;
            oldIdx = newIdx;
        }
    }
}
