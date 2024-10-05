using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Time User Is Showed Cards In Start")]
    public float StartGameAfter = 1f;

    [SerializeField] private Transform _cardPrefab = default, _boardWidgetHolder = default;
    private List<Transform> _spawnCards = new List<Transform>();
    private Transform _tempCard = default;

    /// Takes face card sprites and pass it to card creation  
    public void SetBoard(List<Sprite> selectedCardFace)
    {
        for (int i = 0; i < selectedCardFace.Count; i++)
        {
            CreateCard(i, selectedCardFace[i]);
            CreateCard(i, selectedCardFace[i]);
        }
        ShuffleAndSetToBoard();
    }

    // Shuffles the created card and sets the Cards in to the UI Canvas containor
    private void ShuffleAndSetToBoard()
    {
        GameController.ShuffleCards(ref _spawnCards);
        foreach (Transform card in _spawnCards)
        {
            card.SetParent(_boardWidgetHolder);
        }
    }

    private void CreateCard(int id, Sprite cardFront)
    {
        _tempCard = Instantiate(_cardPrefab.gameObject).transform;
        _tempCard.gameObject.SetActive(true);
        _tempCard.GetComponent<Card>().Init(id, cardFront);
        _spawnCards.Add(_tempCard);
    }
}
