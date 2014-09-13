using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public bool lockMouseMovement;

	//private MouseLook yLook, xLook;
	public MouseLook look;
	public Transform weapon;

	private Vector3 lastPos;

	void Start()
	{
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
			if(weapon.GetComponent<Weapon>()) weapon.GetComponent<Weapon>().WeaponUpdate();
		}
	}

	void FixedUpdate()
	{
		if(!lockMouseMovement && weapon.GetComponent<Weapon>()) weapon.GetComponent<Weapon>().FixedWeaponUpdate();
	}

	public bool isMoving()
	{
		Vector3 disp = transform.position - lastPos;
		lastPos = transform.position;
		return disp.magnitude > 0.001;
	}
}
