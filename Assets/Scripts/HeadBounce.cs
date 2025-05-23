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
    #endregion
    #region Unity's Methods
    private void Awake()
    {
        neckOriginalRot = neckTransform.localRotation;
    }
    #endregion
    #region Public Methods
    public void Bounce(Vector3 hitDirection)
    {
        Vector3 localDir = headTransform.InverseTransformDirection(hitDirection.normalized);

        Vector3 axis = Vector3.Cross(localDir, Vector3.up).normalized * -1;

        float neckWobbleAngle = wobbleAngle * 0.5f;
        Quaternion neckWobbleTarget = Quaternion.AngleAxis(neckWobbleAngle, axis) * neckOriginalRot;

        Sequence neckRotSequence = DOTween.Sequence();
        neckRotSequence.Append(neckTransform.DOLocalRotateQuaternion(neckWobbleTarget, wobbleDuration * 1.2f).SetEase(Ease.OutSine));
        neckRotSequence.Append(neckTransform.DOLocalRotateQuaternion(neckOriginalRot, wobbleDuration * 1.2f).SetEase(Ease.InSine));
    }
    #endregion
}
