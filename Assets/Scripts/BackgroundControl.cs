using UnityEngine;
using UnityEngine.UI;

public class BackgroundControl : MonoBehaviour
{
    [SerializeField]
    private RawImage rawImage;

    private float uvRectY;

    private void Update()
    {
        uvRectY = Camera.main.transform.position.y * rawImage.uvRect.height / 10;// rawImage.uvRect.height / (10 * transform.localScale.y) * Time.deltaTime;
        rawImage.uvRect = new Rect(rawImage.uvRect.x, uvRectY, rawImage.uvRect.width, rawImage.uvRect.height);
    }
}
