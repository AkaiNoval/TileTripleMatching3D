using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    #region Fields
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject lostUI; 
    [SerializeField] GameObject preventClickPanel; 
    #endregion

    #region EventListener
    private void Start()
    {
        /* Subscribe to the GameManager's OnGameStateChanged event */
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }
    private void OnDestroy() => GameManager.OnGameStateChanged -= HandleGameStateChange;
    #endregion

    #region HandleUI
    public void HandleGameStateChange(GameState newState)
    {
        /* Deactivate all UI elements initially */
        preventClickPanel.SetActive(false);
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
                preventClickPanel.SetActive(true);
                gameplayUI.SetActive(true);
                lostUI.SetActive(true);
                break;
        }
    } 
    #endregion

    #region UI
    public void GoBackToMainMenuButton()
    {
        GameManager.Instance.UpdateGameState(GameState.Menu);
    }
    public void GoToGameplayButton()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
    #endregion
}
