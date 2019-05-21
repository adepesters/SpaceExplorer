using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBlock : MonoBehaviour
{
    Vector3 initPos;
    SpriteRenderer myRenderer;
    Color color;
    float speedOfColorChange = 50f;
    bool hasJustStarted = true;
    Player player;
    Rigidbody2D myRigidBody;
    float thresholdAppearance = 60f;

    // Start is called before the first frame update
    void Start()
    {
        color = new Color(0, 0f, Random.Range(0.3f, 1f), Random.Range(0.4f, 0.8f));
        initPos = transform.position;
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.color = color;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < thresholdAppearance)
        {
            myRenderer.enabled = true;
            myRigidBody.simulated = true;
            hasJustStarted = false;
            ChangeColor();
            if (!transform.parent.GetComponent<PixelBlocksField>().PlayerCanPass)
            {
                myRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                myRigidBody.constraints = RigidbodyConstraints2D.None;
            }
        }
        else
        {
            myRigidBody.simulated = false;
            myRenderer.enabled = false;
        }
    }

    private void ChangeColor()
    {
        color.a += Random.Range(-0.03f, 0.03f) * Time.deltaTime * speedOfColorChange;
        color.b += Random.Range(-0.03f, 0.03f) * Time.deltaTime * speedOfColorChange;
        color.a = Mathf.Clamp(color.a, 0.5f, 0.8f);
        color.b = Mathf.Clamp(color.a, 0.5f, 1f);
        myRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasJustStarted)
        {
            if (collision.gameObject.name.Contains("Planet"))
            {
                Destroy(gameObject);
            }
        }
    }
}
