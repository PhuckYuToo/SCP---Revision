using UnityEngine;
using System.Collections;

public class Recoil : MonoBehaviour
{
	public float force = 2.5f; // controls recoil amplitude
	public float upSpeed = 9f; // controls smoothing speed
	public float dnSpeed = 20f; // how fast the weapon returns to original position
	
	private Vector3 ang0; // initial angle
	private float targetX; // unfiltered recoil angle
	private Vector3 ang = Vector3.zero; // smoothed angle
	
	void Start()
	{
		ang0 = transform.localEulerAngles; // save original angles
	}
	
	public void Shoot()
	{
		targetX += force; // add recoil force
	}
	
	void Update()
	{ // smooth movement a little
		ang.x = Mathf.Lerp(ang.x, targetX, upSpeed * Time.deltaTime);
		transform.localEulerAngles = ang0 - ang; // move the camera or weapon
		targetX = Mathf.Lerp(targetX, 0, dnSpeed * Time.deltaTime); // returns to rest
	}
}
