using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class HeadHitDetector : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private HeadBounce headBounce;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private ParticleSystem particlesPrefab;
    [SerializeField] private DisappearingMessageUI disappearingMessageUI;
    #endregion

    #region Private Fields
    private ObjectPool<AudioSource> audioPool;
    private ObjectPool<ParticleSystem> particlesPool;
    #endregion
    #region Unity's Methods
    private void Awake()
    {
        audioPool = new ObjectPool<AudioSource>(audioSourcePrefab, 5);
        particlesPool = new ObjectPool<ParticleSystem>(particlesPrefab, 5);
    }
    #endregion

    #region Unity's Methods
    private void OnTriggerEnter(Collider collision)
    {
        Vector3 collisionPosition = collision.transform.position;
        Vector3 hitDirection = (transform.position - collisionPosition).normalized;

        headBounce.Bounce(hitDirection);
        disappearingMessageUI.ShowText("*PUNCH*", collisionPosition);
        PlayAudio(collisionPosition);
        PlayShortPunchEffect(collisionPosition, hitDirection);
    }
    void PlayAudio(Vector3 position)
    {
        var audio = audioPool.GetFromPool();
        audio.transform.position = position;
        audio.Play();
        StartCoroutine(StopAudioAfterDelay(audio, 0.5f));
    }

    private IEnumerator StopAudioAfterDelay(AudioSource audio, float delay)
    {
        yield return new WaitForSeconds(delay);
        audio.Stop();
        audioPool.ReturnToPool(audio);
    }
    void PlayShortPunchEffect(Vector3 position, Vector3 direction)
    {
        var particles = particlesPool.GetFromPool();
        particles.transform.position = position;
        particles.transform.rotation = Quaternion.LookRotation(direction);
        particles.Play();
        StartCoroutine(StopPunchEffectAfterDelay(particles, 0.1f));
    }

    private IEnumerator StopPunchEffectAfterDelay(ParticleSystem particles, float delay)
    {
        yield return new WaitForSeconds(delay);
        particles.Stop();
        particlesPool.ReturnToPool(particles);
    }
    #endregion
}