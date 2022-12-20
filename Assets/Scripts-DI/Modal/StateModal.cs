using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateModal
{
	GameState getState();
	void setState(GameState gameState);
}

public class StateModal : IStateModal
{
	private GameState gameState;

	private StateModal()
	{
		gameState = GameState.Start;
	}

	public GameState getState()
	{
		return gameState;
	}

	public void setState(GameState gameState)
	{
		this.gameState = gameState;
	}
}
