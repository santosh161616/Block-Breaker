using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private float _x, _y;

    // Update is called once per frame
    void Update()
    {
        _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(_x,_y)*Time.deltaTime,_rawImage.uvRect.size);
    }
}
