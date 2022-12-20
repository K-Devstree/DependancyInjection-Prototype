using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseEnemy : MonoBehaviour
{
	[Inject]
	IScoreModal scoreModal;

	[SerializeField]
	protected int point = 100;
	protected float speed = 1;
	protected bool isAttacking = false;
	protected Vector3 direction;
	protected Vector3 position;
	protected EnemyWave enemyWave;

	protected AudioSource audioSource;
	[SerializeField]
	protected AudioClip attackAudioClip;
	[SerializeField]
	protected AudioClip explodeAudioClip;

	protected virtual void Start()
	{
		enemyWave = GetComponentInParent<EnemyWave>();
		position = transform.position - enemyWave.GetPosition();
		speed = enemyWave.GetSpeed();
		audioSource = GetComponent<AudioSource>();
	}

	protected virtual void Update()
	{
		if (isAttacking)
		{
			OnAttacking();
		}
		else if (enemyWave != null)
		{
			transform.position = Vector3.Lerp(transform.position, enemyWave.GetPosition() + position, Time.deltaTime);
		}
	}

	private void OnBecameInvisible()
	{
		isAttacking = false;
		Vector3 pos = transform.position;
		pos.y = 9;
		transform.position = pos;
	}

	protected virtual void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Bullet")
		{
			PlayAudio(explodeAudioClip, false);
			int point = scoreModal.GetPoint();
			scoreModal.SetPoint(this.point + point);
			CallEnemyWave();
			Destroy(coll.gameObject);
			Destroy(gameObject);
		}
	}

	protected virtual void OnAttacking()
	{
		transform.Translate(direction * Time.deltaTime * speed, Space.Self);
	}

	public virtual void Attack()
	{
		if (isAttacking)
		{
			return;
		}
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
		{
			return;
		}
		direction = player.transform.position - transform.position;
		direction.Normalize();
		isAttacking = true;
		PlayAudio(attackAudioClip, false);
	}

	private void CallEnemyWave()
	{
		EnemyWave wave = GetComponentInParent<EnemyWave>();
		if (wave != null)
		{
			wave.DestoryEnemy(this);
		}
	}

	private void PlayAudio(AudioClip audioClip, bool loop)
	{
		audioSource.clip = audioClip;
		audioSource.loop = loop;
		audioSource.Play();
	}
}
