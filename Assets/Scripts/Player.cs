using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public bool lockMouseMovement;

	//private MouseLook yLook, xLook;
	public MouseLook look;
	public Transform weapon;
	
	private Vector3 lastPos;

	// Use this for initialization
	void Start()
	{
		//xLook = GetComponent<MouseLook>();
		//yLook = transform.FindChild("Main Camera").GetComponent<MouseLook>();
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(lockMouseMovement)
		{
			look.enabled = false;
			look.enabled = false;
		}
		else
		{
			look.enabled = true;
			look.enabled = true;
			weapon.GetComponent<Weapon>().WeaponUpdate();
		}
	}

	void FixedUpdate()
	{
		if(!lockMouseMovement) weapon.GetComponent<Weapon>().FixedWeaponUpdate();
	}

	public bool isMoving()
	{
		Vector3 displacement = transform.position - lastPos;
		lastPos = transform.position;
		return displacement.magnitude > 0.001; // return true if char moved 1mm
	}
}
