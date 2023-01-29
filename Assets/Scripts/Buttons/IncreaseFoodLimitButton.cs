using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseFoodLimitButton : MonoBehaviour
{
    _GameManager _gameManager;
    [SerializeField] float upgradeCost;
    [SerializeField] AudioClip buttonClicked;
    [SerializeField] TextMeshProUGUI textToDisplay;
    [SerializeField] AudioClip buzzerSFX;

    void Awake()
    {
        _gameManager = FindObjectOfType<_GameManager>();
        textToDisplay.text = $"Food Increase: ${upgradeCost}";
    }

    public void ButtonPressed()
    {
        if(_gameManager.playerMoney >= upgradeCost)
        {
            _gameManager.subtractMoney(upgradeCost);
            _gameManager.amountOfFoodOnScreenMax++;
            upgradeCost = upgradeCost + 1000f;
            AudioSource.PlayClipAtPoint(buttonClicked, Camera.main.transform.position);
        }
        if(_gameManager.playerMoney < upgradeCost)
        {
            AudioSource.PlayClipAtPoint(buzzerSFX, Camera.main.transform.position);
        }
    }

}
