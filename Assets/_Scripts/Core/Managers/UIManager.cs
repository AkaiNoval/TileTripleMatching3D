using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject lostUI;
    private void Start()
    {
        /* Subscribe to the GameManager's OnGameStateChanged event */
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }
    private void HandleGameStateChange(GameState newState)
    {
        /* Deactivate all UI elements initially */
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        winUI.SetActive(false);
        lostUI.SetActive(false);

        switch (newState)
        {
            case GameState.Menu:
                mainMenuUI.SetActive(true);
                break;
            case GameState.Playing:
                gameplayUI.SetActive(true);
                break;
            case GameState.Victory:
                gameplayUI.SetActive(true);
                winUI.SetActive(true);
                break;
            case GameState.Lose:
                gameplayUI.SetActive(true);
                lostUI.SetActive(true);
                break;
        }
    }

    private void OnDestroy() => GameManager.OnGameStateChanged -= HandleGameStateChange;
}
