using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 sourcePosition;
    private Vector2 anchorPosition;
    private Vector2 secondAnchorPosition;
    private Vector2 targetPosition;

    private readonly int speed = 12;
    private readonly float distanceX = 0.6425f;
    private float maxDistanceX;

    public Rigidbody2D thisRigidbody2D = null;

    private bool passedThePlatform = true;
    [NonSerialized]
    public bool followMe = true;

    [SerializeField]
    private AudioClip respawnAudio = null;

    private void Start()
    {
        maxDistanceX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).x - distanceX;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= transform.position.y + 5)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sourcePosition = transform.position;
                targetPosition = Vector2.zero;
                anchorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                secondAnchorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition = new Vector2(secondAnchorPosition.x - anchorPosition.x, secondAnchorPosition.y - anchorPosition.y);
            }
        }

#elif UNITY_ANDROID || UNITY_IOS

        if (Input.touchCount > 0)
        {
            if (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y <= 3)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    sourcePosition = transform.position;
                    anchorPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    secondAnchorPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    targetPosition = new Vector2(secondAnchorPosition.x - anchorPosition.x, secondAnchorPosition.y - anchorPosition.y);
                }
            }
        }

#endif

        transform.position = Vector2.Lerp(transform.position, new Vector2(Mathf.Clamp(sourcePosition.x + targetPosition.x, -maxDistanceX, maxDistanceX), transform.position.y), speed * Time.deltaTime);

        if (!passedThePlatform && transform.position.y > GameManager.instance.platforms[GameManager.instance.currentPlatform].transform.position.y)
        {
            passedThePlatform = true;
        }

        if (passedThePlatform)
        {
            if (followMe && transform.position.y < GameManager.instance.platforms[GameManager.instance.currentPlatform].transform.position.y)
            {
                followMe = false;
            }
            else if (!followMe && transform.position.y > GameManager.instance.platforms[GameManager.instance.currentPlatform].transform.position.y)
            {
                followMe = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.instance.gameInProgress)
        {
            if (collision.gameObject != GameManager.instance.platforms[GameManager.instance.currentPlatform])
            {
                GameManager.instance.StartGame();
                GameManager.instance.currentPlatform++;
                if (GameManager.instance.currentPlatform >= GameManager.instance.platforms.Length) GameManager.instance.currentPlatform = 0;
            }
        }

        if (GameManager.instance.gameInProgress)
        {
            ScoreManager.instance.IncreaseScore();

            GameManager.instance.currentPlatform++;
            if (GameManager.instance.currentPlatform >= GameManager.instance.platforms.Length) GameManager.instance.currentPlatform = 0;

            passedThePlatform = false;
        }

        thisRigidbody2D.velocity = Vector2.zero;
        thisRigidbody2D.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
    }

    private void OnBecameInvisible()
    {
        if (GameManager.instance.gameInProgress)
        {
            GameManager.instance.GameOver();
        }

        sourcePosition = new Vector2(GameManager.instance.platforms[GameManager.instance.currentPlatform].transform.position.x, Camera.main.transform.position.y - 6);
        targetPosition = Vector2.zero;
        transform.position = sourcePosition;

        thisRigidbody2D.velocity = Vector2.zero;
        thisRigidbody2D.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
        SongsManager.instance.audioSource.PlayOneShot(respawnAudio, 0.8f);
    }
}
