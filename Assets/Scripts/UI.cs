using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI playerMoneyAmount; 
    [SerializeField] _GameManager gameManager;

    void Start()
    {
        playerMoneyAmount.text = $"{gameManager.playerMoney}";
    }

    public void updateMoneyAmountVisual()
    {
        playerMoneyAmount.text = $"{gameManager.playerMoney}";
    }
}
