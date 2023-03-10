using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    [SerializeField]
    private Collider2D thisCollider2D;

    public void Move()
    {
        transform.position = new Vector2(transform.position.x, GameManager.instance.nextPlatformPositionY);
        GameManager.instance.nextPlatformPositionY += 6;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) thisCollider2D.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) thisCollider2D.enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (GameManager.instance.gameInProgress)
        {
            Move();
        }
    }
}
