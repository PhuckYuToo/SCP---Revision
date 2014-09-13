using UnityEngine;
using System.Collections;

public class SCP106 : OnShot
{
	public Transform player, target, nodes;
	private Transform torso, armL, armR, neck, head, node = null;
	private AnimationClip still, walk, eat, pocket, grab, hit, enterWall, exitWall, walkHands;

	public float moveSpeed = 2f;

	public bool hunt = true, retreat = false;
	private bool hasTarget = true, isHiding = false;
	private Vector3 lastPosition;

	private int hidingStage = 0, previousNode = -1, hitStage = 0;

	public GameObject blood, corrosion;

	void Start()
	{
		still = animation.GetClip("106_Standing");
		walk = animation.GetClip("106_Walk");
		eat = animation.GetClip("106_Eat");
		pocket = animation.GetClip("106_PocketDimDrag");
		grab = animation.GetClip("106_grab");
		hit = animation.GetClip("106_hit");
		enterWall = animation.GetClip("106_EnterWall1");
		exitWall = animation.GetClip("106_ExitWall1");
		walkHands = animation.GetClip("106_Walk2");

		head = transform.Find("Armature/Neck/Head");
		torso = transform.Find("Armature/Torso");
		armL = transform.Find("Armature/Shoulder_L");
		armR = transform.Find("Armature/Shoulder_R");
		neck = transform.Find("Armature/Neck");
		animation[walk.name].AddMixingTransform(torso);
		animation[walk.name].AddMixingTransform(neck, false);
		animation[grab.name].AddMixingTransform(armL);
		animation[grab.name].AddMixingTransform(armR);
		animation[walkHands.name].AddMixingTransform(armL);
		animation[walkHands.name].AddMixingTransform(armR);
		animation[walkHands.name].layer = 1;
		animation[grab.name].layer = 1;
	}
	
	void Update()
	{
		float dist = (transform.position - target.position).magnitude;
		if(hasTarget && hunt && hitStage == 0 && !retreat)
		{
			Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
			lastPosition = transform.position;
			animation[walk.name].speed = velocity.magnitude * 0.15f;

			float xPos = Mathf.Lerp(transform.position.x, target.transform.position.x - transform.forward.x, moveSpeed * 2 * Time.deltaTime);
			float yPos = Mathf.Lerp(transform.position.y, target.transform.position.y - 2f, moveSpeed * 2 * Time.deltaTime);
			float zPos = Mathf.Lerp(transform.position.z, target.transform.position.z - transform.forward.z, moveSpeed * 2 * Time.deltaTime);

			transform.position = new Vector3(xPos, yPos, zPos);
			Vector3 finalVec = target.position - transform.position;
			finalVec.y = 0;
			transform.rotation = Quaternion.LookRotation(finalVec);
			head.LookAt(target);
			head.Rotate(new Vector3(10, 0, -90));

			if(dist >= 2.4f && dist < 3.4f && hunt) animation.CrossFade(grab.name);
			else animation.CrossFade(walkHands.name);
			animation.CrossFade(walk.name);
		}
		else if(hitStage == 0 && !retreat) animation.CrossFade(still.name);

		if(dist < 2.4f && !retreat && hunt && hitStage == 0) hitStage = 1;

		if(hitStage == 1)
		{
			if(player.tag.Equals("Player"))
			{
				if(!player.GetComponent<CharacterController>().isGrounded)
				{
					hitStage = 0;
					return;
				}
				animation.Stop(walk.name);
				animation.Stop(walkHands.name);
				animation.Stop(grab.name);
				animation.CrossFade(hit.name);
				player.GetComponent<CharacterMotor>().enabled = false;
				player.GetComponent<Player>().lockMouseMovement = true;
				player.GetComponent<Player>().enabled = false;
				player.position = transform.FindChild("Strangle").transform.position;

				Transform cam = player.FindChild("Camera").FindChild("Main Camera").transform;
				cam.LookAt(head);
				cam.Rotate(new Vector3(10, 0, 0));
			}
			else
			{
				animation.Stop(walk.name);
				animation.Stop(walkHands.name);
				animation.Stop(grab.name);
				animation.CrossFade(hit.name);
				player.position = transform.FindChild("Strangle").transform.position;
				player.LookAt(head);
				hitStage = 2;
			}
		}

		if(retreat && !node)
		{
			foreach(GameObject na in nodes.GetComponent<Nodes>().nodes)
			{
				Transform n = na.transform;
				if(node == null && n.gameObject.activeSelf) node = n;
				else if(node != null)
				{
					float distance = (transform.position - n.position).magnitude;
					float distancePrev = (transform.position - node.position).magnitude;
					if(distance < distancePrev && n.gameObject.activeSelf) node = n;
				}
			}
		}
		if(retreat && node && !isHiding)
		{
			if(hidingStage == 0 && (int)transform.position.x == (int)node.position.x && (int)transform.position.z == (int)node.position.z)
			{
				animation.Stop(walk.name);
				animation.Stop(walkHands.name);
				animation.CrossFade(enterWall.name);
				RaycastHit hit;
				if(Physics.Raycast(head.position, head.forward, out hit)) Instantiate(corrosion, hit.point + hit.normal * 0.1f, Quaternion.Euler(node.rotation.eulerAngles.x - 90, node.rotation.eulerAngles.y, node.rotation.eulerAngles.z));
				hidingStage = 1;
			}
			else if(hidingStage == 0)
			{
				float xPos = Mathf.Lerp(transform.position.x, node.transform.position.x, moveSpeed * Time.deltaTime);
				float zPos = Mathf.Lerp(transform.position.z, node.transform.position.z, moveSpeed * Time.deltaTime);
				
				transform.position = new Vector3(xPos, transform.position.y, zPos);
				animation.CrossFade(walk.name);
				animation.CrossFade(walkHands.name);
			}
			if(!animation.IsPlaying(enterWall.name) && hidingStage == 1)
			{
				hidingStage = 2;
				isHiding = true;
				Vector3 center = node.parent.transform.renderer.bounds.center;
				transform.position = new Vector3(center.x, transform.position.y, center.z);
				animation.Stop(enterWall.name);
				animation.CrossFade(still.name);
				previousNode = findNode(node);
			}
			else if(!isHiding) transform.rotation = node.rotation;
		}

		if(isHiding && Random.Range(0, 10) == 3 && hidingStage > 0 && hidingStage != 4 && hidingStage != 3)
		{
			hidingStage = 4;
			int rand = RandomRangeExcept(0, nodes.GetComponent<Nodes>().nodes.Count, previousNode);
			GameObject posA = (GameObject)nodes.GetComponent<Nodes>().nodes[rand];
			Transform pos = posA.transform;

			transform.position = new Vector3(pos.position.x, transform.position.y, pos.position.z);
			Vector3 finalVec = pos.parent.transform.position - transform.position;
			finalVec.y = 0;
			transform.rotation = Quaternion.LookRotation(finalVec);
			transform.Rotate(0, 180, 0);
			transform.position = new Vector3(pos.position.x, transform.position.y, pos.position.z) + (transform.forward * -1);
			RaycastHit hit;
			if(Physics.Raycast(head.position + head.forward, head.forward * -1f, out hit)) Instantiate(corrosion, hit.point + hit.normal * 0.1f, Quaternion.Euler(pos.rotation.eulerAngles.x - 90, pos.rotation.eulerAngles.y, pos.rotation.eulerAngles.z));
		}

		if(isHiding && hidingStage > 0)
		{
			if(hidingStage == 4)
			{
				animation.Play(exitWall.name);
				hidingStage = 3;
			}
			if(hidingStage == 3 && !animation.IsPlaying(exitWall.name))
			{
				transform.position += transform.forward;
				isHiding = false;
				retreat = false;
				hunt = true;
				node = null;
				health = maxHealth;
				invincible = false;
				hidingStage = 0;
			}
		}
	}

	private int RandomRangeExcept(int min, int max, int except) 
	{
		GameObject obj;
		int number;
		int maxNodes = nodes.GetComponent<Nodes>().nodes.Count;
		int count = 0;
		do
		{
			count++;
			number = Random.Range(min, max);
			obj = (GameObject)nodes.GetComponent<Nodes>().nodes[number];
			if(count > maxNodes)
			{
				number = -1;
				break;
			}
		}
		while(number == except || !obj.activeSelf);
		return number;
	}

	private int findNode(Transform trans)
	{
		for(int i = 0; i < nodes.GetComponent<Nodes>().nodes.Count; i++)
		{
			Transform n = ((GameObject)nodes.GetComponent<Nodes>().nodes[i]).transform;
			if(n.position.x == trans.position.x && n.position.y == trans.position.y && n.position.z == trans.position.z) return i;
		}
		return -1;
	}

	protected void Retreat()
	{
		hunt = false;
		retreat = true;
	}

	protected override void OnDeath()
	{
		invincible = true;
		hunt = false;
		retreat = true;
	}

	public override void OnShoot(float damage, RaycastHit hit)
	{
		base.OnShoot(damage, hit);
		Instantiate(blood, hit.point, Quaternion.identity);
	}
}
