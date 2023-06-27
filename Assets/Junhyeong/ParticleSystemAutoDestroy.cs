using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        if (ps)
        {
            if (ps && !ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
