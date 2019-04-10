using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeed : MonoBehaviour
{
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;

    Player player;
    Vector3 playerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_System = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_System.particleCount];

        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        int numParticlesAlive = m_System.GetParticles(m_Particles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            m_Particles[i].velocity = player.GetComponent<Rigidbody2D>().velocity;
            Debug.Log(m_Particles[i].velocity);
        }

        // Apply the particle changes to the Particle System
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }
}
