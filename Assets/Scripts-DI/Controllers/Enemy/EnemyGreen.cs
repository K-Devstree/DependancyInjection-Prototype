using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGreen : BaseEnemy
{
	[SerializeField]
	private GameObject beam;

	private Animator animator;
	private bool isStop = false;
	private bool isBeam = false;

	private void Start()
	{
		base.Start();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (!isStop)
		{
			base.Update();
		}
	}

	public override void Attack()
	{
		if (!this.isAttacking)
		{
			isBeam = false;
			isStop = false;
		}
		base.Attack();
	}

	protected override void OnAttacking()
	{
		base.OnAttacking();
		if (!isBeam && transform.position.y < -2)
		{
			isStop = true;
			Instantiate(beam, transform);
			Invoke("HaveBeam", 3f);
		}
	}

	protected override void OnCollisionEnter2D(Collision2D coll)
	{
		if (!animator.GetBool("haveHit") && coll.gameObject.tag == "Bullet")
		{
			Animator animator = GetComponent<Animator>();
			animator.SetBool("haveHit", true);
			Destroy(coll.gameObject);
		}
		else
		{
			base.OnCollisionEnter2D(coll);
		}
	}

	private void HaveBeam()
	{
		this.isBeam = true;
		isStop = false;
	}
}
