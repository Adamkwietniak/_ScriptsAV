using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RingManagerScript : MonoBehaviour
{

	private Transform transformRing;
	public static Transform neckPoint;
	private float force = 100000.0f;
	private float radius = 5.0f;
	[HideInInspector]public float upwardsModifer = 0.0f;
	public ForceMode forceMode;
	[HideInInspector]public Rigidbody[] spheresRb;
	public Text commentText;
	// używany w "Perfect!"
	private GameObject Aviator;
	MissionManagerScript missionManager;
	private float distance;
	public LineRenderer lineRendererObject;
	private Color defaultColor = Color.green;
	private Color newColor = Color.red;



	// Use this for initialization
	void Awake ()
	{
		Aviator = GameObject.Find ("WingsuitAviator");
		spheresRb = GetComponentsInChildren<Rigidbody> ();
		missionManager = FindObjectOfType<MissionManagerScript> ();
		if (neckPoint == null) {
			neckPoint = GameObject.FindGameObjectWithTag ("Player").transform.FindChild ("Root/Ribs/Chest/Neck/Neck/Head");
		}
	}

	void Start ()
	{
		transformRing = GetComponent<Transform> ();
		distance = Vector3.Distance (neckPoint.position, transformRing.position);
	}

	void Update ()
	{
		//transformRing.transform.Rotate (Vector3.left * Time.deltaTime * 80);
		if (commentText.enabled == true) {
			StartCoroutine (ResetCommentText (0.3f));
		}

		if (distance < 5) {
			lineRendererObject.material = new Material (Shader.Find ("Particles/Alpha Blended"));
			lineRendererObject.SetColors (defaultColor, newColor);
			//lineRendererObject = Color.Lerp (Color.green, Color.red, Mathf.PingPong (Time.time, 1));
			Debug.Log ("1");	
		}

	}

	void OnTriggerEnter (Collider other)
	{

		if (other.tag == "Player") {
			missionManager.numberOfRingsCollected++;
			Destroy (gameObject, 1f);	
			foreach (Rigidbody rb in spheresRb) {
				rb.AddExplosionForce (force, transform.position, radius, upwardsModifer, forceMode);
			}

			if (distance < 2.5 && distance > 0) {
				commentText.enabled = true;
				commentText.text = "PERFECT!";
			} else if (distance > 2.5 && distance < 3.5) {
				commentText.enabled = true;
				commentText.text = "GOOD!";
			} else if (distance > 3.5) {
				commentText.enabled = true;
				commentText.text = "OKAY!";
			}
		}


	}

	public IEnumerator ResetCommentText (float time)
	{
		yield return new WaitForSeconds (time);
		commentText.enabled = false;
		yield return null;
	}

}
