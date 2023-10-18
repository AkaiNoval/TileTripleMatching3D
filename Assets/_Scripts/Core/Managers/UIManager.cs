using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Fields
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject lostUI; 
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
    #endregion

    #region UI
    public void GoBackToMainMenuButton()
    {
        GameManager.Instance.UpdateGameState(GameState.Menu);
    } 
    #endregion 
}
