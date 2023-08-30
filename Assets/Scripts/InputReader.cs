using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerActions playerActions;
    private Camera mainCamera;
    private bool firstTouch;

    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void Start()
    {
        playerActions.MobileActions.TouchContact.started += ctx => PlayerTouchStarted(ctx);

        mainCamera = Camera.main;
        firstTouch = true;
    }

    private void PlayerTouchStarted(InputAction.CallbackContext context)
    {
        if (firstTouch)
        {
            firstTouch = false;
            StartCoroutine(FirstTouchDebugCoroutine(context));
            return;
        }
        Vector2 touchVector = playerActions.MobileActions.TouchPosition.ReadValue<Vector2>();
        Ray touchRay = mainCamera.ScreenPointToRay(touchVector);
        if (Physics.Raycast(touchRay, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tappable")))
        {
            ITappable tappable = hit.transform.GetComponent<ITappable>();
            if (tappable != null)
            {
                tappable.OnTapBehaviour();
            }
        }
    }

    private IEnumerator FirstTouchDebugCoroutine(InputAction.CallbackContext context)
    {
        yield return new WaitForEndOfFrame();
        PlayerTouchStarted(context);
    }
}
