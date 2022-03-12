using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerModal
{
	int GetLife();
	void SetLife(int life);
	string GetPlayerName();
	void SetPlayerName(string playerName);
}

public class PlayerModal : IPlayerModal
{
	private int life;
	private string playerName = string.Empty;

	private PlayerModal()
	{
		life = 1;
		playerName = PlayerPrefs.GetString("PlayerName", string.Empty);
	}

	public int GetLife()
	{
		return life;
	}

	public void SetLife(int life)
	{
		this.life = life;
	}

	public string GetPlayerName()
	{
		return playerName;
	}

	public void SetPlayerName(string playerName)
	{
		PlayerPrefs.SetString("PlayerName", playerName);
		PlayerPrefs.Save();
		this.playerName = playerName;
	}
}
