using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [Header("-- Level Time Settings --")]
    [Space(10)]
    [SerializeField] private float TimeRemaining = 300;

    private float _defaultTime = default;
    private bool _timerIsRunning = false;


    public void Start()
    {
        _defaultTime = TimeRemaining;
    }

    private void Update()
    {
        if (_timerIsRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
                DisplayTime(TimeRemaining);
            }
            else
            {
                GameUIMnager.Instance.ToggleActivateLevelFailScreen(true);
                GameSoundManager.Instance.PlaySoundOneShot(GameSoundManager.SoundType.GameFail);
                TimeRemaining = 0;
                _timerIsRunning = false;
            }
        }
    }

    public void StartGameTimer()
    {
        _timerIsRunning = true;
    }

    public void ResetTimer()
    {
        TimeRemaining = _defaultTime;
        _timerIsRunning = true;
    }

    public void StopLevelTimer()
    {
        _timerIsRunning = false;
    }
 
    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        GameUIMnager.Instance.SetGameTimerText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }
}
