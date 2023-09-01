using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallBehaviour : MonoBehaviour, ITappable
{
    // Dependency to ManagerLevel class.

    public delegate void OnBallTappedDelegate(GameObject obj);
    public static event OnBallTappedDelegate OnBallTapped;
    public delegate void OnHitUnmatchedDelegate();
    public static event OnHitUnmatchedDelegate OnHitUnmatched;
    public delegate void OnHitMatchedDelegate(BallColor color);
    public static event OnHitMatchedDelegate OnHitMatched;

    [SerializeField]
    private BallColor myColor;

    [SerializeField]
    [Tooltip("The particles to be initiated upon popping.")]
    private GameObject particlesPrefab;

    [SerializeField]
    [Tooltip("The speed during the attracted movement.")]
    private float attractedSpeed;

    [SerializeField]
    private bool isMoving;

    [Header("Fill the remaining sections if ball is moving.")]

    [SerializeField]
    [Range(1f, 10f)]
    private float movingSpeed;
    [SerializeField]
    [Range(0f, 2f)]
    private float movingDuration;

    [Header("Program normalizes the direction automatically.")]

    [SerializeField]
    private float ballDirectionX;
    [SerializeField]
    private float ballDirectionY;

    public BallState ballState { get; private set; }

    private Rigidbody ballRigidbody;
    private SphereCollider ballCollider;
    private GameObject targetObject;
    private Vector3 ballDirection;
    private Coroutine movementCoroutine;
    private bool isComingBack;
    private bool initialFlag;

    public enum BallColor
    {
        RED,
        BLUE,
        GREEN,
        PINK,
        WHITE,
        BLACK,
    }

    public enum BallState
    {
        IDLE,
        SELECTED,
        ATTRACTED,
    }

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballCollider = GetComponent<SphereCollider>();
        targetObject = null;
    }

    private void OnEnable()
    {
        ManagerLevel.Instance.OnColorFinished += ColorFinishedHandler;
        OnBallTapped += BallTappedHandler;
        OnHitUnmatched += HitUnmatchedHandler;
    }

    private void OnDisable()
    {
        ManagerLevel.Instance.OnColorFinished -= ColorFinishedHandler;
        OnBallTapped -= BallTappedHandler;
        OnHitUnmatched -= HitUnmatchedHandler;
    }

    private void Start()
    {
        ballState = BallState.IDLE;
        movementCoroutine = null;
        isComingBack = false;
        initialFlag = true;
        if (isMoving)
        {
            ballDirection = new Vector3(ballDirectionX, 0f, ballDirectionY).normalized;
        }
        else
        {
            ballDirection = Vector3.zero;
        }

        StartCoroutine(InitialCoroutine());
    }

    private void Update()
    {
        if (ballState == BallState.ATTRACTED)
        {
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;
            ballRigidbody.velocity = direction * attractedSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BallObject")) return;
        if (ballState == BallState.SELECTED) return;

        BallBehaviour otherBehaviour = other.gameObject.GetComponent<BallBehaviour>();
        BallColor otherColor = otherBehaviour.GetBallColor();
        if (otherColor == myColor)
        {
            if (otherBehaviour.ballState == BallState.SELECTED)
            {
                Instantiate(particlesPrefab, transform.position, Quaternion.identity);
                OnHitMatched?.Invoke(myColor);
                Destroy(gameObject);
            }
        }
        else
        {
            OnHitUnmatched?.Invoke();
        }
    }

    private IEnumerator InitialCoroutine()
    {
        ballCollider.isTrigger = false;
        ballRigidbody.useGravity = true;
        yield return new WaitForSeconds(2);
        ballRigidbody.useGravity = false;
        ballCollider.isTrigger = true;
        initialFlag = false;
        if (isMoving) movementCoroutine = StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        while (isMoving)
        {
            if (isComingBack)
            {
                ballRigidbody.velocity = ballDirection * movingSpeed * -1f;
            }
            else
            {
                ballRigidbody.velocity = ballDirection * movingSpeed;
            }
            yield return new WaitForSeconds(movingDuration);
            isComingBack = !isComingBack;
        }
    }

    public BallColor GetBallColor()
    {
        return myColor;
    }

    public void MultiplyAttractedSpeed(float amount)
    {
        attractedSpeed *= amount;
    }

    public void OnTapBehaviour()
    {
        if (ballState == BallState.ATTRACTED || initialFlag) return;

        ballState = BallState.SELECTED;
        ballRigidbody.isKinematic = true;
        OnBallTapped?.Invoke(gameObject);
    }

    private void BallTappedHandler(GameObject caller)
    {
        // Debug.Log("Hello, I am " + gameObject.name + " and I know you clicked on " + caller.name);
        
        if (caller == gameObject) return;

        BallBehaviour callerBehaviour = caller.GetComponent<BallBehaviour>();
        BallColor callerColor = callerBehaviour.GetBallColor();

        if (callerColor != myColor) return;

        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        ballState = BallState.ATTRACTED;

        targetObject = caller;
        Vector3 direction = (caller.transform.position - transform.position).normalized;
        ballRigidbody.velocity = direction * attractedSpeed;
    }

    private void HitUnmatchedHandler()
    {
        ballState = BallState.IDLE;
        ballRigidbody.isKinematic = true;
    }

    private void ColorFinishedHandler(BallColor color)
    {
        if (color == myColor && ballState == BallState.SELECTED)
        {
            Instantiate(particlesPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
