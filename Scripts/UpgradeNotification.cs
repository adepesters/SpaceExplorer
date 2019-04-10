using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeNotification : MonoBehaviour
{
    Coroutine upgradeNotificationPause;
    Vector3 targetPos;
    float appearingSpeed = 65f;

    void Update()
    {
        if (upgradeNotificationPause == null)
        {
            upgradeNotificationPause = StartCoroutine(UpgradeNotificationCoroutine());
        }
        GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(GetComponent<RectTransform>().anchoredPosition, targetPos, appearingSpeed * Time.deltaTime);
    }

    IEnumerator UpgradeNotificationCoroutine()
    {
        targetPos = new Vector3(-1, -23, 0);
        yield return new WaitForSeconds(3);
        targetPos = new Vector3(-1, 32, 0);
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

}
