using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public interface IScoreModal
{
	int GetPoint();
	void SetPoint(int point);

	List<PlayerScore> GetHighScoreList();
	int GetHighScore();
	void AddScore(string playerName, int point);
	void SaveScores();
}

public class ScoreModal : IScoreModal
{
	private int point = 0;
	private List<PlayerScore> player_score_list;


	private ScoreModal()
	{
		player_score_list = new List<PlayerScore>();
		if (PlayerPrefs.HasKey("PlayerScoreData"))
		{
			player_score_list = JsonConvert.DeserializeObject<List<PlayerScore>>(PlayerPrefs.GetString("PlayerScoreData"));
		}
	}

	public int GetPoint()
	{
		return point;
	}

	public void SetPoint(int point)
	{
		this.point = point;
	}

	public List<PlayerScore> GetHighScoreList()
	{
			return player_score_list.OrderByDescending(o => o.score).ToList();
	}

	public int GetHighScore()
	{
		if (player_score_list.Count > 0)
		{
			return player_score_list.OrderByDescending(o => o.score).First().score;
		}
		else
		{
			return 0;
		}
	}

	public void AddScore(string playerName, int point)
	{
		PlayerScore playerScore = new PlayerScore(playerName, point)
		{
			name = playerName,
			score = point
		};
		player_score_list.Add(playerScore);
		List<PlayerScore> sortedList = player_score_list.OrderByDescending(o => o.score).ToList();
		player_score_list = sortedList;
	}

	public void SaveScores()
	{
		PlayerPrefs.SetString("PlayerScoreData", JsonConvert.SerializeObject(player_score_list));
		PlayerPrefs.Save();
	}
}

public class PlayerScore
{
	public PlayerScore(string name, int score)
	{
		this.name = name;
		this.score = score;
	}

	public string name { get; set; }
	public int score { get; set; }
}
