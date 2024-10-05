using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <Comment-Start>  Class Purpose
/// 
//   Class for creating game board with required card sprites and providing basic shuffling functionality,
//    class also keep tack of the game state.
///
/// </Comment End>

public class GameController : MonoBehaviour
{
    public int CardToPlaceOnBoard = 8;
    public static Action LevelCompleteEventListner;

    [Header("Pass Sprites Of Facing Cards")]
    [SerializeField] private List<Sprite> _cardFace = new List<Sprite>();
    [Header("Pass GameBoard Object Reference")]
    [SerializeField] private GameBoard _gameBoard = default;


    private void Start()
    {
        InitializeBoard();
    }

    private void OnEnable()
    {
        LevelCompleteEventListner += LevelComplete;
    }

    private void OnDisable()
    {
        LevelCompleteEventListner += LevelComplete;
    }

    public void StartNextLevel()
    {
        ShuffleCards(ref _cardFace);
        _gameBoard.ResetBoard(GetShuffledFaceCards());
    }
    public void LevelComplete()
    {
        StartCoroutine(StartGame());

        GameSoundManager.Instance.PlaySoundOneShot(GameSoundManager.SoundType.GameComplete);
    }

    //Function Shuffles the sprites so everytime new face card are spawn on to the board.
    private void InitializeBoard()
    {
        ShuffleCards(ref _cardFace);
        _gameBoard.SetBoard(GetShuffledFaceCards());
    }

    // Returns the number of cards to be placed on board from sprite list.
    private List<Sprite> GetShuffledFaceCards()
    {
        return _cardFace.Take(CardToPlaceOnBoard).ToList();
    }

    //For shuffling objects provide through List.
    public static void ShuffleCards<T>(ref List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3f);
        StartNextLevel();
    }
}
