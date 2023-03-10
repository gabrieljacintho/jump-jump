using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.instance.playerControl.followMe)
        {
            transform.position = new Vector3(transform.position.x, GameManager.instance.playerControl.gameObject.transform.position.y + 2, transform.position.z);
        }
    }
}
