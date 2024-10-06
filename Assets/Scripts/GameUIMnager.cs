using TMPro;
using UnityEngine;

public class GameUIMnager : Singleton<GameUIMnager>
{
    [Header("-- UI Text Scene References --")]
    [Space(10)]
    [SerializeField] private TMP_Text _matchesTxt; 
    [SerializeField] private TMP_Text _turnsTxt;
    [SerializeField] private TMP_Text _comboTxt;
    [SerializeField] private TMP_Text _totalMatchesTxt;
    [SerializeField] private TMP_Text _totalTurnsTxt;
    [SerializeField] private TMP_Text _totalComboTxt;
    [SerializeField] private TMP_Text _gameLevelTxt;
    [SerializeField] private TMP_Text _gameTimerTxt;
    [SerializeField] private Transform _LevelCompleteScreen;
    [SerializeField] private Transform _LevelFailScreen;


    public void SetMatchesText(string messageText)
    {
        _matchesTxt.text = messageText;
    }

    public void SetTurnsText(string messageText)
    {
        _turnsTxt.text = messageText;
    }

    public void SetComboText(string messageText)
    {
        _comboTxt.text = messageText;
    }

    public void SetTotalComboText(string messageText)
    {
        _totalComboTxt.text = messageText;
    }

    public void SetTotalMatchesText(string messageText)
    {
        _totalMatchesTxt.text = messageText;
    }

    public void SetTotalTurnsText(string messageText)
    {
        _totalTurnsTxt.text = messageText;
    }

    public void SetGameLevelText(string messageText)
    {
        _gameLevelTxt.text = "LEVEL " + messageText;
    }

    public void SetGameTimerText(string messageText)
    {
        _gameTimerTxt.text = "TIME " + messageText;
    }

    public void ToggleActivateLevelCompleteScreen(bool flag)
    {
        _LevelCompleteScreen.gameObject.SetActive(flag);
    }

    public void ToggleActivateLevelFailScreen(bool flag)
    {
        _LevelFailScreen.gameObject.SetActive(flag);
    }

}
