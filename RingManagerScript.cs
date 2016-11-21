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
	//private Animator anim;

	// Use this for initialization
	void Awake ()
	{
		Aviator = GameObject.Find ("WingsuitAviator");
		//Aviator = GetComponent<Transform> ();
		spheresRb = GetComponentsInChildren<Rigidbody> ();
		missionManager = FindObjectOfType<MissionManagerScript> ();
		//anim = GetComponent<Animator> ();
		if (neckPoint == null) {
			neckPoint = GameObject.FindGameObjectWithTag ("Player").transform.FindChild ("Root/Ribs/Chest/Neck/Neck/Head");
		}
	}

	void Start ()
	{
		transformRing = GetComponent<Transform> ();
		//anim.enabled = false;

	}

	/*void EnableAnimation ()
	{
		float dist = Vector3.Distance (Aviator.transform.position, this.transform.position);
		if (dist < 5) {
			anim.enabled = true;
		}
	}*/

	// Update is called once per frame
	void Update ()
	{
		//transformRing.transform.Rotate (Vector3.left * Time.deltaTime * 80);
		//EnableAnimation ();
		if (commentText.enabled == true) {
			StartCoroutine (ResetCommentText (0.5f));
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

			float distance = Vector3.Distance (neckPoint.position, transformRing.position);

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
