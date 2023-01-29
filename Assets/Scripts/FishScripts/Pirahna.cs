using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirahna : MonoBehaviour
{
    public enum FishState
    {
        Normal,
        Hungry,
        Dead
    }

    _GameManager _gameManager;

    public FishState fishState;

    [SerializeField] float fishSpeedMax;
    [SerializeField] float fishSpeedMin;
    [SerializeField] float fishHungerSpeed;
    

    [SerializeField] GameObject moneyToSpawnPrefab;
    [SerializeField] float timerToDropMoney;
    [SerializeField] float baseTimerToDropMoney;

    [SerializeField] float locationChooserMinTime;
    [SerializeField] float locationChooserMaxTime;
    private float locationChooserTime;
    private bool shouldMove = false;

    public Vector2 randomTargetPosition;

    public float hungerTimer;
    public float hungerTimerBase;
    [SerializeField] float starvationTime;

    SpriteRenderer sr;

    void Awake()
    {
        _gameManager = FindObjectOfType<_GameManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        hungerTimerBase = hungerTimer;
        baseTimerToDropMoney = timerToDropMoney;
        locationChooserTime = Random.Range(locationChooserMinTime, locationChooserMaxTime);
        randomTargetPosition = GetRandomPosition();
    }

    void Update()
    {
        Movement();
        DropMoney();
        ChooseLocationTimer();
        Hungry();
    }

    public void Hungry()
    {
        hungerTimer = hungerTimer - Time.deltaTime;
        if(hungerTimer <= 0)
        {
            //TODO: Change fish sprite
            fishState = FishState.Hungry;
        }
        if(hungerTimer <= starvationTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void DropMoney()
    {
        if(timerToDropMoney <= 0)
        {
            Instantiate(moneyToSpawnPrefab, this.transform.position, Quaternion.identity);
            timerToDropMoney = baseTimerToDropMoney;
        }
        else
        {
            timerToDropMoney = timerToDropMoney - Time.deltaTime;
        }
    }

    public void ChooseLocationTimer()
    {
        if(locationChooserTime <= 0)
        {
            shouldMove = true;
        }
        else
        {
            locationChooserTime = locationChooserTime -Time.deltaTime;
        }
        
    }

    public void Movement()
    {
        if(fishState == FishState.Hungry)
        {
            BasicFish[] basicFishScripts = FindObjectsOfType<BasicFish>();
            float currentMinDistance = float.MaxValue;
            GameObject currentClosestBasicFish = null;
            foreach (BasicFish basicFishScript in basicFishScripts)
            {
                GameObject basicFishObject = basicFishScript.gameObject;
                float basicFishDistance = Vector2.Distance(basicFishObject.transform.position, transform.position);
                if(basicFishDistance < currentMinDistance && basicFishDistance < 2)
                {
                    currentClosestBasicFish = basicFishObject;
                    currentMinDistance = basicFishDistance;
                }
            }

            if (currentClosestBasicFish != null) 
            {
                transform.position = Vector2.MoveTowards(transform.position, currentClosestBasicFish.transform.position, fishHungerSpeed * Time.deltaTime);
                CheckSpriteDirection(currentClosestBasicFish.transform.position);
            }
            else 
            {
                MoveToTargetPosition();
                CheckSpriteDirection(randomTargetPosition);
            }

        }
        else if(shouldMove)
        {
            MoveToTargetPosition();
            CheckSpriteDirection(randomTargetPosition);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.05f * Time.deltaTime);
        }
        
    }

    public void MoveToTargetPosition() 
    {
        float randomFishSpeed = Random.Range(fishSpeedMin, fishSpeedMax);

        // Move towards location
        transform.position = Vector2.MoveTowards(transform.position, randomTargetPosition, randomFishSpeed * Time.deltaTime);
        // Did we reach location?
        if(Vector2.Distance(transform.position, randomTargetPosition) < 0.1f) 
        {
            shouldMove = false;
            GetRandomPosition();
            locationChooserTime = Random.Range(locationChooserMinTime, locationChooserMaxTime);
        }
    }

    public Vector2 GetRandomPosition()
    {
        Vector3 viewportMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 viewportMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        RaycastHit2D hit = Physics2D.Linecast(transform.position, randomTargetPosition);
        float randomX = Random.Range(viewportMin.x, viewportMax.x);
        float randomY = Random.Range(viewportMin.y, viewportMax.y);
        randomTargetPosition = new Vector2(randomX, randomY);
        hit = Physics2D.Linecast(transform.position, randomTargetPosition);
        if (hit.collider.gameObject.tag == "Ground")
        {
            randomTargetPosition = new Vector2(0, 0);
        }
        return randomTargetPosition;
    }

    void CheckSpriteDirection(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }

    void OnDestroy()
    {
        GameObject[] currentAmountOfFish = GameObject.FindGameObjectsWithTag("Fish");

        if(currentAmountOfFish.Length == 0)
        {
            _gameManager.GameLoseChecker();
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (fishState == FishState.Hungry && otherCollider.tag == "Fish")
        {
            Destroy(otherCollider.gameObject);
            fishState = FishState.Normal;
            hungerTimer = hungerTimerBase;
        }
    }
}
