using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishButton : MonoBehaviour
{
    public GameObject fishToSpawnPrefab;
    [SerializeField] float fishCost;
    [SerializeField] _GameManager _gameManager;
    [SerializeField] GameObject spawnLocation;
    [SerializeField] AudioClip[] fishBoughtSFX;
    [SerializeField] AudioClip notEnoughMoneySFX;
    [SerializeField] TextMeshProUGUI textToDisplay;
    
    void Awake()
    {
        textToDisplay.text = $"Buy Fish: ${fishCost}";
    }

    public void ButtonClicked()
    {
        //check points then tell it to subtract then spawn
        if(_gameManager.playerMoney >= fishCost)
        {
            _gameManager.subtractMoney(fishCost);
            Vector2 randomPos = new Vector2(Random.Range(spawnLocation.transform.position.x - spawnLocation.transform.localScale.x*9, spawnLocation.transform.position.x + spawnLocation.transform.localScale.x*9), Random.Range(spawnLocation.transform.position.y+2 - spawnLocation.transform.localScale.y, spawnLocation.transform.position.y+2 + spawnLocation.transform.localScale.y));
            Debug.Log(spawnLocation.transform.position.x);
            Debug.Log(spawnLocation.transform.localScale.x);
            Instantiate(fishToSpawnPrefab, randomPos, spawnLocation.transform.rotation);
            AudioClip sfx = fishBoughtSFX[Random.Range(0, fishBoughtSFX.Length)];
            AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(notEnoughMoneySFX, Camera.main.transform.position);
        }
    }
}
