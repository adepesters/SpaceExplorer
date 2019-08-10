using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class Cloud : MonoBehaviour
{
    float speed;
    float direction;

    int planetID;

    // Start is called before the first frame update
    void Start()
    {
        string numbersOnly = Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", "");
        planetID = int.Parse(numbersOnly);
        if (planetID == 5) // volcano planet
        {
            speed = Random.Range(0.05f, 1f);
            direction = Random.Range(-1f, 1f);
        }
        else
        {
            speed = Random.Range(0.05f, 0.15f);
            direction = Random.Range(-1f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(speed * Time.deltaTime * Mathf.Sign(direction), 0, 0);
    }
}
