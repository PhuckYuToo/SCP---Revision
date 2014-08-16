using UnityEngine;
using System.Collections;

public class Sun : MonoBehaviour
{
	public Light sunLight;
	public Transform player;
	public float speed = 0.01f;

	// Update is called once per frame
	void Update ()
	{
		if(transform.rotation.eulerAngles.x > 180 || transform.rotation.eulerAngles.x < 0) sunLight.enabled = false;
		else sunLight.enabled = true;

		transform.Rotate(new Vector3(speed, 0, 0));

		transform.localPosition = new Vector3(0, 0, player.position.z);
	}
}
