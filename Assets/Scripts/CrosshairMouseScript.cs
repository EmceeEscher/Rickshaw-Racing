using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairMouseScript : MonoBehaviour {

	private Rect position;
	public Texture2D crosshairTexture;
	public Rigidbody bulletRigidbody;
	public Camera followCam;
	public Transform car;
	public float force = 0.0f;
	public int ammoCount = 10;
	public GameObject startLine;
	private LapManagerScript lapManager;

	// Use this for initialization
	void Start () {
		position = new Rect(
			(Input.mousePosition.x - crosshairTexture.width/2), 
			(Screen.height - Input.mousePosition.y - crosshairTexture.height/2), 
			crosshairTexture.width, 
			crosshairTexture.height);

		lapManager = startLine.GetComponent<LapManagerScript> ();
	}

	// Update is called once per frame
	void Update () {
		position = new Rect(
			(Input.mousePosition.x - crosshairTexture.width/2), 
			(Screen.height - Input.mousePosition.y - crosshairTexture.height/2), 
			crosshairTexture.width, 
			crosshairTexture.height);
		if (Input.GetMouseButtonDown (0)) {
			OnClick ();
		}
	}

	void OnGUI() {
		GUI.DrawTexture (position, crosshairTexture);
	}

	void OnClick(){
		print ("ammoCount: " + ammoCount);
		if (lapManager.hasRaceStarted ()) {
			if (ammoCount > 0) {
				RaycastHit hit;
				int surfaceLayer = 1 << 8;
				Vector3 crosshairLocation = Input.mousePosition;
				// This gives pixel coordinates, not in-world coordinates
				// need to get world coordinates using Camera.main.ScreenPointToRay instead
				Ray ray = followCam.ScreenPointToRay (crosshairLocation);

				if (Physics.Raycast (ray, out hit, Mathf.Infinity, surfaceLayer)) {
					Vector3 destination = hit.point;
					destination.y = car.position.y;
					createBullet (hit.point);
					ammoCount--;
				}
			}
		}
	}

	void createBullet(Vector3 crosshairLoc) {
		Vector3 bulletDir = crosshairLoc - car.position;
		bulletDir = bulletDir.normalized;
		bulletDir.y = 0;
		Vector3 bulletSpawnLoc = car.position + bulletDir * 10.0f;
		Rigidbody bulletClone = (Rigidbody)Instantiate (bulletRigidbody, bulletSpawnLoc, Random.rotation);
		bulletClone.velocity = bulletDir * force;
	}

	public void addAmmo(int amount) {
		ammoCount += amount;
	}
}