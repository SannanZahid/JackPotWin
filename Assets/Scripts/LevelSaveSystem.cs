using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


/// <Comment-Start>  Class Purpose
//   To Save Board data of cards to playerpref as json
//   To Load Previously Saved Board Game Data
/// </Comment End>

[System.Serializable]
public class LevelSaveSystem
{
    public int TotalCardsOnBoard = default;
    public List<CardDetails> CardsOnBoard = new List<CardDetails>();
    public int MatchedCount;
    public int TurnsCount;
    public int CardsComboCount;

    public void SaveCardToPlayerPrefAsJason(List<Transform> cards)
    {
        TotalCardsOnBoard = cards.Count;
        for (int i = 0; i < TotalCardsOnBoard; i++) 
        {
            CardDetails cardDetail = new CardDetails();
            cardDetail.CardID = cards[i].GetComponent<Card>().CardID;
            cardDetail.CardFaceName = cards[i].GetComponent<Card>().GetFaceCardName();
            cardDetail.CardActive = cards[i].GetComponent<Card>().IsCardActive();
            CardsOnBoard.Add(cardDetail);
        }

        SaveBoardData();
    }
    public void UpdateCard(string FaceCardName, int matchedCount, int turnsCount, int cardsComboCount)
    {
        for(int i = 0; i < CardsOnBoard.Count; i++)
        {
            if (FaceCardName == CardsOnBoard[i].CardFaceName)
            {
                CardsOnBoard[i].SetCardActive(false);
            }
        }

        MatchedCount = matchedCount;
        TurnsCount = turnsCount;
        CardsComboCount = cardsComboCount;

        SaveBoardData();
    }

    public void ClearSaveData()
    {
        TotalCardsOnBoard = 0;
        MatchedCount = 0;
        TurnsCount = 0;
        CardsComboCount = 0;

        CardsOnBoard.Clear();
        GameConstantsPlayerPref.SetBoardData(string.Empty);
    }

    public void LoadPreviousSavedBoardData()
    {
        LevelSaveSystem SaveData = JsonUtility.FromJson<LevelSaveSystem>(GameConstantsPlayerPref.GetBoardData());

        this.TotalCardsOnBoard = SaveData.TotalCardsOnBoard;
        this.CardsOnBoard = SaveData.CardsOnBoard;
        this.MatchedCount = SaveData.MatchedCount;
        this.TurnsCount = SaveData.TurnsCount;
        this.CardsComboCount = SaveData.CardsComboCount;

        GameUIMnager.Instance.SetMatchesText(MatchedCount.ToString());
        GameUIMnager.Instance.SetTurnsText(TurnsCount.ToString());
        GameUIMnager.Instance.SetComboText(CardsComboCount.ToString());

    }

    private void SaveBoardData()
    {
        var boardJsonDataString = JsonUtility.ToJson(this);
        GameConstantsPlayerPref.SetBoardData(boardJsonDataString);
    }
    
}
[System.Serializable]
public class CardDetails
{
    public int CardID;
    public string CardFaceName;
    public bool CardActive;
    public void SetCardActive(bool cardActive)
    {
        CardActive = cardActive;
    }
}
