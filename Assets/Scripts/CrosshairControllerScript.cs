using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairControllerScript : MonoBehaviour {

	private Rect position;
	private Rect basePosition;
	public Texture2D crosshairTexture;
	public GameObject bulletPrefab;
	public Transform car;
	public Camera followCam;
	public float force = 0.0f;
	public int ammoCount = 10;
	public GameObject startLine;
	private LapManagerScript lapManager;
	private float lastFireTime = 0.0f;
	public float fireDelay = 1.0f;
	public int playerNum = 1;
	public float bulletSpawnDistance = 15.0f;
	private string horizontalAimAxis = "HorizontalAimP1";
	private string verticalAimAxis = "VerticalAimP1";
	private string fireAxis = "FireP1";
	private int bulletLayer;
	private string parentName = "P1Rickshaw";

	// Use this for initialization
	void Start () {
		Vector3 carPosition = followCam.WorldToScreenPoint (car.position);

		basePosition = new Rect (
			(carPosition.x - crosshairTexture.width / 2), 
			(Screen.height - carPosition.y - crosshairTexture.height / 2), 
			crosshairTexture.width, 
			crosshairTexture.height);

		position = basePosition;

		lapManager = startLine.GetComponent<LapManagerScript> ();

		bulletLayer = LayerMask.NameToLayer ("P1Bullets");

		if (playerNum == 2) {
			horizontalAimAxis = "HorizontalAimP2";
			verticalAimAxis = "VerticalAimP2";
			fireAxis = "FireP2";
			bulletLayer = LayerMask.NameToLayer ("P2Bullets");
			parentName = "P1Rickshaw";
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 carPosition = followCam.WorldToScreenPoint (car.position);

		basePosition = new Rect (
			(carPosition.x - crosshairTexture.width / 2), 
			(Screen.height - carPosition.y - crosshairTexture.height / 2), 
			crosshairTexture.width, 
			crosshairTexture.height);

		position = new Rect (
			(basePosition.x + Input.GetAxis (horizontalAimAxis) * 250.0f),
			(basePosition.y + Input.GetAxis (verticalAimAxis) * -250.0f),
			crosshairTexture.width,
			crosshairTexture.height);

		if (Input.GetAxis(fireAxis) > 0 && (Time.time - lastFireTime) > fireDelay) {
			OnFire ();
			lastFireTime = Time.time;
		}
	}

	void OnGUI() {
		GUI.DrawTexture (position, crosshairTexture);
	}

	void OnFire(){
		print ("ammoCount: " + ammoCount);
		if (lapManager.hasRaceStarted ()) {
			if (ammoCount > 0) {
				RaycastHit hit;
				int surfaceLayer = 1 << 8;
				Vector3 crosshairLocation = new Vector3 (
					                           (position.x + crosshairTexture.width / 2), 
					                           (Screen.height - position.y - crosshairTexture.height / 2),
					                           0);
				// This gives pixel coordinates, not in-world coordinates
				// need to use Camera.main.ScreenPointToRay instead
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
		Vector3 bulletSpawnLoc = car.position + bulletDir * bulletSpawnDistance;
		GameObject bulletClone = Instantiate (bulletPrefab, bulletSpawnLoc, Quaternion.identity);
		bulletClone.layer = bulletLayer;
		Rigidbody bulletRb = bulletClone.GetComponent<Rigidbody> ();
		bulletRb.velocity = bulletDir * force;
	}

	public void addAmmo(int amount) {
		ammoCount += amount;
	}
}
