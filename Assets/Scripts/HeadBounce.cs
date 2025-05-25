using UnityEngine;
using DG.Tweening;
public class HeadBounce : MonoBehaviour
{
    #region Seriliazed Fields
    [SerializeField] private Transform neckTransform;
    [SerializeField] private Transform headTransform;
    [SerializeField] private float wobbleAngle = 15f;
    [SerializeField] private float wobbleDuration = 0.2f;
    #endregion
    #region Private Fields
    private Quaternion neckOriginalRot;
    private Quaternion bounceFromRot;
    private Quaternion bounceToRot;

    private float bounceStartTime;
    private float returnStartTime;
    private bool isReturning;
    private bool isWobbling;
    #endregion
    #region Unity's Methods
    private void Awake()
    {
        neckOriginalRot = neckTransform.localRotation;
    }
    #endregion
    #region Unity's Methods
    void LateUpdate()
    {
        if (isWobbling)
        {
            float t = (Time.time - bounceStartTime) / wobbleDuration;
            float eased = Mathf.SmoothStep(0, 1, Mathf.Sin(t * Mathf.PI));

            neckTransform.localRotation = Quaternion.Slerp(bounceFromRot, bounceToRot, eased);

            if (t >= 1f)
            {
                isWobbling = false;
                isReturning = true;
                returnStartTime = Time.time;
                bounceFromRot = neckTransform.localRotation;
                bounceToRot = neckOriginalRot;
            }
        }
        else if (isReturning)
        {
            float t = (Time.time - returnStartTime) / wobbleDuration;
            float eased = Mathf.SmoothStep(0, 1, t);

            neckTransform.localRotation = Quaternion.Slerp(bounceFromRot, bounceToRot, eased);

            if (t >= 1f)
            {
                isReturning = false;
                neckTransform.localRotation = neckOriginalRot;
            }
        }
    }
    #endregion
    #region Public Methods
    public void Bounce(Vector3 hitDirection)
    {
        Vector3 localDir = headTransform.InverseTransformDirection(hitDirection.normalized);

        Vector3 axis = Vector3.Cross(localDir, Vector3.up).normalized * -1;

        float neckWobbleAngle = wobbleAngle * 0.5f;
        bounceFromRot = neckTransform.localRotation;
        bounceToRot = Quaternion.AngleAxis(neckWobbleAngle, axis) * neckOriginalRot;

        bounceStartTime = Time.time;
        isWobbling = true;
        isReturning = false;
    }
    #endregion
}
