using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Net.Http;

public class Ball : MonoBehaviour
{
    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 7f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = .2f;

    //State
    Vector2 paddleToBallVector;
    private bool _hasStarted = false;
    private bool _enableInput = true;    

    //Cache component reference.
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;

    public static Ball instance;

    public bool HasStarted
    {
        set
        {
            Utility.myLog("Has stated " + _hasStarted);
            _hasStarted = value;
        }
    }

    public bool EnableInput
    {
        set
        {
            value = _enableInput;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasStarted)
        {
            LockBallToPaddle();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LaunchOnMouseClick();
        }
    }

    public void LaunchOnMouseClick()
    {
        if (_enableInput && !_hasStarted)
        {
            // enableButton.SetActive(false);
            _hasStarted = true;
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(Random.Range(-randomFactor, randomFactor), Random.Range(-randomFactor, randomFactor));
        if (_enableInput)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);

            myRigidBody2D.velocity = myRigidBody2D.velocity.normalized * 7f;
            //Debug.Log(myRigidBody2D.velocity.normalized + "Before Tweak" + velocityTweak);

            myRigidBody2D.velocity += velocityTweak;

            //Debug.Log(myRigidBody2D.velocity + "Velocity Tweak" + velocityTweak);
        }

    }
}
