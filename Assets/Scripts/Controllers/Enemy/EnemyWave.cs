using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyWave : MonoBehaviour
{
	[Inject]
	IStateModal stateModal;

	private GameController gameController;
	private List<BaseEnemy> enemies;

	[SerializeField]
	private float minAttractRange = 0.5f;
	[SerializeField]
	private float maxAttractRange = 2f;

	private float speed = 2;
	private float halfWidth = 5;
	private float translateLimitX;

	private Vector3 position;
	private Vector3 direction = Vector3.right;

	private void Start()
	{
		translateLimitX = new CameraUtil().getCameraWidth() - halfWidth;
		enemies = new List<BaseEnemy>();
		GetComponentsInChildren<BaseEnemy>(enemies);
		position = transform.position;
		Invoke("EnemyAttack", Random.Range(minAttractRange, maxAttractRange));
	}

	private void Update()
	{
		MoveWave();
	}

	private void MoveWave()
	{
		position += GetForce(direction);
	}

	private Vector3 GetForce(Vector3 direction)
	{
		float force = Time.deltaTime * speed;
		float posX = position.x + (direction.x * force);
		if (posX < -translateLimitX || posX > translateLimitX)
		{
			this.direction *= -1;
			return Vector3.zero;
		}
		return direction * force;
	}

	private void EnemyAttack()
	{
		BaseEnemy enemy = enemies[Random.Range(0, enemies.Count)];
		if (enemy != null)
		{
			enemy.Attack();
		}
		if (stateModal.getState() == GameState.Playing)
		{
			Invoke("EnemyAttack", Random.Range(minAttractRange, maxAttractRange));
		}
	}

	public void DestoryEnemy(BaseEnemy enemy)
	{
		enemies.Remove(enemy);
		if (enemies.Count == 0)
		{
			if (gameController != null)
			{
				gameController.InstantiateWave();
			}
			Destroy(gameObject);
		}
	}

	public void InitWave(GameController gameController, float speed)
	{
		this.gameController = gameController;
		this.speed = speed;
	}

	public Vector3 GetPosition()
	{
		if (position == null)
		{
			position = transform.position;
		}
		return position;
	}

	public float GetSpeed()
	{
		return speed;
	}
}
