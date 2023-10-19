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
        Application.targetFrameRate = 60;
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
                HandleVictoryState();
                break;
            case GameState.Lose:
                HandleLosingState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }
    private void HandleVictoryState()
    {
        AudioSFXManager.PlaySFX(AudioKey.WinSFX);
        PlayerDataManager.Instance.UpdateWinMedals();
    }
    private void HandleLosingState()
    {
        AudioSFXManager.PlaySFX(AudioKey.LostSFX);
    }

}
