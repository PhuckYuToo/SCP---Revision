using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	private AnimationClip still, walk, run, fire, checkMag, reload, grab, pickup;

	public Light muzzleLight;

	private int aimInt = 0, reloadInt = 0, checkMagInt = 0, itemInt = 0;

	public Transform playerObj, gunCamera;
	private Player player;

	public Transform cameraObj;

	public Transform gunPos, aimPos;
	public float aimSpeed = 5f;

	[HideInInspector]
	public bool canRun = true;

	private CharacterController ch;

	public Transform bulletText;
	private float reloadAnim = 0;

	public float shootDelay, range = 15f, damage = 1f, hipFireSpread = 0.04f;
	private float shootInt = 0;
	public int startBullets, startMags;
	private int bullets, mags;

	public GameObject spark, mag;

	private GUITexture crosshair;
	private GUIText pickupText;

	private int oldBullets = 0;
	
	private Vector3 lastPos;

	void Start()
	{
		still = animation.GetClip("N4A1_Still");
		walk = animation.GetClip("N4A1_Walk");
		run = animation.GetClip("N4A1_Run");
		fire = animation.GetClip("N4A1_Shoot");
		checkMag = animation.GetClip("N4A1_CheckMag");
		reload = animation.GetClip("N4A1_Reload");
		grab = animation.GetClip("N4A1_Grab");
		pickup = animation.GetClip("N4A1_Pickup");

		player = playerObj.GetComponent<Player>();
		ch = playerObj.GetComponent<CharacterController>();
		bullets = startBullets;
		mags = startMags;

		crosshair = GameObject.Find("HUD/MTF/Reticle").guiTexture;
		pickupText = GameObject.Find("HUD/MTF/ItemPickup").guiText;

		lastPos = playerObj.transform.position;
	}

	public void WeaponUpdate()
	{
		RaycastHit item;
		if(reloadInt == 0 && checkMagInt == 0 && Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out item, 3f) && item.transform.GetComponent<Pickable>())
		{
			Pickable pick = item.transform.GetComponent<Pickable>();
			pickupText.text = "Press F To Pickup " + pick.itemName;
			itemInt = 1;
			if(Input.GetButton("Interact") && itemInt == 1)
			{
				animation.CrossFade(pickup.name);
				itemInt = 2;
				pick.OnPickup(playerObj);
				Destroy(item.transform.gameObject);
			}
			else animation.CrossFade(grab.name);
		}
		else if(itemInt != 2)
		{
			if(pickupText) pickupText.text = "";
			itemInt = 0;
		}
		if(pickup && !animation.IsPlaying(pickup.name) && itemInt == 2) itemInt = 0;

		if(!Input.GetMouseButton(1) && !Input.GetMouseButton(0) && aimInt == 0 && reloadInt == 0 && checkMagInt == 0) canRun = true;

		if(Input.GetMouseButton(1) && itemInt == 0 && ch.isGrounded && reloadInt == 0 && checkMagInt == 0)
		{
			transform.position = Vector3.Lerp(transform.position, aimPos.position, Time.deltaTime * aimSpeed);
			animation[still.name].time = 0f;
			animation[still.name].speed = 0f;
			animation.CrossFade(still.name);
			canRun = false;
			crosshair.enabled = false;
		}
		else 
		{
			transform.position = Vector3.Lerp(transform.position, gunPos.position, Time.deltaTime * aimSpeed);
			animation[still.name].speed = 1f;
			crosshair.enabled = true;
		}
		if(Input.GetMouseButton(0) && itemInt == 0 && checkMagInt == 0 && reloadInt == 0 && shootInt <= 0 && bullets > 0 && ch.enabled)
		{
			animation[fire.name].time = 0;
			shootInt = shootDelay;
			animation.Play(fire.name);
			muzzleLight.enabled = true;
			cameraObj.GetComponent<Recoil>().Shoot();
			canRun = false;
			shoot();
		}
		else muzzleLight.enabled = false;

		if(shootInt > 0) shootInt--;

		if(!player.isMoving() && aimInt == 0 && reloadInt == 0 && checkMagInt == 0 && !Input.GetMouseButton(0) && !Input.GetButton("Ammo") && !Input.GetMouseButton(1) && ch.isGrounded && itemInt == 0) animation.CrossFade(still.name);

		if(Input.GetButtonDown("Reload") && itemInt == 0 && bullets < startBullets && ch.isGrounded && !animation.IsPlaying(reload.name) && checkMagInt == 0 && !Input.GetButton("Run") && mags > 0)
		{
			animation[reload.name].speed = 1.3f;
			animation.CrossFade(reload.name);
			reloadInt = 1;
			canRun = false;
			reloadMag();
		}

		if(animation.IsPlaying(reload.name) && reloadInt == 1)
		{
			Transform magBone = transform.Find("Armature/Gun/Mag/BulletCount");
			GameObject magOBJ = (GameObject)Instantiate(mag, magBone.position, magBone.rotation);
			magOBJ.rigidbody.detectCollisions = false;
			magOBJ.transform.position = magBone.position;
			magOBJ.rigidbody.isKinematic = true;
			magOBJ.transform.localPosition -= magBone.right * 2.4f;
			magOBJ.transform.localPosition += magBone.up * -0.55f;
			magOBJ.transform.localPosition += magBone.forward * 1f;
			magOBJ.transform.rotation = magBone.rotation;
			magOBJ.rigidbody.useGravity = true;
			magOBJ.rigidbody.isKinematic = false;
			magOBJ.rigidbody.detectCollisions = true;
			magOBJ.GetComponent<Mag>().bullets = oldBullets;
			reloadInt = 2;
		}

		if(reloadInt == 2 && !animation.IsPlaying(reload.name)) reloadInt = 0;

		//checkMag
		if(Input.GetButton("Ammo") && itemInt == 0 && ch.isGrounded && !animation.IsPlaying(checkMag.name) && checkMagInt == 0 && reloadInt == 0 && !Input.GetButton("Run"))
		{
			animation[checkMag.name].speed = 1.3f;
			animation.CrossFade(checkMag.name);
			checkMagInt = 1;
			canRun = false;
		}
		if(Input.GetButton("Ammo") && animation[checkMag.name].time > animation[checkMag.name].length) animation[checkMag.name].time = animation[checkMag.name].length;
		reloadAnim = Mathf.Clamp((animation[checkMag.name].time / animation[checkMag.name].length), 0f, 1f);

		bulletText.GetComponent<TextMesh>().color = new Color(255, 255, 255, reloadAnim);
		bulletText.GetChild(0).GetComponent<TextMesh>().color = new Color(0, 0, 0, reloadAnim);
		bulletText.GetComponent<TextMesh>().text = bulletText.GetChild(0).GetComponent<TextMesh>().text = bullets + "/" + mags;

		if(!Input.GetButton("Ammo") && checkMagInt == 1)
		{
			animation[checkMag.name].speed = -1.3f;
			checkMagInt = 2;
		}
		if(animation[checkMag.name].time <= 0 && checkMagInt == 2)
		{
			checkMagInt = 0;
			animation[checkMag.name].time = 0;
		}
	}

	public void FixedWeaponUpdate()
	{
		if(ch.isGrounded && isMoving() && itemInt == 0 && !Input.GetMouseButton(1) && checkMagInt == 0 && reloadInt == 0 && shootInt == 0)
		{
			animation.Stop(still.name);
			if(Input.GetButton("Run") && !playerObj.GetComponent<PlayerMove>().isCrouching) animation.Play(run.name);
			else animation.Play(walk.name);
		}
	}

	private bool isMoving()
	{
		Vector3 displacement = playerObj.transform.position - lastPos;
		lastPos = playerObj.transform.position;
		displacement = new Vector3(displacement.x, 0, displacement.z);
		return displacement.magnitude > 0.001;
	}

	private void shoot()
	{
		if(bullets > 0) bullets--;
		else bullets = 0;

		float randX = 0f, randY = 0f;
		if(crosshair.enabled)
		{
			randX = Random.Range(-hipFireSpread, hipFireSpread);
			randY = Random.Range(-hipFireSpread, hipFireSpread);
		}

		RaycastHit hit;
		if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f + randX, 0.5f + randY, 0f)), out hit, range))
		{
			if(hit.rigidbody && hit.transform.GetComponent<OnShot>())
			{
				hit.transform.GetComponent<OnShot>().OnShoot(damage, hit);
				if(hit.transform.GetComponent<OnShot>().knockback) hit.rigidbody.AddForceAtPosition(-2000f * transform.forward, hit.point);
			}
			if(!hit.transform.GetComponent<OnShot>() || hit.transform.GetComponent<OnShot>().defaultParticle)
			{
				GameObject obj = (GameObject)Instantiate(spark, hit.point, Quaternion.identity);
				if(hit.transform.renderer && hit.transform.renderer.material.HasProperty("_Color")) obj.transform.FindChild("Main").renderer.material.SetColor("_TintColor", hit.transform.renderer.material.color);
				else if(hit.transform.renderer && hit.transform.renderer.material.HasProperty("_SpecColor")) obj.transform.FindChild("Main").renderer.material.SetColor("_TintColor", hit.transform.renderer.material.GetColor("_SpecColor"));
				else obj.transform.FindChild("Main").renderer.material.SetColor("_TintColor", new Color(0.8f, 0.3f, 0f, 1f));
			}
		}
	}

	private void reloadMag()
	{
		oldBullets = bullets;
		bullets = startBullets;
		if(mags > 0) mags--;
		else mags = 0;
	}

	public void AddMag()
	{
		mags++;
	}
}