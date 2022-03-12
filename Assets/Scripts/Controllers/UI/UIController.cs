using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    [Inject]
    IStateModal stateModal;

    [SerializeField]
    private UITransitionEffect m_UITransitionEffect;

    [Header("UI Panels")]
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject gamePlayPanel;
    [SerializeField]
    private GameObject gameOverPanel;

    [Header("Start Panel")]
    [SerializeField]
    private GameObject gameLogo;
    public Button gameStartButton;
    public TMP_InputField playerNameInputField;

    [Header("Game Play Panel")]
    public GameObject lifePrefab;
    public GameObject lifePrefabParent;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    [Header("Game Over Panel")]
    public Button gameRestartButton;
    public Button gameReplayButton;
    public TMP_Text gameOverScoreText;
    public GameObject highScorePrefab;
    public ScrollRect highScoreScrollView;

    public void HandleGameUI()
    {
        if (stateModal.getState() == GameState.Start)
        {
            StartCoroutine(PlayScreenTransition(startPanel));
        }
        else if (stateModal.getState() == GameState.Ready)
        {
            StartCoroutine(PlayScreenTransition(readyPanel));
        }
        else if (stateModal.getState() == GameState.Playing)
        {
            StartCoroutine(PlayScreenTransition(gamePlayPanel));
        }
        else if (stateModal.getState() == GameState.GameOver)
        {
            StartCoroutine(PlayScreenTransition(gameOverPanel));
        }
    }

    private IEnumerator PlayScreenTransition(GameObject screenToEnable)
    {
        m_UITransitionEffect.gameObject.SetActive(true);
        m_UITransitionEffect.Show();
        yield return new WaitForSeconds(m_UITransitionEffect.effectPlayer.duration);
        startPanel.SetActive(false);
        readyPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        screenToEnable.SetActive(true);
        m_UITransitionEffect.Hide();
        m_UITransitionEffect.gameObject.SetActive(false);
    }
}
