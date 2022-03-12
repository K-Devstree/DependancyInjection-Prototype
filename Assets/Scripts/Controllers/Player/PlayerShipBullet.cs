using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipBullet : MonoBehaviour
{
	[SerializeField]
	private int speed = 20;

	private void Start()
	{

	}

	private void Update()
	{
		transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
