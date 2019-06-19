using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErasePixels : MonoBehaviour
{
    Texture2D originalTexture;
    Texture2D newTexture;

    bool updateColors = true;

    PlayerTileVania player;

    GameObject portal;

    public bool UpdateColors { get => updateColors; set => updateColors = value; }
    public GameObject Portal { get => portal; set => portal = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTileVania>();
        Portal = new GameObject();
    }

    public Texture2D CopyTexture2D(Texture2D copiedTexture)
    {
        Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        int y = 0;
        while (y < texture.height)
        {
            int x = 0;
            while (x < texture.width)
            {
                if (player.transform.position.x - Portal.transform.position.x > 0)
                {
                    if (player.transform.localScale.x > 0)
                    {
                        if (x > texture.width - (14 + 15 * (player.transform.position.x - Portal.transform.position.x)))
                        {
                            Color pixelColor = texture.GetPixel(x, y);
                            Color color = pixelColor;
                            color.a = 0f;
                            texture.SetPixel(x, y, color);
                        }
                        else
                        {
                            texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                        }
                    }
                    else
                    {
                        if (x < (13 + 15 * (player.transform.position.x - Portal.transform.position.x)))
                        {
                            Color pixelColor = texture.GetPixel(x, y);
                            Color color = pixelColor;
                            color.a = 0f;
                            texture.SetPixel(x, y, color);
                        }
                        else
                        {
                            texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                        }
                    }
                }
                else if (player.transform.position.x - Portal.transform.position.x < 0)
                {
                    if (player.transform.localScale.x < 0)
                    {
                        if (x > texture.width - (14 + 15 * (Portal.transform.position.x - player.transform.position.x)))
                        {
                            Color pixelColor = texture.GetPixel(x, y);
                            Color color = pixelColor;
                            color.a = 0f;
                            texture.SetPixel(x, y, color);
                        }
                        else
                        {
                            texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                        }
                    }
                    else
                    {
                        if (x < (13 + 15 * (Portal.transform.position.x - player.transform.position.x)))
                        {
                            Color pixelColor = texture.GetPixel(x, y);
                            Color color = pixelColor;
                            color.a = 0f;
                            texture.SetPixel(x, y, color);
                        }
                        else
                        {
                            texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
                        }
                    }
                }
                ++x;
            }
            ++y;
        }

        texture.Apply();
        return texture;
    }

    public void UpdateCharacterTexture()
    {
        originalTexture = GetComponent<SpriteRenderer>().sprite.texture;
        newTexture = CopyTexture2D(originalTexture);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(newTexture, sr.sprite.rect, new Vector2(0.5f, 0.5f), 15);

        sr.material.mainTexture = newTexture;
    }

    public void LateUpdate()
    {
        if (UpdateColors)
        {
            UpdateCharacterTexture();
        }
    }

}
