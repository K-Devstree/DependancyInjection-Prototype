using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
	[Inject]
	IStateModal stateModal;
	[Inject]
	IScoreModal scoreModal;
	[Inject]
	IPlayerModal playerModal;

	[Header("Controller")]
	[SerializeField]
	private UIController m_UIController;

	[Header("Audio")]
	[SerializeField]
	private AudioSource audioSource;
	[SerializeField]
	private AudioClip introAudioClip;
	[SerializeField]
	private AudioClip levelStartAudioClip;
	[SerializeField]
	private AudioClip gameOverAudioClip;

	[Header("Particle System")]
	[SerializeField]
	private ParticleSystem particleSystem;

	[Header("Player")]
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private int playerLife = 3;

	[Header("Enemy")]
	[SerializeField]
	private List<GameObject> enemyWave;
	[SerializeField]
	private float enemiesSpeed = 2f;
	[SerializeField]
	private float enemiesSpeedUp = 0.5f;

	private int waveIndex;
	private GameObject wave;

	private float startSpeed;

	private int score;
	private int highScore;
	private List<PlayerScore> playerScoreList;
	private List<GameObject> highScoreObjList = new List<GameObject>();

	private List<GameObject> lifeImages = new List<GameObject>();

	private void Awake()
    {
		audioSource = GetComponent<AudioSource>();
    }

	private void Start()
	{
		m_UIController.HandleGameUI();
		PlayAudio(introAudioClip, true);
		startSpeed = enemiesSpeed;
		m_UIController.playerNameInputField.text = playerModal.GetPlayerName();
		m_UIController.gameStartButton.onClick.AddListener(HandleGameStart);
		m_UIController.gameRestartButton.onClick.AddListener(HandleRestart);
		m_UIController.gameReplayButton.onClick.AddListener(HandleReplay);
	}

	private void Update()
	{
		if (stateModal.getState() == GameState.Playing)
		{
			HandleGameScore();
		}
	}

	private void HandleGameScore()
	{
		score = scoreModal.GetPoint();
		m_UIController.scoreText.text = "<color=\"red\">Score:</color>" + score.ToString();
		if (score < highScore)
		{
			m_UIController.highScoreText.text = "<color=\"red\">High Score:</color>" + highScore;
		}
		else
		{
			m_UIController.highScoreText.text = "<color=\"red\">High Score:</color>" + score;
		}
	}

	private void HandlePlayerLifeUI()
    {
		DestroyLifeImages();
		for (int i = 0; i < playerLife; i++)
		{
			var lifeImage = Instantiate(m_UIController.lifePrefab, m_UIController.lifePrefabParent.transform);
			lifeImage.SetActive(true);
			lifeImages.Add(lifeImage);
		}
	}

	private void HandleGameStart()
	{
		stateModal.setState(GameState.Ready);
		m_UIController.HandleGameUI();
		ResetData();
		StartCoroutine(StartGame());
		playerModal.SetPlayerName(m_UIController.playerNameInputField.text);
	}

	private void HandleReplay()
	{
		stateModal.setState(GameState.Ready);
		m_UIController.HandleGameUI();
		ResetData();
		StartCoroutine(StartGame());
	}

	private void HandleRestart()
	{
		stateModal.setState(GameState.Start);
		m_UIController.HandleGameUI();
		ResetData();
		PlayAudio(introAudioClip, true);
	}

	private void ResetData()
    {
		playerModal.SetLife(playerLife);
		HandlePlayerLifeUI();
		scoreModal.SetPoint(0);
		highScore = scoreModal.GetHighScore();
		enemiesSpeed = startSpeed;
		waveIndex = 0;
	}

	private IEnumerator StartGame()
	{
		yield return new WaitForSeconds(4f);
		stateModal.setState(GameState.Playing);
		m_UIController.HandleGameUI();
		yield return new WaitForSeconds(1f);
		InstantiateShip();
		InstantiateWave();
		PlayAudio(levelStartAudioClip, false);
		particleSystem.Play();
	}

	private void InstantiateShip()
	{
		PlayerShipController playerShipController = Instantiate(player).GetComponent<PlayerShipController>();
		if (playerShipController != null)
		{
			playerShipController.SetGamePlay(this);
		}
	}

	public void InstantiateWave()
	{
		if (waveIndex > enemyWave.Count - 1)
		{
			waveIndex = 0;
		}
		wave = Instantiate(enemyWave[waveIndex]);
		EnemyWave _enemyWave = wave.GetComponent<EnemyWave>();
		if (_enemyWave != null)
		{
			_enemyWave.InitWave(this, enemiesSpeed);
			enemiesSpeed += enemiesSpeedUp;
		}
		waveIndex++;
	}

	public void ShipDestory()
	{
		int life = playerModal.GetLife() - 1;
		playerModal.SetLife(life);
		Destroy(lifeImages[life]);
		if (life > 0)
		{
			InstantiateShip();
        }
		else
		{
			Destroy(wave);
			particleSystem.Stop();
			stateModal.setState(GameState.GameOver);
			m_UIController.HandleGameUI();
			StartCoroutine(ProcessScores());
			PlayAudio(gameOverAudioClip, false);
		}
	}

	private IEnumerator ProcessScores()
    {
		scoreModal.AddScore(playerModal.GetPlayerName(), scoreModal.GetPoint());
		scoreModal.SaveScores();
		yield return null;
		score = scoreModal.GetPoint();
		m_UIController.gameOverScoreText.text = score.ToString();
		DestroyHighScoreListObj();
		playerScoreList = scoreModal.GetHighScoreList();
		for (int i = 0; i < playerScoreList.Count; i++)
		{
			var obj = Instantiate(m_UIController.highScorePrefab, m_UIController.highScoreScrollView.content);
			var index = i + 1;
			obj.GetComponent<HighScoreDisplayController>().SetData(playerScoreList[i], index);
			obj.GetComponent<HighScoreDisplayController>().SetDataUI();
			obj.SetActive(true);
			highScoreObjList.Add(obj);
		}
	}

	private void DestroyHighScoreListObj()
    {
		foreach (var obj in highScoreObjList)
		{
			DestroyImmediate(obj);
		}
		highScoreObjList = new List<GameObject>();
	}

	private void DestroyLifeImages()
    {
		foreach (var img in lifeImages)
		{
			DestroyImmediate(img);
		}
		lifeImages = new List<GameObject>();
	}

	private void PlayAudio(AudioClip audioClip, bool loop)
    {
		audioSource.Stop();
		audioSource.clip = audioClip;
		audioSource.loop = loop;
		audioSource.Play();
	}
}
