using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinButton : MonoBehaviour
{
    [SerializeField] _GameManager _gameManager;
    private bool playerCanBuyWinConditionAgain = true;
    [SerializeField] float costToBuy;
    [SerializeField] float timeForPlayerToWait;
    [SerializeField] float amountOfTimesPlayerHasToBuyWinCondition;
    private float winCounter;
    [SerializeField] AudioClip buttonClicked;
    [SerializeField] AudioClip buzzerSFX;
    [SerializeField] TextMeshProUGUI textToDisplay;
    [SerializeField] GameObject winScreen;


    void Awake ()
    {
        _gameManager = FindObjectOfType<_GameManager>();
        textToDisplay.text = $"ESCAPE TANK: ${costToBuy}";
    }

    public void ButtonPressed()
    {
        if (playerCanBuyWinConditionAgain == false || _gameManager.playerMoney < costToBuy)
        {
            AudioSource.PlayClipAtPoint(buzzerSFX, Camera.main.transform.position);
        }
        if(_gameManager.playerMoney >= costToBuy && playerCanBuyWinConditionAgain == true)
        {
            _gameManager.subtractMoney(costToBuy);
            costToBuy = costToBuy *1.5f;
            textToDisplay.text = $"ESCAPE TANK: ${costToBuy}";
            AudioSource.PlayClipAtPoint(buttonClicked, Camera.main.transform.position);
            _gameManager.randomMonsterSpawnTimer = _gameManager.randomMonsterSpawnTimer / 2;
            winCounter++;
            playerCanBuyWinConditionAgain = false;
            if(winCounter == amountOfTimesPlayerHasToBuyWinCondition)
            {
                Debug.Log("Player won!");
                winScreen.SetActive(true);
                StartCoroutine(waitToGetToNextLevel());
            }
            StartCoroutine(timerForPlayerToWinAgain());
        }

    }
    
    IEnumerator waitToGetToNextLevel()
    {
        yield return new WaitForSecondsRealtime(3);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    
    IEnumerator timerForPlayerToWinAgain()
    {
        yield return new WaitForSecondsRealtime(timeForPlayerToWait);
        playerCanBuyWinConditionAgain = true;
    }
}
