using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <Comment-Start>  Class Purpose
/// Class for managing state of cards on board as well as checking cards interation
/// </Class Purpose>

public class GameBoard : MonoBehaviour
{
    [Header("-- Time User Is Showed Cards In Start --")]
    [Space(10)]
    [SerializeField] private float StartGameAfterTimer = 1f;

    [Header("-- Card Settings --")]
    [Space(10)]
    [SerializeField] private Transform _cardPrefab = default;
    [SerializeField] private Transform _boardWidgetHolder = default;
    [SerializeField] private float _cellSpacing = 5f;

    [Header("-- Level Object Timer Settings --")]
    [Space(10)]
    [SerializeField] private TimeSystem _timerSystem;

    private List<Transform> _spawnCards = new List<Transform>();
    private Transform _tempCard = default;
    private Card _previousCard;
    private int state = 0;
    private int _currentLevel = default;
    private ScoreSystem _scoreSystem;
    [SerializeField] private LevelSaveSystem _levelSaveSystem;
    private void Start()
    {
        _scoreSystem = new ScoreSystem();
        _levelSaveSystem = new LevelSaveSystem();
        _currentLevel = GameConstantsPlayerPref.GetGameLevel();
        SetCurrentLevelText(_currentLevel);
    }

    public void LoadLastSaveLevel(List<Sprite> faceCardSprites)
    {
        _levelSaveSystem.LoadPreviousSavedBoardData();

        ScaleCardToFitContainor(faceCardSprites[0], (float)_levelSaveSystem.TotalCardsOnBoard / 4);

        for (int i = 0; i < _levelSaveSystem.TotalCardsOnBoard; i++)
        {
            CreateCard(
                    _levelSaveSystem.CardsOnBoard[i].CardID, 
                    faceCardSprites.Single(s => s.name == _levelSaveSystem.CardsOnBoard[i].CardFaceName), 
                    _levelSaveSystem.CardsOnBoard[i].CardActive
                );
        }

        foreach (Transform card in _spawnCards)
        {
            card.SetParent(_boardWidgetHolder);
            card.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        StartCoroutine(StartGame());

    }
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
                        _scoreSystem.CardsMatched_Score();
                        GameSoundManager.Instance.PlaySoundOneShot(GameSoundManager.SoundType.Match);
                    }
                    else
                    {
                        StartCoroutine(ResetCardsSelected(_previousCard, currentCard));
                        _scoreSystem.CardsMisMatchedScore();
                        GameSoundManager.Instance.PlaySoundOneShot(GameSoundManager.SoundType.MisMatch);
                    }

                    state = 0;
                    break;
                }
        }
    }

    public void ResetBoardElements()
    {
        foreach (Transform card in _spawnCards)
        {
            card.GetComponent<Card>().DeactivateCard();
        }

        _spawnCards.Clear();
        _timerSystem.ResetTimer();
    }

    public void ResetBoard(List<Sprite> selectedCardFace)
    {
        _levelSaveSystem.ClearSaveData();
        ResetBoardElements();
        _scoreSystem.ResetScoreForNewLevel();
        Transform[] exixtingCards = _boardWidgetHolder.GetComponentsInChildren<Transform>();

        for (int i = 1, j = 0; i < exixtingCards.Length; i += 2, j++)
        {
            exixtingCards[i].GetComponent<Card>().ResetCard(j, selectedCardFace[j]);
            exixtingCards[i].SetParent(null);
            _spawnCards.Add(exixtingCards[i]);
            exixtingCards[i + 1].GetComponent<Card>().ResetCard(j, selectedCardFace[j]);
            exixtingCards[i + 1].SetParent(null);
            _spawnCards.Add(exixtingCards[i + 1]);
        }

        ShuffleAndSetToBoard();
        StartCoroutine(StartGame());
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
        _levelSaveSystem.SaveCardToPlayerPrefAsJason(_spawnCards);
    }

    private void CreateCard(int id, Sprite cardFront, bool activeCard = true)
    {
        _tempCard = Instantiate(_cardPrefab.gameObject).transform;
        _tempCard.gameObject.SetActive(true);
        _tempCard.GetComponent<Card>().Init(id, cardFront, ValidateCardCombination, activeCard);
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

        _levelSaveSystem.UpdateCard
            (
                card1.GetFaceCardName(),
                _scoreSystem.CardsMatchScore,
                _scoreSystem.TurnsScore,
                _scoreSystem.CardComboScore
            );

        ValidateGameEnd();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(StartGameAfterTimer);

        List< Transform> RemovePlayedCards = new List< Transform >();   
        foreach (Transform card in _spawnCards)
        {
            if(card.GetComponent<Button>().interactable)
            {
                card.GetComponent<Card>().ShowCardSide(Card.CardSides.Back);
            }
            else
            {
                RemovePlayedCards.Add(card);
            }
        }

        RemoveMatchingCardsFromPreviousSavedGame(RemovePlayedCards);

        _timerSystem.StartGameTimer();
    }

    private void RemoveMatchingCardsFromPreviousSavedGame(List<Transform> RemovePlayedCards)
    {
        foreach (Transform card in RemovePlayedCards)
        {
            _spawnCards.Remove(card);
        }
    }

    private void ValidateGameEnd()
    {
        if (_spawnCards.Count <= 0)
        {
            GameController.LevelCompleteEventListner?.Invoke();
            _timerSystem.StopLevelTimer();
            SetLevelLabel();
            _levelSaveSystem.ClearSaveData();
        }
    }

    private void SetLevelLabel()
    {
        _currentLevel++;
        GameConstantsPlayerPref.SetGameLevel(_currentLevel);
        SetCurrentLevelText(_currentLevel);
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

    private void SetCurrentLevelText(int value)
    {
        GameUIMnager.Instance.SetGameLevelText("" + value);
    }
}