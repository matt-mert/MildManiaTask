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

    public BallState ballState { get; private set; }

    private Rigidbody ballRigidbody;
    private SphereCollider ballCollider;
    private GameObject targetObject;

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
        if (ballState == BallState.ATTRACTED) return;

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

        ballState = BallState.ATTRACTED;

        targetObject = caller;
        Vector3 direction = (caller.transform.position - transform.position).normalized;
        ballRigidbody.velocity = direction * attractedSpeed;
    }

    private void HitUnmatchedHandler()
    {
        ballState = BallState.IDLE;
        ballRigidbody.velocity = Vector3.zero;
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
