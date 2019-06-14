using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoHalo : MonoBehaviour
{
    VLB.VolumetricLightBeam mylight;

    // Start is called before the first frame update
    void Start()
    {
        mylight = GetComponent<VLB.VolumetricLightBeam>();
    }

    // Update is called once per frame
    void Update()
    {
        mylight.intensityGlobal += Random.Range(-30f, 30f) * Time.deltaTime;
        mylight.intensityGlobal = Mathf.Clamp(mylight.intensityGlobal, 2f, 8f);
        mylight.UpdateAfterManualPropertyChange();
    }
}
