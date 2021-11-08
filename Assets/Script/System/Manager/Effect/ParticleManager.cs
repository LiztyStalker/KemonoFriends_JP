using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ParticleManager : SingletonClass<ParticleManager>
{
    Dictionary<string, ParticleSystem> m_particleDic = new Dictionary<string, ParticleSystem>();

    public ParticleManager()
    {
        initParse();
    }

    void initParse()
    {
        ParticleSystem[] particles = Resources.LoadAll<ParticleSystem>(PrepClass.particleEffectPath);

        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                m_particleDic.Add(particles[i].name, particles[i]);
            }
        }
        else
        {
            Debug.LogError(string.Format("{0}의 데이터를 찾지 못했습니다", PrepClass.particleEffectPath));
        }
    }

    public int ElementCount { get { return m_particleDic.Count; } }

    public ParticleSystem getParticle(string key)
    {
        if (ElementCount > 0)
        {
//            Debug.LogWarning("key : " + key);
            if (m_particleDic.ContainsKey(key))
                return m_particleDic[key];
        }
        return null;
    }


}

