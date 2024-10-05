using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int CardID { private set; get; } 
    [Header("Front and Back Side Card Images")]
    [SerializeField] private Transform _cardFront, _cardBack;
    private Button _cardBtn;
    public enum CardSides { Front, Back }

    //Initialize the card front with sprite, adds button and binds interation listener to button
    public void Init(int cardId, Sprite cardFace)
    {
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
                    break;
                }
            case CardSides.Back:
                {
                    break;
                }
        }
    }

    // Capturing user input through button
    public void CardInteraction()
    {
        _cardBtn.interactable = false;
        ShowCardSide(CardSides.Front);
    }
}
