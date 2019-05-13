using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelBlocksField : MonoBehaviour
{
    [SerializeField] GameObject pixelBlock;
    int numBlocks = 2000;
    float radius;
    [SerializeField] bool playerCanPass = false;
    CircleCollider2D mycircleCollider;

    public bool PlayerCanPass { get => playerCanPass; set => playerCanPass = value; }

    // Start is called before the first frame update
    void Start()
    {
        mycircleCollider = GetComponent<CircleCollider2D>();
        radius = mycircleCollider.radius;
        for (int i = 0; i < numBlocks; i++)
        {
            GeneratePixelBlock();
        }
    }

    private void GeneratePixelBlock()
    {
        Vector3 pos = Random.insideUnitCircle * (radius - 3);
        pos += transform.parent.transform.position;
        GameObject newPixelBlock = Instantiate(pixelBlock, pos, Quaternion.identity, transform);
        newPixelBlock.transform.localScale *= Random.Range(0.05f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCanPass)
        {
            mycircleCollider.enabled = false;
        }
        else
        {
            mycircleCollider.enabled = true;
        }
    }

}
