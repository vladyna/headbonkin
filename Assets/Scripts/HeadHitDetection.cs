using UnityEngine;

public class HeadHitDetector : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private HeadBounce headBounce;
    [SerializeField] private AudioSource audioSource;
    #endregion

    #region Public Methods
    public void OnHit(Vector3 force, Vector3 hitPoint)
    {
        headBounce.Bounce(force);
    }
    #endregion

    #region Unity's Methods
    private void OnTriggerEnter(Collider collision)
    {
        Vector3 hitDirection = (transform.position - collision.transform.position).normalized;

        headBounce.Bounce(hitDirection);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.Play();
    }
    #endregion
}