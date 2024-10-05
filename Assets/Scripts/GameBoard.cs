using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <Comment-Start>  Class Purpose
/// Class for managing state of cards on board as well as checking cards interation
/// </Class Purpose>

public class GameBoard : MonoBehaviour
{
    [Header("Time User Is Showed Cards In Start")]
    [SerializeField] private float StartGameAfterTimer = 1f;
    [SerializeField] private Transform _cardPrefab = default, _boardWidgetHolder = default;
    [SerializeField] private float _cellSpacing = 5f;
    private List<Transform> _spawnCards = new List<Transform>();
    private Transform _tempCard = default;
    private Card _previousCard;
    private int state = 0;

    /// Takes face card sprites and pass it to card creation  
    public void SetBoard(List<Sprite> selectedCardFace)
    {
        ScaleCardToFitContainor(selectedCardFace[0], (float)selectedCardFace.Count / 2);
        for (int i = 0; i < selectedCardFace.Count; i++)
        {
            CreateCard(i, selectedCardFace[i]);
            CreateCard(i, selectedCardFace[i]);
        }
        ShuffleAndSetToBoard();
        StartCoroutine(StartGame());
    }

    // validates cards matching and mismatching condition
    public void ValidateCardCombination(Card currentCard)
    {
        switch (state)
        {
            case 0:
                {
                    _previousCard = currentCard;
                    state = 1;
                    break;
                }
            case 1:
                {
                    if (_previousCard.CardID.Equals(currentCard.CardID))
                    {
                        StartCoroutine(DeactivateMatchingCards(_previousCard, currentCard));
                    }
                    else
                    {
                        StartCoroutine(ResetCardsSelected(_previousCard, currentCard));
                    }
                    state = 0;
                    break;
                }
        }
    }

    // Shuffles the created card and sets the Cards in to the UI Canvas containor
    private void ShuffleAndSetToBoard()
    {
        GameController.ShuffleCards(ref _spawnCards);
        foreach (Transform card in _spawnCards)
        {
            card.SetParent(_boardWidgetHolder);
            card.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

    private void CreateCard(int id, Sprite cardFront)
    {
        _tempCard = Instantiate(_cardPrefab.gameObject).transform;
        _tempCard.gameObject.SetActive(true);
        _tempCard.GetComponent<Card>().Init(id, cardFront, ValidateCardCombination);
        _spawnCards.Add(_tempCard);
    }

    private IEnumerator ResetCardsSelected(Card card1, Card card2)
    {
        yield return new WaitForSeconds(1f);
        card1.ResetCard();
        card2.ResetCard();
    }


    private IEnumerator DeactivateMatchingCards(Card card1, Card card2)
    {
        yield return new WaitForSeconds(1f);
        card1.DeactivateCardAnimated();
        card2.DeactivateCardAnimated();
        _spawnCards.Remove(card1.transform);
        _spawnCards.Remove(card2.transform);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(StartGameAfterTimer);
        foreach (Transform card in _spawnCards)
        {
            card.GetComponent<Card>().ShowCardSide(Card.CardSides.Back);
        }
    }

    //scalling cards according to containor widget
    private void ScaleCardToFitContainor(Sprite referenceCardSize, float gridSize)
    {
        float containorWidth;
        float containorHeight;
        GridLayoutGroup boardContainor = _boardWidgetHolder.GetComponent<GridLayoutGroup>();
        containorWidth = _boardWidgetHolder.GetComponent<RectTransform>().rect.width;
        containorHeight = _boardWidgetHolder.GetComponent<RectTransform>().rect.height;
        float spriteWidth = referenceCardSize.rect.width;
        float spriteHeight = referenceCardSize.rect.height;
        float forcastCellWidth = (containorWidth - (_cellSpacing * gridSize)) / gridSize;
        float forcastCellHeight = (containorHeight - (_cellSpacing * gridSize)) / gridSize;
        float ratioWidth = forcastCellWidth / spriteWidth;
        float ratioHeight = forcastCellHeight / spriteHeight;

        if (ratioHeight > 1 && ratioWidth > 1)
        {
            boardContainor.cellSize = new Vector2(spriteWidth, spriteHeight);
        }
        else if (ratioWidth < ratioHeight)
        {
            boardContainor.cellSize = new Vector2(spriteWidth * ratioWidth, spriteHeight * ratioWidth);
        }
        else
        {
            boardContainor.cellSize = new Vector2(spriteWidth * ratioHeight, spriteHeight * ratioHeight);
        }
    }
}
