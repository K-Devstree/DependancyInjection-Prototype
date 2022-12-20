using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreDisplayController : MonoBehaviour
{
    public PlayerScore playerScoreData;
    public int rank;
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    public void SetData(PlayerScore playerScoreData, int rank)
    {
        this.playerScoreData = playerScoreData;
        this.rank = rank;
    }

    public void SetDataUI()
    {
        rankText.text = "#" + rank;
        nameText.text = playerScoreData.name;
        scoreText.text = playerScoreData.score.ToString();
    }
}
