using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStarfieldDisappear : MonoBehaviour
{
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;

    // Start is called before the first frame update
    void Start()
    {
        m_System = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_System.particleCount];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTransparencyToZero(float transparency)
    {
        int numParticlesAlive = m_System.GetParticles(m_Particles);

        // Debug.Log(numParticlesAlive);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            Color color = m_Particles[i].GetCurrentColor(m_System);
            //            Debug.Log(color);
            color.a -= 0.005f * Time.deltaTime;
            color.a = Mathf.Max(color.a, transparency);
            m_Particles[i].startColor = color;
        }

        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

}
