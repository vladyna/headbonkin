using UnityEngine;

public class HeadHitDetector : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private HeadBounce headBounce;
    #endregion
    #region Public Methods
    public void OnHit(Vector3 force, Vector3 hitPoint)
    {
        headBounce.Bounce(force);
    }
    #endregion
}