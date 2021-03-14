using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCountdown : MonoBehaviour {

    public float secondsRemaining = 480;
    public bool hasExploded = false;
    private TextMesh text;

	// Use this for initialization
	void Start()
    {
        text = GetComponent<TextMesh>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!hasExploded)
        {
            if (secondsRemaining > 0)
            {
                secondsRemaining -= Time.deltaTime;
                text.text = secondsRemaining.ToString();
            }
            else
            {
                text.text = "BOOM";
                hasExploded = true;
            }
        }
	}
}
