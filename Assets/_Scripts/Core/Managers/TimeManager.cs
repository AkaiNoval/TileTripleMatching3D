using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public TMP_Text timerText;
    public TMP_Text plusTimerText;
    private float countdownTime;
    private float currentTime;
    private LevelDataSO levelDataSO;
    Vector3 originalPosition;
    public LevelDataSO LevelDataSO
    {
        get => levelDataSO;
        set
        {
            levelDataSO = value;
            CurrentTime = levelDataSO.stateMaxTime; // Set the timer based on the selected LevelDataSO
            UpdateTimerText();
        }
    }

    public float CurrentTime 
    { 
        get => currentTime; 
        set => currentTime = Mathf.Clamp(value, 0, levelDataSO.stateMaxTime ); 
    }
    private new void Awake()
    {
        originalPosition = plusTimerText.transform.localPosition;
    }
    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;
        if (CurrentTime > 0)
        {
            if(currentTime < 5)
            {
                AudioSFXManager.PlaySFX(AudioKey.SlowOnTime);
            }
            CurrentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            UpdateTimerText();
            UIManager.Instance.HandleGameStateChange(GameState.Lose);
            // Handle time's up logic here
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(CurrentTime / 60);
        int seconds = Mathf.FloorToInt(CurrentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void PlusBonusTime()
    {
        CurrentTime += levelDataSO.plusTime;

        // Set the text of the plusTimerText
        plusTimerText.text = $"+{levelDataSO.plusTime} sec";

        // Enable the plusTimerText
        plusTimerText.gameObject.SetActive(true);

        // Use DoTween to animate the text (move up)
        plusTimerText.transform.DOLocalMoveY(originalPosition.y + 10f, 0.5f).OnComplete(() =>
        {
            // Disable the text when the animation is complete
            plusTimerText.gameObject.SetActive(false);
        });
    }

    public void RestartLevel()
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
        CurrentTime = levelDataSO.stateMaxTime; // Reset the timer to its initial value
        UpdateTimerText();
        Debug.Log("Restart Level");
    }
    private void OnEnable()
    {
        Container.OnTileMatching += PlusBonusTime;
    }
    private void OnDisable()
    {
        Container.OnTileMatching -= PlusBonusTime;
        // Reset the position to the original
        plusTimerText.transform.localPosition = originalPosition;
    }
}
