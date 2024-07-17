using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float minXPosition = 5.54f;
    [SerializeField] float maxX = 10.49f;

    private float deltaX, deltaY;
    private Rigidbody2D rb;

    // Vector2 moveX = ;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXpos(), minXPosition, maxX);
        transform.position = paddlePos;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;

                    break;
                case TouchPhase.Moved:
                    rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;
                case TouchPhase.Ended:
                    rb.velocity = Vector2.zero;
                    break;

            }
        }
    }

    public bool isAutoPlayEnabled = false;
    public float GetXpos()
    {
        if (/*GameSession.Instance.IsAutoPlayEnabled()*/ isAutoPlayEnabled)
        {
            return FindObjectOfType<Ball>().transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
            //return Input.touches[Input.touches.Length-1].position.x / Screen.width * screenWidthInUnits;
        } 
    }    
}
