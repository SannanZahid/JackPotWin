using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <Comment-Start>  Class Purpose
/// - Class for managing score during game play 
/// - Score calculated is populated to scored board in game screen UI
/// </Class Purpose>

public class ScoreSystem
{
    private int _cardsMatchScore = 0;
    private int _turnsScore = 0;
    private int _cardComboScore = 0;
    private int _totalMatchScore = 0;
    private int _totalTurnsScore = 0;
    private int _totalCombo = 0;
    private bool _match = false;

    public ScoreSystem()
    {
         _totalMatchScore = GameConstantsPlayerPref.GetTotalMatches();
        SetTotalMatchScoreDashboard(_totalMatchScore);
        _totalTurnsScore = GameConstantsPlayerPref.GetTotalTurns();
        SetTotalTurnScoreDashboard(_totalTurnsScore);
        _totalCombo = GameConstantsPlayerPref.GetTotalCombo();
        SetTotalComboScoreDashboard(_totalCombo);
    }

    public void ResetScoreForNewLevel()
    {
        _cardsMatchScore = 0;
        SetMatchScoreDashboard(0);
        _turnsScore = 0;
        SetTurnScoreDashboard(0);
        _cardComboScore = 0;
        SetComboScoreDashboard(0);
    }

    public void CardsMatched_Score()
    {
        if (_match)
        {
            CardsComboScore();
        }
        else
        {
            _match = true;
        }

        IncrementCardsMatch();
        IncrementTurn();
    }

    public void CardsMisMatchedScore()
    {
        _match = false;
        IncrementTurn();
        _cardComboScore = 0;
        SetComboScoreDashboard(_cardComboScore);
    }

    private void IncrementCardsMatch()
    {
        _cardsMatchScore++;
        SetMatchScoreDashboard(_cardsMatchScore);
        SetCardsTotalMatchScore();
    }

    private void IncrementTurn()
    {
        _turnsScore++;
        SetTurnScoreDashboard(_turnsScore);
        SetCardsTotalTurnsScore();
    }

    private void CardsComboScore()
    {
        _cardComboScore++;
        SetComboScoreDashboard(_cardComboScore);
        SetCardsTotalComboScore();
    }

    private void SetCardsTotalMatchScore()
    {
        _totalMatchScore++;
        SetTotalMatchScoreDashboard(_totalMatchScore);
        GameConstantsPlayerPref.SetTotalMatches(_totalMatchScore);
    }

    private void SetCardsTotalTurnsScore()
    {
        _totalTurnsScore++;
        SetTotalTurnScoreDashboard(_totalTurnsScore);
        GameConstantsPlayerPref.SetTotalTurns(_totalTurnsScore);
    }

    private void SetCardsTotalComboScore()
    {
        _totalCombo++;
        SetTotalComboScoreDashboard(_totalCombo);
        GameConstantsPlayerPref.SetTotalCombo(_totalCombo);
    }

    private void SetComboScoreDashboard(int value)
    {
        GameUIMnager.Instance.SetComboText("" + value);
    }

    private void SetMatchScoreDashboard(int value)
    {
        GameUIMnager.Instance.SetMatchesText("" + value);
    }

    private void SetTurnScoreDashboard(int value)
    {
        GameUIMnager.Instance.SetTurnsText("" + value);
    }

    private void SetTotalComboScoreDashboard(int value)
    {
        GameUIMnager.Instance.SetTotalComboText("" + value);
    }

    private void SetTotalMatchScoreDashboard(int value)
    {
        GameUIMnager.Instance.SetTotalMatchesText("" + value);
    }

    private void SetTotalTurnScoreDashboard(int value)
    {
        GameUIMnager.Instance.SetTotalTurnsText("" + value);
    }
}