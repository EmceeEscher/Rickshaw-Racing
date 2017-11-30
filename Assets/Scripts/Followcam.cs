using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followcam : MonoBehaviour {

    public Transform p1Rickshaw;
	public Transform p2Rickshaw;
	private Vector3 targetLoc;

	public Vector3 camOffset = new Vector3(-95.5f, 147.2f, 100.5f);
    public float zoomFactor = 0.75f;


    // Use this for initialization
    void Start() {
		targetLoc = calculateMidPoint ();
    }

    // Update is called once per frame
    void FixedUpdate() {
		targetLoc = calculateMidPoint ();
		transform.position = Vector3.Slerp(transform.position, targetLoc, Time.deltaTime * 7.0f);
    }

	Vector3 calculateMidPoint() {
		Vector3 playerDiff = p1Rickshaw.position - p2Rickshaw.position;
		//playerDiff will be a vector starting at P2, pointint at P1
		Vector3 midpoint = p2Rickshaw.position + playerDiff/2;
		Vector3 offset = camOffset - transform.forward * playerDiff.magnitude * zoomFactor;

		return midpoint + offset;
	}
}
