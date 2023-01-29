using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, IPointerClickHandler
{
    public float health;
    [SerializeField] _GameManager _gameManager;
    [SerializeField] float enemyMovementSpeed;

    SpriteRenderer sr;

    [SerializeField] AudioClip[] laserSFX;
    
    void Awake()
    {
        _gameManager = FindObjectOfType<_GameManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        EnemyPathing();
    }

    public void DamageToTake(float damageDealt)
    {
        health = health - damageDealt;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DamageToTake(_gameManager.currentAmountOfDamage);
        AudioClip sfx = laserSFX[Random.Range(0, laserSFX.Length)];
        AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Fish" || 
            otherCollider.tag == "Pirahna")
        {
            Destroy(otherCollider.gameObject);
        } 
    }

    public void EnemyPathing()
    {
        GameObject[] fishes = GameObject.FindGameObjectsWithTag("Fish");
        GameObject[] pirahnas = GameObject.FindGameObjectsWithTag("Pirahna");
        fishes = fishes.Concat(pirahnas).ToArray();
        float currentMinDistanceToFish = float.MaxValue;
        GameObject currentClosestFish = null;
        foreach(GameObject fishObject in fishes)
        {
            float fishDistance = Vector2.Distance(fishObject.transform.position, transform.position);
            if(fishDistance < currentMinDistanceToFish)
            {
                currentClosestFish = fishObject;
                currentMinDistanceToFish = fishDistance;
            }
        }
        if(currentClosestFish != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentClosestFish.transform.position, enemyMovementSpeed * Time.deltaTime);
            CheckSpriteDirection(currentClosestFish.transform.position);
        }
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
}
