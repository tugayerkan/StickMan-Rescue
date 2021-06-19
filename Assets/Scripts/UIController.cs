using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SencanUtils;
using Cysharp.Threading.Tasks;
using System;
using MU;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, IPointerClickHandler
{
    public Text timerText;
    public ObjectController placementConfirm;
    public Text levelWon;
    public Text levelLost;
    public Text level;
    public Image success;
    public Image tryAgain;
    public Image timeImg;
    public Button tapToPlay;
    public Image tapToPlayImage;

    Counter count;

    private bool _takingAway = false;
    public int secondsLeft = 30;

    void Start()
    {
        timerText.GetComponent<Text>().text = "" + secondsLeft;
        count = Counter.counter;
    }

    void Update()
    {

        if (_takingAway == false && secondsLeft > 0 && GameManager.gameState == GameState.Playing)
        {
            tapToPlay.gameObject.SetActive(false);
            tapToPlayImage.gameObject.SetActive(false);
            TimerCountdown();


        }
        //TODO: Fix this logic statement here
        //Fixed so Far
        else if (secondsLeft <= 0 && GameManager.gameState == GameState.Playing)
        {
            GameManager.gameState = GameState.Over;


        }
        else if (GameManager.gameState == GameState.Over)
        {
            StartCoroutine(UILostDelay());
        }
        else if (placementConfirm.isWin == true && ObjectController.boxIsPlaced == true && count.count > 1)
        {
            if (!tryAgain.gameObject.activeInHierarchy)
            {
                GameManager.gameState = GameState.LevelWon;
                timeImg.gameObject.SetActive(false);
                level.gameObject.SetActive(false);
                success.gameObject.SetActive(true);
            }
        }
    }
    private async void TimerCountdown()
    {
        _takingAway = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        secondsLeft -= 1;
        if (timerText != null)
        {
            timerText.GetComponent<Text>().text = "" + secondsLeft;
            timerText.color = Color.Lerp(timerText.color, Color.red, 0.3f);
        }
        _takingAway = false;
        if (secondsLeft < 5 && timerText != null)
        {
            // timeImg.color = Color.red;
            Timer.SetAlpha(timerText, true);
        }

    }
    private IEnumerator UILostDelay()
    {
        yield return new WaitForSeconds(4f);
        timeImg.gameObject.SetActive(false);
        level.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        GameManager.gameState = GameState.Playing;
        
    }
}
