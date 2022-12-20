using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtil
{
	public float getCameraHeight()
	{
		return Camera.main.orthographicSize;
	}

	public float getCameraWidth()
	{
		Camera mainCamera = Camera.main;
		return mainCamera.orthographicSize * mainCamera.aspect;
	}
}
