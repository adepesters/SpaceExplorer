using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour
{
    [SerializeField] GameObject rock;
    PlayerTileVania player;
    GameObject newRock;
    float targetDepth;
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
            int rdnChoice = Random.Range(0, 10);
            float rdnTime = Random.Range(0.01f, 0.1f);
            Vector3 rdnTarget;

            targetDepth = player.transform.position.z;
            target = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 2f);

            if (rdnChoice < 1)
            {
                newRock = Instantiate(rock, transform.position, Quaternion.identity, volcanoRocksParent.transform);
                newRock.GetComponent<VolcanicRock>().Target = target;
            }
            if (rdnChoice > 0 && rdnChoice < 5)
            {

                rdnTarget = new Vector3(target.x + Random.Range(-30f, 30f),
                target.y, target.z);
                newRock = Instantiate(rock, transform.position, Quaternion.identity, volcanoRocksParent.transform);
                newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
            }
            else if (rdnChoice > 4)
            {
                rdnTarget = new Vector3(target.x + Random.Range(-200f, 200f),
                        target.y, target.z + Random.Range(0, 160f));
                newRock = Instantiate(rock, transform.position, Quaternion.identity, volcanoRocksParent.transform);
                newRock.GetComponent<VolcanicRock>().Target = rdnTarget;
            }

            yield return new WaitForSeconds(rdnTime);
        }
    }

}
