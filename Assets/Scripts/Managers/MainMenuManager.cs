using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("-- Pass Text References --")]
    [Space(10)]
    [SerializeField] private TMP_Text _totalMatches;
    [SerializeField] private TMP_Text _totalTurns;
    [SerializeField] private TMP_Text _totalCombo;
    [SerializeField] private TMP_Text _gameLevel;

    public void Start()
    {
        if(GameConstantsPlayerPref.GetFirstTimeGameOpenSet().Equals(0))
        {
            GameConstantsPlayerPref.SetDefaultGameValues();
        }
        SetMenuScoreBoard();
    }

    public void SetTotalComboText(string messageText)
    {
        _totalCombo.text = messageText;
    }

    public void SetTotalMatchesText(string messageText)
    {
        _totalMatches.text = messageText;
    }

    public void SetTotalTurnsText(string messageText)
    {
        _totalTurns.text = messageText;
    }

    public void SetGameLevelText(string messageText)
    {
        _gameLevel.text = messageText;
    }

    private void SetMenuScoreBoard()
    {
        SetTotalComboText("" + GameConstantsPlayerPref.GetTotalCombo());
        SetTotalMatchesText("" + GameConstantsPlayerPref.GetTotalMatches());
        SetTotalTurnsText("" + GameConstantsPlayerPref.GetTotalTurns());
        SetGameLevelText("LEVEL " + GameConstantsPlayerPref.GetGameLevel());
    }
}