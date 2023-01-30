using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Coin : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] float amountWorth;
    [SerializeField] _GameManager _gameManager;
    [SerializeField] float timeToDestroy;
    [SerializeField] float moveSpeed;
    GameObject coinBankLocation;
    bool isClicked = false;
    Rigidbody2D rb;
    CircleCollider2D coinCollider;

    [SerializeField] AudioClip coinSFX;

    void Awake()
    {
       _gameManager = FindObjectOfType<_GameManager>();
       transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        coinBankLocation = GameObject.Find("CoinBank");
        rb = GetComponent<Rigidbody2D>();
        coinCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if(isClicked == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, coinBankLocation.transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == coinBankLocation.transform.position)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClicked = true;
        rb.isKinematic = true;
        coinCollider.enabled = false;
        AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position);
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Ground")
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            StartCoroutine(coinCountDown());
        }
        if(otherCollider.tag == "MoneyBox")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator coinCountDown()
    {
        yield return new WaitForSeconds(timeToDestroy);
        if(isClicked == false)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if(isClicked)
        {
            _gameManager.addMoney(amountWorth);
        }
    }
}
