using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBulletScript : MonoBehaviour {
    public GameObject ParticleObject;

    Rigidbody rb;
    public float impact = 5;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(rb.velocity * impact, ForceMode.Impulse);
        }
        
        Instantiate(ParticleObject, transform.localPosition, transform.rotation);
		Destroy (gameObject);
	}
}
