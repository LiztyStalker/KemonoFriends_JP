using System.Collections;
using UnityEngine;

public class ParticleLife : MonoBehaviour
{
    void Start()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        if(particle != null)
            StartCoroutine(particleTimeCoroutine(particle.main.duration));
    }

    IEnumerator particleTimeCoroutine(float time)
    {
        while (time > 0)
        {
            time -= PrepClass.frameTime;
            yield return new WaitForSeconds(PrepClass.frameTime);
        }

        Destroy(gameObject);
    }
}

