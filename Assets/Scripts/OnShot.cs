using UnityEngine;
using System.Collections;

public class OnShot : MonoBehaviour
{
	public float maxHealth = 10;
	protected float health;
	public bool invincible = false;

	void Awake()
	{
		health = maxHealth;
	}

	public virtual void OnShoot(float damage, RaycastHit hit)
	{
		if(!invincible)
		{
			health = Mathf.Clamp(health - damage, 0, health);
			if(health <= 0) OnDeath();
		}
	}

	protected virtual void OnDeath()
	{

	}
}
