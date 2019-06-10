using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    [SerializeField] GameObject rock;
    PlayerTileVania player;
    GameObject newRock;
    Vector3 target;

    const string VOLCANO_ROCKS = "Volcano Rocks Parent";
    GameObject volcanoRocksParent;

    // Start is called before the first frame update
    void Start()
    {
        volcanoRocksParent = GameObject.Find(VOLCANO_ROCKS);
        if (!volcanoRocksParent)
        {
            volcanoRocksParent = new GameObject(VOLCANO_ROCKS);
        }

        player = FindObjectOfType<PlayerTileVania>();
        StartCoroutine(LaunchRocks());
    }

    IEnumerator LaunchRocks()
    {
        while (true)
        {
            float rdnTime = Random.Range(0.01f, 0.1f) * 1.2f; //1.2 is arbitrary.

            Vector3 rdnTarget;
            Vector3 oriPos;

            oriPos = new Vector3(transform.position.x, transform.position.y + 35, transform.position.z + 5);
            target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 100f) // to make sure only close-by volcanos can target the player
            {
                int rdnChoice = Random.Range(0, 10);

                if (rdnChoice < 1)
                {
                    newRock = Instantiate(rock, oriPos, Quaternion.identity, volcanoRocksParent.transform);
                    newRock.GetComponent<VolcanicRock>().Target = target;
                    newRock.GetComponent<VolcanicRock>().IsTargetingPlayerLayer = true;
                    newRock.tag = player.tag;
                }
                if (rdnChoice > 0 && rdnChoice < 5)
                {

                    rdnTarget = new Vector3(target.x + Random.Range(-30f, 30f),
                    target.y, target.z);
                    newRock = Instantiate(rock, oriPos, Quaternion.identity, volcanoRocksParent.transform);
                    newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
                    newRock.GetComponent<VolcanicRock>().IsTargetingPlayerLayer = true;
                    newRock.tag = player.tag;
                }
                else if (rdnChoice > 4)
                {
                    rdnTarget = new Vector3(target.x + Random.Range(-200f, 200f),
                            target.y, target.z + Random.Range(0, 160f));
                    newRock = Instantiate(rock, oriPos, Quaternion.identity, volcanoRocksParent.transform);
                    newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
                }
            }
            else
            {
                rdnTarget = new Vector3(transform.position.x + Random.Range(-200f, 200f),
                            transform.position.y, transform.position.z + Random.Range(0, 160f));
                newRock = Instantiate(rock, oriPos, Quaternion.identity, volcanoRocksParent.transform);
                newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
            }

            yield return new WaitForSeconds(rdnTime);
        }
    }

}
