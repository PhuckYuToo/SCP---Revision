using UnityEngine;
using System.Collections;

public class OnShot : MonoBehaviour
{
	public float maxHealth = 10;
	protected float health;
	public bool invincible = false;
	public bool knockback = true;

	private bool dead = false;

	public bool defaultParticle = true;

	void Awake()
	{
		health = maxHealth;
	}

	public virtual void OnShoot(float damage, RaycastHit hit)
	{
		if(!invincible)
		{
			health = Mathf.Clamp(health - damage, 0, health);
			if(health == 0 && !dead) OnDeath();
		}
	}

	protected virtual void OnDeath()
	{
		dead = true;
	}

	public void reset()
	{
		dead = false;
	}
}
