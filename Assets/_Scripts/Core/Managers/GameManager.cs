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
                PlayerDataManager.Instance.ResetPlayerScore();
                break;
            case GameState.Playing:
                PlayerDataManager.Instance.ResetPlayerScore();
                break;
            case GameState.Victory:
                PlayerDataManager.Instance.UpdateWinMedals();
                break;
            case GameState.Lose:              
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

}
