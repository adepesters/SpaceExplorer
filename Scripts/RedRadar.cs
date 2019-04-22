using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRadar : MonoBehaviour
{
    GameObject targetZone;

    float facingSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //targetZone = GameObject.Find("Dialog Fairy");

        targetZone = GameObject.Find("Pointer");
    }

    // Update is called once per frame
    void Update()
    {
        //GameObject targetZone = GameObject.Find("Zone 3");

        //Debug.Log(targetZone.transform.position);

        targetZone = GameObject.Find("Pointer");

        Transform target = targetZone.gameObject.transform;
        Vector2 direction = target.position - FindObjectOfType<Player>().gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 135;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, facingSpeed * Time.deltaTime);
    }

    public void SetCurrentTargetRedRadar(GameObject currentTargetZone)
    {
        targetZone = currentTargetZone;
    }
}
