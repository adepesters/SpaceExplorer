using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanicRockDetector : MonoBehaviour
{
    [SerializeField] GameObject virtualRock;

    [SerializeField] AudioClip[] rockImpactSound;
    float volumeSoundRockImpact = 0.3f;

    AudioSource audiosource;
    PlayerTileVania player;

    float counter = 0f;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerTileVania>();
    }

    private void Update()
    {
        counter += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<VolcanicRock>().IsTargetingPlayerLayer)
        {
            if (gameObject.tag == other.gameObject.tag)
            {
                GameObject newVirtualRock = Instantiate(virtualRock, other.gameObject.transform.position, Quaternion.identity);
                newVirtualRock.tag = other.gameObject.tag;
                if (Vector2.Distance(player.transform.position, newVirtualRock.transform.position) < 20f)
                {
                    if (counter > 0.3f)
                    {
                        // audiosource.PlayOneShot(rockImpactSound[UnityEngine.Random.Range(0, rockImpactSound.Length - 1)], volumeSoundRockImpact);
                        counter = 0;
                    }
                }
                Destroy(other.gameObject, 0.01f);
                Destroy(newVirtualRock, 0.01f);
            }
        }
    }
}
