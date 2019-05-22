using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    [SerializeField] GameObject rock;
    PlayerTileVania player;
    int numRocks = 30;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTileVania>();
        StartCoroutine(LaunchRocks());
    }

    IEnumerator LaunchRocks()
    {
        while (true)
        {
            int rdnChoice = Random.Range(0, 10);
            float rdnTime = Random.Range(0.01f, 0.1f);
            Vector3 rdnTarget;
            GameObject newRock;

            if (rdnChoice == 0)
            {
                GameObject rockTargetingPlayer = Instantiate(rock, transform.position, Quaternion.identity);
                rockTargetingPlayer.GetComponent<VolcanicRock>().Target = player.transform.position;
            }
            if (rdnChoice > 0 && rdnChoice < 5)
            {

                rdnTarget = new Vector3(player.transform.position.x + Random.Range(-80f, 80f),
                player.transform.position.y, player.transform.position.z);
                newRock = Instantiate(rock, transform.position, Quaternion.identity);
                newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
            }
            else
            {
                rdnTarget = new Vector3(player.transform.position.x + Random.Range(-200f, 200f),
                        player.transform.position.y, player.transform.position.z + Random.Range(-200f, 200f));
                newRock = Instantiate(rock, transform.position, Quaternion.identity);
                newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
            }

            yield return new WaitForSeconds(rdnTime);
        }
    }

}
