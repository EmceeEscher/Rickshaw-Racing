using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapManagerScript : MonoBehaviour {

    public int countdown;
	public float countdownDelay = 1.0f;
    public int numLaps;

	private float prevTime;
	private bool hasStarted = false;
	private bool hasFinished = false;
	private string countdownText;
	private GUIStyle countdownStyle;
	private int countdownFontSize = 100;
	private int P1LapCounter = 0;
	private int P1MidCounter = 1;
	private int P2LapCounter = 0;
	private int P2MidCounter = 1;


    public Text lapText;

	// Use this for initialization
	void Start () {
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("P1Rickshaw"), LayerMask.NameToLayer ("P1Bullets"));
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("P2Rickshaw"), LayerMask.NameToLayer ("P2Bullets"));
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("P1Bullets"), LayerMask.NameToLayer ("Pickups"));
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("P2Bullets"), LayerMask.NameToLayer ("Pickups"));

		prevTime = Time.time;
		countdownText = "" + countdown;
		countdownStyle = new GUIStyle ();
		countdownStyle.fontSize = countdownFontSize;
		countdownStyle.alignment = TextAnchor.MiddleCenter;

        lapText.text = "Lap: " + P1LapCounter + " / " + numLaps;
    }
	
	// Update is called once per frame
	void Update () {
		if (countdown > 1 && (Time.time - prevTime) > countdownDelay) {
			countdown--;
			prevTime = Time.time;
			countdownText = "" + countdown;
		}

		if (countdown == 1 && (Time.time - prevTime) > countdownDelay) {
			countdown--;
			prevTime = Time.time;
			countdownText = "GO!";
			hasStarted = true;
		}

		if (countdown <= 0 && (Time.time - prevTime) > countdownDelay) {
			countdownText = "";
		}

		if (P1LapCounter == numLaps + 1) {
			hasFinished = true;
			hasStarted = false;
			countdownText = "P1 wins!";
		}

		if (P2LapCounter == numLaps + 1) {
			hasFinished = true;
			hasStarted = false;
			countdownText = "P2 wins!";
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(
			Screen.width / 2, 
			Screen.height / 2, 
			100, 
			50), countdownText, countdownStyle);
	}

	public bool hasRaceStarted() {
		return hasStarted;
	}

	public bool hasRaceFinished() {
		return hasFinished;
	}

	public void incrementLapCounter(int playerNum) {
		if (playerNum == 1) {
			if (P1LapCounter < P1MidCounter) {
				P1LapCounter++;
			}
			if (P1LapCounter < numLaps + 1) {
				lapText.text = "Lap: " + P1LapCounter + " / " + numLaps;
			}
		} else if (playerNum == 2) {
			if (P2LapCounter < P2MidCounter) {
				P2LapCounter++;
			}
			if (P2LapCounter < numLaps + 1) {
				lapText.text = "Lap: " + P2LapCounter + " / " + numLaps;
			}
		}
        
        
	}

	public void incrementMidCounter(int playerNum) {
		if (playerNum == 1) {
			if (P1LapCounter == P1MidCounter) {
				P1MidCounter++;
			}
		} else if (playerNum == 2) {
			if (P2LapCounter == P2MidCounter) {
				P2MidCounter++;
			}
		}
	}
}
