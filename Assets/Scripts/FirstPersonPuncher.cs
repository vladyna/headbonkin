using UnityEngine;

public class FirstPersonPuncher : MonoBehaviour
{
    #region Serialized Fields
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 2f;

    [Header("Punching")]
    [SerializeField] private Transform punchOrigin;
    [SerializeField] private float punchRange = 2f;
    [SerializeField] private float punchForce = 500f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private KeyCode punchKey = KeyCode.Mouse0;
    #endregion
    #region Private Methods
    private CharacterController controller;
    private float pitch = 0f;
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
            TryPunch();
        }
    }
    #endregion
    #region Private Methods
    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.SimpleMove(move * speed);
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

    private void TryPunch()
    {
        if (Physics.Raycast(punchOrigin.position, punchOrigin.forward, out RaycastHit hit, punchRange, hitMask))
        {
            HeadHitDetector hitDetector = hit.collider.GetComponentInParent<HeadHitDetector>();
            if (hitDetector != null)
            {
                hitDetector.OnHit(punchOrigin.forward * punchForce, hit.point);
            }
        }
    }
    #endregion
}