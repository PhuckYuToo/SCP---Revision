using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour 
{
	public float walkSpeed = 7f; // regular speed
	public float crchSpeed = 3f; // crouching speed
	public float runSpeed = 20f; // run speed
	
	private CharacterMotor chMotor;
	private CharacterController ch;
	private Transform tr;
	private float height; // initial height

	[HideInInspector]
	public bool isCrouching = false;
	
	void Start()
	{
		chMotor = GetComponent<CharacterMotor>();
		tr = transform;
		ch = GetComponent<CharacterController>();
		height = ch.height;
	}
	
	void Update()
	{
		float h = height;
		float speed = walkSpeed;
		
		if(ch.isGrounded && Input.GetButton("Run") && transform.GetComponent<Player>().weapon.GetComponent<Weapon>().canRun) speed = runSpeed;
		if(Input.GetButton("Crouch") && chMotor.enabled)
		{
			h = 0.5f * height;
			speed = crchSpeed; // slow down when crouching
			isCrouching = true;
		}
		else isCrouching = false;
		chMotor.movement.maxForwardSpeed = speed; // set max speed
		var lastHeight = ch.height; // crouch/stand up smoothly 
		ch.height = Mathf.Lerp(ch.height, h, 5 * Time.deltaTime);

		//tr.position.y += (ch.height-lastHeight)/2; // fix vertical position
		tr.position = new Vector3(tr.position.x, tr.position.y + ((ch.height-lastHeight)/2), tr.position.z);
	}
}
