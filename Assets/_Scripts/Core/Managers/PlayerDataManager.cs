using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    private const string PlayerScoreKey = "PlayerScore";
    private const string PlayerWinMedalsKey = "PlayerWinMedals";

    [SerializeField] private List<TMP_Text> playerScoreTexts;
    [SerializeField] private List<TMP_Text> winMedalsTexts;

    new void Awake()
    {
        // Retrieve the player's score and win medals when the game starts
        int playerScore = PlayerPrefs.GetInt(PlayerScoreKey, 0);
        int playerWinMedals = PlayerPrefs.GetInt(PlayerWinMedalsKey, 0);

        Debug.Log("Player's Score: " + playerScore);
        Debug.Log("Player's Win Medals: " + playerWinMedals);

        UpdatePlayerScoreUI(playerScore);
        UpdateWinMedalsUI(playerWinMedals);
    }

    private void OnEnable()
    {
        Container.OnTileMatching += UpdatePlayerScore;
        // Add more event listeners for actions that earn win medals
    }

    private void OnDisable()
    {
        Container.OnTileMatching -= UpdatePlayerScore;
        // Remove event listeners for actions that earn win medals
    }

    public void UpdatePlayerScore()
    {
        int scoreToAdd = 10;
        int playerScore = PlayerPrefs.GetInt(PlayerScoreKey, 0);
        playerScore += scoreToAdd;
        PlayerPrefs.SetInt(PlayerScoreKey, playerScore);
        PlayerPrefs.Save();
        Debug.Log("Player's Score Updated: " + playerScore);
        UpdatePlayerScoreUI(playerScore);
    }

    public void UpdateWinMedals()
    {
        int winMedalsToAdd = 1; // For example, adding 1 win medal
        int playerWinMedals = PlayerPrefs.GetInt(PlayerWinMedalsKey, 0);
        playerWinMedals += winMedalsToAdd;
        PlayerPrefs.SetInt(PlayerWinMedalsKey, playerWinMedals);
        PlayerPrefs.Save();
        Debug.Log("Player's Win Medals Updated: " + playerWinMedals);
        UpdateWinMedalsUI(playerWinMedals);
    }

    public void ResetPlayerScore()
    {
        PlayerPrefs.SetInt(PlayerScoreKey, 0);
        PlayerPrefs.Save();
        Debug.Log("Player's Score Reset to 0");
        UpdatePlayerScoreUI(0);
    }

    // Function to retrieve the player's score (e.g., for displaying it on the UI)
    public int GetPlayerScore()
    {
        return PlayerPrefs.GetInt(PlayerScoreKey, 0);
    }

    // Function to retrieve the player's win medals
    public int GetWinMedals()
    {
        return PlayerPrefs.GetInt(PlayerWinMedalsKey, 0);
    }

    private void UpdatePlayerScoreUI(int score)
    {
        foreach (var text in playerScoreTexts)
        {
            text.text = score.ToString();
        }
    }

    private void UpdateWinMedalsUI(int winMedals)
    {
        foreach (var text in winMedalsTexts)
        {
            text.text = winMedals.ToString();
        }
    }
}
