using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaCleanText : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;

    Coroutine areaCleanedroutine;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.SetActive(false);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    Debug.Log(areaCleanedroutine);
    //    if (areaCleanedroutine == null && FindObjectOfType<AreaManager>().IsAreaClean())
    //    {
    //        Debug.Log("ok");
    //        areaCleanedroutine = StartCoroutine(LaunchGameOver());
    //    }
    //    if (areaCleanedroutine != null && FindObjectOfType<AreaManager>().IsAreaClean() == false)
    //    {
    //        StopCoroutine(areaCleanedroutine);
    //    }
    //}

    IEnumerator LaunchGameOver()
    {
        Color color = new Color(1f, 0.9243603f, 0.2028302f, 0f);
        float a = 0.0f;
        StartCoroutine(MakeGameOverAppear(a, color));
        gameOverText.SetActive(true);
        yield return new WaitForSeconds(3);
        a = 1f;
        StartCoroutine(MakeGameOverDisappear(a, color));
    }

    IEnumerator MakeGameOverAppear(float a, Color color)
    {
        while (true)
        {
            color = new Color(1f, 0.9243603f, 0.2028302f, a);
            gameOverText.GetComponent<Text>().color = color;
            yield return new WaitForSeconds(0.01f);
            a += 0.01f;
        }
    }

    IEnumerator MakeGameOverDisappear(float a, Color color)
    {
        while (true)
        {
            color = new Color(1f, 0.9243603f, 0.2028302f, a);
            gameOverText.GetComponent<Text>().color = color;
            yield return new WaitForSeconds(0.01f);
            a -= 0.01f;
        }
    }
}
