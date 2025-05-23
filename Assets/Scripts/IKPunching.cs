using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKPunching : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private bool enableIK = false;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform leftHandTarget;
    [Range(0f, 1f)]
    [SerializeField] private float ikWeight = 1f;
    #endregion
    #region Private Fields
    private Animator animator;
    #endregion
    #region Unity Methods
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (animator == null || !enableIK) return;

        if (rightHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
        }

        if (leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
    #endregion
}
