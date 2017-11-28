using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Rigidbody rb;
    public float speed;
    public float boostedSpeed;
    public float baseSpeed;
    
    public Quaternion targetRotation;
    private float orientTolerance = 0.2f;
    private float boost;
    private float maxBoost = 150;

	public CrosshairMouseScript crosshairScript;
	public GameObject startLine;
    private bool startedRace;
    private bool wheelsOnGround;
    private bool boosting;

    public List<GameObject> wheels;

	public bool usingController = false;

	public int playerNum = 1;
	private string playerHorizAxis = "HorizontalMoveP1";
	private string playerVertAxis = "VerticalMoveP1";



    



    private float turnSpeed;
    private float deltaTurn;

    private Vector3 updatedForward;
  
    private bool turning;


    private TrailRenderer trailRend;

	private LapManagerScript lapManager;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        GetWheels();

        targetRotation = transform.rotation;
        turnSpeed = 0f;

		lapManager = startLine.GetComponent<LapManagerScript> ();

		if (playerNum == 2) {
			playerHorizAxis = "HorizontalMoveP2";
			playerVertAxis = "VerticalMoveP2";
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(lapManager.hasRaceStarted()){
        	SpinWheels();
        	HandleControls();
            //OrientUp();
            CheckBoost();
            HandleBoost();
		}
	}

    void HandleControls()
    {
        if (!usingController) {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddRelativeForce(Vector3.forward * speed);

            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddRelativeForce(-Vector3.forward * speed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (turnSpeed < 60)
                    turnSpeed += 5f;
                //transform.RotateAround(transform.position, Vector3.up, -turnSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
                turning = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (turnSpeed < 60)
                    turnSpeed += 5f;
                //transform.RotateAround(transform.position, Vector3.up, -turnSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                turning = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !boosting)
            {
                    boosting = true;

            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                boosting = false;
            }

			if (Input.GetKeyUp (KeyCode.D)) {
				turning = false;
			}

			if (Input.GetKeyUp (KeyCode.A)) {
				turning = false;
			}
		} else {
			if (Input.GetAxis (playerVertAxis) > 0.5) {
				rb.AddRelativeForce (Vector3.forward * speed);
			}

			//Commented out reversing for now.
//			if (Input.GetAxis (playerVertAxis) < -0.5) {
//				rb.AddRelativeForce (-Vector3.forward * speed);
//
//			}

			if (Input.GetAxis (playerHorizAxis) < -0.5) {
				if (turnSpeed < 60)
					turnSpeed += 5f;
				//transform.RotateAround(transform.position, Vector3.up, -turnSpeed * Time.deltaTime);
				transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
				turning = true;
			}

			if (Input.GetAxis (playerHorizAxis) > 0.5) {
				if (turnSpeed < 60)
					turnSpeed += 5f;
				//transform.RotateAround(transform.position, Vector3.up, -turnSpeed * Time.deltaTime);
				transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
				turning = true;
			}


			if (turning && Mathf.Abs (Input.GetAxis (playerHorizAxis)) < 0.5) {
				turning = false;
			}
		}

        if (!turning)
        {
            if (turnSpeed > 0) 
                turnSpeed -= 2.0f;
        }

    }

    void OrientUp()
    {
        Ray rayToGround = new Ray();
        rayToGround.origin = transform.position;
        rayToGround.direction = Vector3.down;

        Debug.DrawRay(rayToGround.origin, rayToGround.direction * 10, Color.red);

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(rayToGround, out hitInfo,Mathf.Infinity, LayerMask.GetMask("HitSurface"));
        
        if (hit)
        {
 
            if (Vector3.Distance(Vector3.Cross(transform.right, transform.forward), Vector3.Cross(hitInfo.transform.right,  hitInfo.transform.forward)) > orientTolerance)
            {
                Debug.Log("Orienting...");

                transform.up = Vector3.RotateTowards(transform.up, Vector3.Cross(hitInfo.transform.right, -hitInfo.transform.forward), 2.0f * Time.deltaTime, 0.0f);
            }

        }
    }

    void GetWheels()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Wheel")
            {
                wheels.Add(child.gameObject);
            }
        }
    }

    void SpinWheels()
    {
        foreach (GameObject g in wheels)
        {
           
            g.transform.Rotate(Vector3.right, -rb.velocity.z * 4.0f * Time.deltaTime);
            
        }

        
    }

	void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Pickups"))
        {
            Destroy(collider.gameObject);
            crosshairScript.addAmmo(5);
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer ("StartLine")) {
            if (!startedRace)
                startedRace = true;
            if (startedRace)
            {
                lapManager.incrementLapCounter(playerNum);
            }
			
		}

		if (collider.gameObject.layer == LayerMask.NameToLayer ("MidLine")) {
			lapManager.incrementMidCounter (playerNum);
		}
	}

    void CheckBoost()
    {
        if (turnSpeed > 50 && boost++ < maxBoost && !boosting)
        {
            
            boost++;
            Debug.Log(boost);
        }
    }

    void HandleBoost()
    {
        if (boosting && boost > 0)
        {
            speed = boostedSpeed;
            boost -= 5;
        }

        if (!boosting || boost <= 0)
        {
            speed = baseSpeed;
        }
    }
}
