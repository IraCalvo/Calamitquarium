using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageUpgrade : MonoBehaviour
{
    _GameManager _gameManager;
    [SerializeField] float upgradeCost;
    [SerializeField] float gunDamageIncrease;
    [SerializeField] float gunDamageMultipler;
    [SerializeField] TextMeshProUGUI textToDisplay;
    [SerializeField] AudioClip buzzerSFX;
    [SerializeField] AudioClip buttonClickedSFX;

    void Awake()
    {
        _gameManager = FindObjectOfType<_GameManager>();
        textToDisplay.text = $"Upgrade Damage: ${upgradeCost}";
    }

    public void ButtonPressed()
    {
        if(_gameManager.playerMoney >= upgradeCost)
        {
            textToDisplay.text = $"Upgrade Damage: ${upgradeCost}";
            _gameManager.subtractMoney(upgradeCost);
            AudioSource.PlayClipAtPoint(buttonClickedSFX, Camera.main.transform.position);
            _gameManager.currentAmountOfDamage = _gameManager.currentAmountOfDamage + gunDamageIncrease;
            gunDamageIncrease = gunDamageIncrease * gunDamageMultipler;
            upgradeCost = upgradeCost + 1000f;
        }
        if(_gameManager.playerMoney < upgradeCost)
        {
            AudioSource.PlayClipAtPoint(buzzerSFX, Camera.main.transform.position);
        }
    }
}
