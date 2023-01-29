using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    [SerializeField] float timeToDestroy;
    _GameManager _gameManager;
    [SerializeField] AudioClip[] fishEatSFX;

    [SerializeField] float timerTillFoodDestroy;
    CircleCollider2D circleCollider;


    void Awake()
    {
        _gameManager = FindObjectOfType<_GameManager>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {

        if (otherCollider.gameObject.tag == "Fish")
        {
            Debug.Log("Collided with Fish");
            BasicFish fish = otherCollider.gameObject.GetComponent<BasicFish>();
            if (fish.fishState == BasicFish.FishState.Hungry) 
            {
                StartCoroutine(timeUntilFoodEaten());
                fish.animator.SetTrigger("isEating");
                fish.fishState = BasicFish.FishState.Normal;
                fish.hungerTimer = fish.hungerTimerBase;

                AudioClip sfx = fishEatSFX[Random.Range(0, fishEatSFX.Length)];
                AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);
                fish.animator.SetBool("isHungry", false);
                fish.animator.SetBool("isMoving", true);
                
                fish.randomTargetPosition = fish.GetRandomPosition();
            }
        }
        if (otherCollider.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            StartCoroutine(foodCountDown());
        }
    }

    IEnumerator foodCountDown()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
    }

    IEnumerator timeUntilFoodEaten()
    {  
        circleCollider.enabled = false;
        yield return new WaitForSeconds(timerTillFoodDestroy);
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        _gameManager.amountOfFoodOnScreenCurrent = _gameManager.amountOfFoodOnScreenCurrent - 1;
    }
}
