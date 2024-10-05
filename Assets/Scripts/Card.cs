using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardID { private set; get; } 
    [Header("Front and Back Side Card Images")]
    [SerializeField] private Transform _cardFront, _cardBack;
    private Button _cardBtn;
    private Action<Card> _callbackSelectedCardToGameBoard;
    public enum CardSides { Front, Back }

    //Initialize the card front with sprite, adds button and binds interation listener to button
    public void Init(int cardId, Sprite cardFace, Action<Card> callbackSelectedCard)
    {
        _callbackSelectedCardToGameBoard = callbackSelectedCard;
        CardID = cardId;
        _cardFront.GetComponent<Image>().sprite = cardFace;
        _cardBtn = transform.gameObject.AddComponent<Button>();
        _cardBtn.onClick.AddListener(CardInteraction);
        ShowCardSide(CardSides.Front);
    }

    // for calling card side functionality
    public void ShowCardSide(CardSides cardSide)
    {
        switch (cardSide)
        {
            case CardSides.Front:
                {
                    _cardFront.gameObject.SetActive(true);
                    _cardBack.gameObject.SetActive(false);
                    break;
                }
            case CardSides.Back:
                {
                    _cardFront.gameObject.SetActive(false);
                    _cardBack.gameObject.SetActive(true);
                    break;
                }
        }
    }

    // Capturing user input through button
    public void CardInteraction()
    {
        _cardBtn.interactable = false;
        _callbackSelectedCardToGameBoard.Invoke(this);
        ShowCardSide(CardSides.Front);
    }

    public void ResetCard()
    {
        ShowCardSide(CardSides.Back);
        _cardBtn.interactable = true;
    }

    public void DeactivateCardAnimated()
    {
        _cardFront.gameObject.SetActive(false);
        _cardBack.gameObject.SetActive(false);
        _cardBtn.interactable = false;
    }
}
