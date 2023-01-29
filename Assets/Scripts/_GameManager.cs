using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class _GameManager : MonoBehaviour
{
    public float playerMoney;
    public UI ui;
    [SerializeField] float playerStartMoney;
    [SerializeField] float foodCost;
    [SerializeField] GameObject basicFoodPrefab;
    public int amountOfFoodOnScreenMax;
    public int amountOfFoodOnScreenCurrent;
    public float currentAmountOfDamage;

    [SerializeField] GameObject starterFish;
    [SerializeField] GameObject spawnLocation;

    [SerializeField] GameObject[] possibleEnemySpawns;
    [SerializeField] float initialMonsterTimer;
    private bool initialMonsterSpawned = false;
    [SerializeField] float randomMinMonsterTimer;
    [SerializeField] float randomMaxMonsterTimer;
    private bool randomMonsterTimerChosen = false;
    public float randomMonsterSpawnTimer;
    [SerializeField] GameObject enemySpawnPosition;
    [SerializeField] AudioClip AWOOGA;
    [SerializeField] GameObject loseImage;
    
    void Awake()
    {
        Vector2 randomPos = new Vector2(Random.Range(spawnLocation.transform.position.x - spawnLocation.transform.localScale.x*9, spawnLocation.transform.position.x + spawnLocation.transform.localScale.x*9), Random.Range(spawnLocation.transform.position.y+2 - spawnLocation.transform.localScale.y, spawnLocation.transform.position.y+2 + spawnLocation.transform.localScale.y));
        Instantiate(starterFish, randomPos, spawnLocation.transform.rotation);
    }

    void Start()
    {
        playerMoney = playerMoney + playerStartMoney;
    }

    void Update()
    {
        SpawnMonster();
    }

    public void addMoney(float moneyToAdd)
    {
        //add money code here
        playerMoney = playerMoney + moneyToAdd;

        ui.updateMoneyAmountVisual();
    }

    public void subtractMoney(float moneyToSubtract)
    {
        //subtract money code here
        playerMoney = playerMoney - moneyToSubtract;

        //update the UI
        ui.updateMoneyAmountVisual();
    }

    void OnSpawnFood(InputValue value)
    {
        if(playerMoney >= foodCost && amountOfFoodOnScreenCurrent < amountOfFoodOnScreenMax)
        {
            amountOfFoodOnScreenCurrent++;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Instantiate(basicFoodPrefab, worldPos, Quaternion.identity);
            subtractMoney(foodCost);
        }
    }

    public void GameLoseChecker()
    {
        Debug.Log("You lose!");
        loseImage.SetActive(true);
        StartCoroutine(QuitToMenu());
        playerMoney = 0;
    }

    void SpawnMonster()
    {
        //TODO: make a warning when half or 3/4ths of the time is left
        initialMonsterTimer = initialMonsterTimer - Time.deltaTime;
        if(initialMonsterTimer <= 0 && initialMonsterSpawned == false)
        {
            StartCoroutine(SOUNDTHEAWOOGA());
            initialMonsterSpawned = true;
        }
        randomMonsterSpawnTimer = randomMonsterSpawnTimer - Time.deltaTime;
        if(randomMonsterSpawnTimer <= 0 && randomMonsterTimerChosen == true)
        {
            randomMonsterTimerChosen = false;
            StartCoroutine(SOUNDTHEAWOOGA());
        }

    }

    void ChooseRandomEnemy()
    {
        int randomEnemy = Random.Range(0, possibleEnemySpawns.Length);
        Vector2 randomEnemySpawnPosition = new Vector2(Random.Range(enemySpawnPosition.transform.position.x - enemySpawnPosition.transform.localScale.x * 8, enemySpawnPosition.transform.position.x + enemySpawnPosition.transform.localScale.x * 8), 
        Random.Range(enemySpawnPosition.transform.position.y + 2 - enemySpawnPosition.transform.localScale.y, enemySpawnPosition.transform.position.y + 2 + enemySpawnPosition.transform.localScale.y));
        Instantiate(possibleEnemySpawns[randomEnemy], randomEnemySpawnPosition,enemySpawnPosition.transform.rotation);
        randomMonsterSpawnTimer = Random.Range(randomMinMonsterTimer, randomMaxMonsterTimer);
        randomMonsterTimerChosen = true;
    }

    IEnumerator SOUNDTHEAWOOGA()
    {
        AudioSource.PlayClipAtPoint(AWOOGA, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(2);
        AudioSource.PlayClipAtPoint(AWOOGA, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(2);
        AudioSource.PlayClipAtPoint(AWOOGA, Camera.main.transform.position);
        yield return new WaitForSecondsRealtime(5);
        ChooseRandomEnemy();
    }

    IEnumerator QuitToMenu()
    {
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene("TitleScreen");
    }
}
