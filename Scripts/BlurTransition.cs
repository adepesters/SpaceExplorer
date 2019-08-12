using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurTransition : MonoBehaviour
{
    Player player;
    float transparencyNonBlurred = 1f;
    float transparencyBlurred = 0f;

    [SerializeField] GameObject planetParent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transparencyBlurred = (Mathf.Abs(transform.position.z - player.transform.position.z) / 10f);
        transparencyBlurred = Mathf.Clamp(transparencyBlurred, 0f, 1f);
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, transparencyBlurred);

        if (transparencyBlurred > 0.9f)
        {
            transparencyNonBlurred = 1.5f - transparencyBlurred;
        }
        else
        {
            transparencyNonBlurred = 1;
        }
        //transparencyNonBlurred = 1 - transparencyBlurred;
        planetParent.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, transparencyNonBlurred);
    }
}
