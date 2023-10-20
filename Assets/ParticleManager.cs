using System;
using System.Collections;
using System.Collections.Generic;
using TorbuTils.EzPools;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Serializable] private class Particle
    {
        public string name;
        public GameObject prefab;
    }private class LiveParticle
    {
        public ParticleSystem system;
        public float timeRemaining;
    }
    public static ParticleManager Instance { get; private set; }

    [SerializeField] private List<Particle> particles = new();
    private List<LiveParticle> liveParticles = new();

    public void Spawn(string name, Vector2 position)
    {
        Particle p = particles.Find(x => x.name == name);
        if (p == null)
        {
            Debug.LogWarning($"'{name}' is not a registered particle");
            p = particles[0];
        }
        GameObject go = Pools.Instance.Depool(p.prefab);
        go.transform.position = position;
        ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();
        LiveParticle liveParticle = new();
        liveParticle.system = particleSystem;
        liveParticle.timeRemaining = particleSystem.main.startLifetime.constantMax;
        liveParticles.Add(liveParticle);
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        for (int i = liveParticles.Count - 1; i >= 0; i--)
        {
            LiveParticle liveParticle = liveParticles[i];
            liveParticle.timeRemaining -= Time.deltaTime;
            if (liveParticle.timeRemaining < 0f)
            {
                liveParticles.RemoveAt(i);
                Pools.Instance.Enpool(liveParticle.system.gameObject);
            }
        }
    }
}
