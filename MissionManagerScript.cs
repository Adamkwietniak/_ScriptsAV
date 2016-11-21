using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionManagerScript : MonoBehaviour
{

	public Text displayCollectedRings;
	public int maximumRings;
	[HideInInspector]public int numberOfRingsCollected;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		displayCollectedRings.text = "Rings collected: " + numberOfRingsCollected + "/" + maximumRings;	
	}
}
