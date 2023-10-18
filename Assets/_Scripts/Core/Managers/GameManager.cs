using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Victory,
    Lose
}
public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;
    [field: SerializeField] public GameState gameState { get; private set; }

    private void Start()
    {
        UpdateGameState(GameState.Menu);
    }

    public void UpdateGameState(GameState newState)
    {
        gameState = newState;
        switch (gameState)
        {
            case GameState.Menu:
                break;
            case GameState.Playing:
                break;
            case GameState.Victory:
                VictoryStateHandle();
                break;
            case GameState.Lose:
                LoseStateHandle();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }
    private void MenuStateHandle()
    {

    }
    private void PlayingStateHandle()
    {

    }
    private void VictoryStateHandle()
    {
        Debug.Log("You won the game");
    }
    private void LoseStateHandle()
    {
        Debug.Log("You lost the game");
    }

}
