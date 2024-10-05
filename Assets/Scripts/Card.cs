using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <Comment Start>  Class Purpose
/// Class for managing state of cards as well as handeling card functionality
/// </Class Purpose>

public class Card : MonoBehaviour
{
    public int CardID { private set; get; }
    public enum CardSides { Front, Back }

    [Header("Front and Back Side Card Images")]
    [SerializeField] private Transform _cardFront, _cardBack;
    private Button _cardInteractionBtn;
    private Action<Card> _callbackSelectedCardToGameBoard;
    private float _cardRotationVelocity;
    private bool flipAnimFlag = true;

    //Initialize the card front with sprite, adds button and binds interation listener to button
    public void Init(int cardId, Sprite cardFace, Action<Card> callbackSelectedCard)
    {
        _callbackSelectedCardToGameBoard = callbackSelectedCard;
        CardID = cardId;
        _cardFront.GetComponent<Image>().sprite = cardFace;
        _cardInteractionBtn = transform.gameObject.AddComponent<Button>();
        _cardInteractionBtn.onClick.AddListener(CardInteraction);
        ShowCardSide(CardSides.Front);
    }

    // for calling card side functionality
    public void ShowCardSide(CardSides cardSide)
    {
        if (!flipAnimFlag)
        {
            return;
        }

        switch (cardSide)
        {
            case CardSides.Front:
                {
                    StartCoroutine(CardAnimationRotateAnimation(_cardBack, _cardFront));
                    break;
                }
            case CardSides.Back:
                {
                    StartCoroutine(CardAnimationRotateAnimation(_cardFront, _cardBack));
                    break;
                }
        }

        GameSoundManager.Instance.PlaySoundOneShot(GameSoundManager.SoundType.CardFlip);
    }

    public void CardInteraction()
    {
        _cardInteractionBtn.interactable = false;
        _callbackSelectedCardToGameBoard.Invoke(this);
        ShowCardSide(CardSides.Front);
    }

    public void ResetCard()
    {
        ShowCardSide(CardSides.Back);
        _cardInteractionBtn.interactable = true;
    }

    public void DeactivateCardAnimated()
    {
        AnimateMatchCard();
        _cardInteractionBtn.interactable = false;
    }

    public void AnimateMatchCard()
    {
        StartCoroutine(scaleOverTime(_cardFront, new Vector3(0, 0, 0), 0.25f));
        StartCoroutine(scaleOverTime(_cardBack, new Vector3(0, 0, 0), 0.25f));
    }

    private IEnumerator CardAnimationRotateAnimation(Transform cardFront, Transform cardBack)
    {
        flipAnimFlag = false;
        cardBack.gameObject.SetActive(false);
        cardFront.gameObject.SetActive(true);
        cardFront.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        cardBack.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));

        while (true)
        {
            float Angle = Mathf.SmoothDampAngle(cardFront.eulerAngles.y, 90f, ref _cardRotationVelocity, 0.05f);
            cardFront.rotation = Quaternion.Euler(0, Angle, 0);
            if (cardFront.eulerAngles.y >= 89.0f)
            {
                cardFront.gameObject.SetActive(false);
                cardBack.gameObject.SetActive(true);
                break;
            }
            else
                yield return null;
        }

        while (true)
        {
            float Angle = Mathf.SmoothDampAngle(cardBack.eulerAngles.y, 0f, ref _cardRotationVelocity, 0.05f);
            cardBack.rotation = Quaternion.Euler(0, Angle, 0);
            if (cardBack.eulerAngles.y <= 0.1f)
            {
                break;
            }
            else
                yield return null;
        }

        cardFront.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        cardBack.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        flipAnimFlag = true;
    }

    private IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
        float counter = 0;
        Vector3 startScaleSize = objectToScale.localScale;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, toScale, counter / duration);
            yield return null;
        }

        objectToScale.gameObject.SetActive(false);
        objectToScale.localScale = Vector3.one;
    }
}