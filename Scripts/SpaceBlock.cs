using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlock : MonoBehaviour
{
    Vector3 initPos;
    SpriteRenderer renderer;
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        color = new Color(0, 0f, Random.Range(0.3f, 1f), Random.Range(0.4f, 0.8f));
        initPos = transform.position;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = initPos;
        color.a += Random.Range(-0.03f, 0.03f);
        color.b += Random.Range(-0.03f, 0.03f);
        color.a = Mathf.Clamp(color.a, 0.5f, 0.8f);
        color.b = Mathf.Clamp(color.a, 0.5f, 1f);
        renderer.color = color;
    }
}
