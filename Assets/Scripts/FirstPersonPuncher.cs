using DG.Tweening;
using UnityEngine;

public class FirstPersonPuncher : MonoBehaviour
{
    #region Serialized Fields
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Animator animator;

    [Header("Punching")]
    [SerializeField] private Transform punchOrigin;
    [SerializeField] private float punchRange = 2f;
    [SerializeField] private float punchDuration = 0.1f;
    [SerializeField] private float punchForce = 500f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private KeyCode punchKey = KeyCode.Mouse0;

    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform hitTarget;
    #endregion
    #region Private Methods
    private CharacterController controller;
    private float pitch = 0f;
    private bool _rightPunch = true;
    #endregion
    #region Unity's Methods
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        MovePlayer();
        LookAround();

        if (Input.GetKeyDown(punchKey))
        {
            if (_rightPunch)
            {
                ThrowPunch(rightHandTarget);
                _rightPunch = false;
            }
            else
            {
                ThrowPunch(leftHandTarget);
                _rightPunch = true;
            }
        }
    }
    #endregion
    #region Private Methods
    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        Vector3 movement = move * speed;
        controller.SimpleMove(movement);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void ThrowPunch(Transform ikTarget)
    {
        var position = ikTarget.localPosition;
        Sequence posSequence = DOTween.Sequence();
        posSequence.Append(ikTarget.DOLocalMove(hitTarget.localPosition, punchDuration).SetEase(Ease.OutQuad));
        posSequence.Append(ikTarget.DOLocalMove(position, punchDuration).SetEase(Ease.InQuad));
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    #endregion
}