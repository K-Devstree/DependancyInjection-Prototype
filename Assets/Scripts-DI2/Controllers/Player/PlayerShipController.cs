using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
	[SerializeField]
	private int speed = 5;
	[SerializeField]
	private GameObject bullet;

	private float translateLimitX;
	private float halfWidth = 0.5f;

	private bool isDmg = false;
	private bool isDestory = false;

	private GameController gameController;

	[SerializeField]
	private AudioSource audioSource;
	[SerializeField]
	private AudioClip explodeAudioClip;

	public string horizontalAxis = "Horizontal";
	private float inputHorizontal;
	public string shootButton = "Shoot";

    private void Awake()
    {
		audioSource = GetComponent<AudioSource>();
    }

	private void Start()
	{
		translateLimitX = new CameraUtil().getCameraWidth() - halfWidth;
		Invoke("ShipReady", 1f);
	}

	private void Update()
	{
		if (!isDestory)
		{
			HandleInput();
		}
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Enemy" && isDmg)
		{
			isDestory = true;
			Animator animator = GetComponent<Animator>();
			if (animator == null)
			{
				ShipDestory();
				return;
			}
			else
			{
				animator.SetTrigger("Destory");
				PlayAudio(explodeAudioClip, false);
				Invoke("ShipDestory", 0.29f);
			}
		}
	}

	private void ShipDestory()
	{
		if (gameController != null)
		{
			gameController.ShipDestory();
		}
		Destroy(gameObject);
	}

	private void ShipReady()
	{
		isDmg = true;
	}

	private void HandleInput()
	{
		inputHorizontal = SimpleInput.GetAxis(horizontalAxis);
		Move(new Vector3(inputHorizontal, 0f, 0f));
		if (SimpleInput.GetButtonDown(shootButton))
        {
			Shoot();
		}
	}

	private void Move(Vector3 direction)
	{
		transform.Translate(GetForce(direction), Space.World);
	}

	private void Shoot()
	{
		Instantiate(bullet, transform.position, bullet.transform.rotation);
	}

	private void PlayAudio(AudioClip audioClip, bool loop)
	{
		audioSource.clip = audioClip;
		audioSource.loop = loop;
		audioSource.Play();
	}

	private Vector3 GetForce(Vector3 direction)
	{
		float force = Time.deltaTime * speed;
		float posX = transform.position.x + (direction.x * force);
		if (posX < -translateLimitX || posX > translateLimitX)
		{
			return Vector3.zero;
		}
		return direction * force;
	}

	public void SetGamePlay(GameController gameController)
	{
		this.gameController = gameController;
	}
}
