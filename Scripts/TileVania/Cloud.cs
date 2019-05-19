using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    float speed;
    float direction;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.05f, 0.15f);
        direction = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime * Mathf.Sign(direction), 0, 0);
    }
}
