using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallBehaviour : MonoBehaviour, ITappable
{
    public delegate void OnBallTappedDelegate(GameObject obj);
    public static event OnBallTappedDelegate OnBallTapped;
    public delegate void OnHitUnmatchedDelegate();
    public static event OnHitUnmatchedDelegate OnHitUnmatched;
    public delegate void OnHitMatchedDelegate();
    public static event OnHitMatchedDelegate OnHitMatched;

    [SerializeField]
    private GameObject particlesPrefab;

    private BallSO ballProperties;
    private Rigidbody ballRigidbody;
    private float ballSpeed;
    private bool selectedFlag;

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        OnBallTapped += BallTappedHandler;
    }

    private void OnDisable()
    {
        OnBallTapped -= BallTappedHandler;
    }

    private void Start()
    {
        ballSpeed = 10;
    }

    private void Update()
    {
        if (!ballProperties.isMoving) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BallObject")) return;
        if (selectedFlag) return;

        BallSO otherSO = other.gameObject.GetComponent<BallBehaviour>().GetBallProperties();
        if (otherSO.ballColor == ballProperties.ballColor)
        {
            Instantiate(particlesPrefab, transform.position, Quaternion.identity);
            OnHitMatched?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            OnHitUnmatched?.Invoke();
        }
    }

    public BallSO GetBallProperties()
    {
        return ballProperties;
    }

    public void SetBallProperties(BallSO ballSO)
    {
        ballProperties = ballSO;
    }

    public void OnTapBehaviour()
    {
        selectedFlag = true;
        OnBallTapped?.Invoke(gameObject);
    }

    private void BallTappedHandler(GameObject caller)
    {
        // Debug.Log("Hello, I am " + gameObject.name + " and I know you clicked on " + caller.name);
        
        if (caller == gameObject) return;

        BallSO.BallColor callerColor = caller.GetComponent<BallBehaviour>().GetBallProperties().ballColor;
        if (callerColor != ballProperties.ballColor) return;

        Vector3 direction = (caller.transform.position - transform.position).normalized;
        ballRigidbody.velocity = direction * ballSpeed;
    }
}
