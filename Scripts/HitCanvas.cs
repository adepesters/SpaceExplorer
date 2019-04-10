using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCanvas : MonoBehaviour
{
    float displayDuration = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator HandleHitCanvas()
    {
        GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(displayDuration);
        GetComponent<Canvas>().enabled = false;
    }

}
