using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
	private void Start()
	{
		Invoke("DestoryBeam", 2.9f);
	}

	private void Update()
	{

	}

	private void DestoryBeam()
	{
		Destroy(gameObject);
	}
}
