using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundPlayer : MonoBehaviour
{
    float enemySpeed = 15f;
    Coroutine moveAround;
    Vector2 target;

    bool isImmobile;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isImmobile)
        {
            if (moveAround == null)
            {
                target = player.transform.position;
                moveAround = StartCoroutine(MoveAround());
            }

            transform.position = Vector2.MoveTowards(transform.position, target, enemySpeed * Time.deltaTime);
        }
    }

    IEnumerator MoveAround()
    {
        while (true)
        {
            enemySpeed += UnityEngine.Random.Range(-4f, 4f);
            enemySpeed = Mathf.Clamp(enemySpeed, 0.5f, 25f);
            Vector2 playerPos = player.transform.position;
            Vector2 randomJitter = new Vector2(UnityEngine.Random.Range(-40f, 40f), UnityEngine.Random.Range(-30f, 30f));
            target = playerPos + randomJitter;
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 4f));
        }
    }

    public void SetImmobile(bool currentImmobility)
    {
        isImmobile = currentImmobility;
    }
}
