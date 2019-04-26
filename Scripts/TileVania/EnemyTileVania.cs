using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTileVania : MonoBehaviour
{
    float health = 500f;
    Color originalColor;
    float[] maxNumberOfParticles = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f };
    float[] probabilityOfParticles = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f };
    float jitter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Sword"))
        {
            ProcessHit(collision);
            if (health <= 0)
            {
                KillEnemy();
            }
        }
    }

    private void ProcessHit(Collider2D collision)
    {
        health -= collision.gameObject.GetComponent<Sword>().GetDamage();
        StartCoroutine(ChangeColorWhenHit());
        BleedParticles();
    }

    IEnumerator ChangeColorWhenHit()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

    private void BleedParticles()
    {
        List<GameObject> listOfBonuses = FindObjectOfType<ListOfBonuses>().listOfBonuses;
        int numBonuses = listOfBonuses.Count;
        RandomizeOrder(listOfBonuses);

        for (int bonusIndex = 0; bonusIndex < numBonuses; bonusIndex++)
        {
            for (int i = 0; i < maxNumberOfParticles[bonusIndex]; i++)
            {
                float rand = UnityEngine.Random.Range(0f, 1f);
                if (rand < probabilityOfParticles[bonusIndex])
                {
                    Vector3 bonusPos = new Vector3(UnityEngine.Random.Range(transform.position.x - jitter, transform.position.x + jitter),
                    UnityEngine.Random.Range(transform.position.y - jitter, transform.position.y + jitter),
                    transform.position.z);
                    Instantiate(listOfBonuses[bonusIndex], bonusPos, Quaternion.identity);
                }
            }
        }
    }

    private static void RandomizeOrder(List<GameObject> listOfBonuses)
    {
        for (int i = 0; i < listOfBonuses.Count; i++)
        {
            GameObject temp = listOfBonuses[i];
            int randomIndex = Random.Range(i, listOfBonuses.Count);
            listOfBonuses[i] = listOfBonuses[randomIndex];
            listOfBonuses[randomIndex] = temp;
        }
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
    }


}
