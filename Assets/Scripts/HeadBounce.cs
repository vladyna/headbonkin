using UnityEngine;
using DG.Tweening;
public class HeadBounce : MonoBehaviour
{
    #region Seriliazed Fields
    [SerializeField] private Transform neckTransform;
    [SerializeField] private Transform headTransform;

    [SerializeField] private float bounceDistance = 0.3f;
    [SerializeField] private float bounceDuration = 0.15f;
    [SerializeField] private float wobbleAngle = 15f;
    [SerializeField] private float wobbleDuration = 0.2f;
    #endregion
    #region Private Fields
    private Vector3 headOriginalPos;
    private Quaternion headOriginalRot;
    private Quaternion neckOriginalRot;
    #endregion
    #region Unity's Methods
    private void Awake()
    {
        headOriginalPos = headTransform.localPosition;
        headOriginalRot = headTransform.localRotation;
        neckOriginalRot = neckTransform.localRotation;
    }
    #endregion
    #region Public Methods
    public void Bounce(Vector3 hitDirection)
    {
        Vector3 localDir = headTransform.InverseTransformDirection(hitDirection.normalized);
        Vector3 bounceTarget = headOriginalPos + localDir * bounceDistance;

        Sequence posSequence = DOTween.Sequence();
        posSequence.Append(headTransform.DOLocalMove(bounceTarget, bounceDuration).SetEase(Ease.OutQuad));
        posSequence.Append(headTransform.DOLocalMove(headOriginalPos, bounceDuration).SetEase(Ease.InQuad));

        Vector3 axis = Vector3.Cross(localDir, Vector3.up).normalized;

        float neckWobbleAngle = wobbleAngle * 0.5f;
        Quaternion neckWobbleTarget = Quaternion.AngleAxis(neckWobbleAngle, axis) * neckOriginalRot;

        Sequence neckRotSequence = DOTween.Sequence();
        neckRotSequence.Append(neckTransform.DOLocalRotateQuaternion(neckWobbleTarget, wobbleDuration * 1.2f).SetEase(Ease.OutSine));
        neckRotSequence.Append(neckTransform.DOLocalRotateQuaternion(neckOriginalRot, wobbleDuration * 1.2f).SetEase(Ease.InSine));
    }
    #endregion
}
