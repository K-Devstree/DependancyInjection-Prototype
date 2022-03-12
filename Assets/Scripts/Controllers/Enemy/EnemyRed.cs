using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRed : BaseEnemy
{
	[SerializeField]
	private GameObject bullet;

	private void Start()
	{
		base.Start();
	}

	private void Update()
	{
		base.Update();
	}

	public override void Attack()
	{
		if (!this.isAttacking)
		{
			Invoke("InstantiateBullet", 1f);
		}
		base.Attack();
	}

	private void InstantiateBullet()
	{
		if (this.isAttacking)
		{
			Instantiate(bullet, transform);
			Invoke("InstantiateBullet", 1f);
		}
	}
}
