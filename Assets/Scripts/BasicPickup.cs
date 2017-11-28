using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPickup : MonoBehaviour {

    public float degreesPerSecond;
    public float amplitude;
    public float frequency;
    private float timingOffset;

    public GameObject ParticleObject;


    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

	// Use this for initialization
	void Start () {
        posOffset = transform.position;
        timingOffset = Random.value * Mathf.PI;
    }
	
	// Update is called once per frame
	void Update () {
        //Spin Object
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        //Bob Up and Down
        tempPos = posOffset;
        tempPos.y += Mathf.Sin((Time.fixedTime + timingOffset) * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
	}

    private void OnDestroy()
    {
        Instantiate(ParticleObject, transform.position, Quaternion.identity);
    }

            

}
