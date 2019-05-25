using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{

    [SerializeField] float transparency = 1f;
    SpriteRenderer spriterenderer;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        originalColor = spriterenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        transparency = Mathf.Clamp(transparency, 0f, 1f);
        Color newColor = originalColor;
        newColor.a = transparency;
        spriterenderer.color = newColor;
    }
}



