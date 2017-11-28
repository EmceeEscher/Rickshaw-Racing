using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour {

    public float timeToDestroy;

    // Use this for initialization
    void Start()
    {
        timeToDestroy = GetComponent<ParticleSystem>().main.duration;
        Destroy(gameObject, timeToDestroy * 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
