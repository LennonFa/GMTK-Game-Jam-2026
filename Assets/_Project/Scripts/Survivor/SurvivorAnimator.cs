using System;
using Unity.VisualScripting;
using UnityEngine;

public class SurvivorAnimator : MonoBehaviour
{
    private static readonly int IsTreading = Animator.StringToHash("isTreadingWater");
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int IsSaved = Animator.StringToHash("isSaved");
    private static readonly int IsGrabbed = Animator.StringToHash("isGrabbed");
    private static readonly int IsWobbly = Animator.StringToHash("isWobbly");
    [SerializeField] private Animator animator;
    [SerializeField] private Survivor survivor;
    [SerializeField] private SurvivorOxygen survivorOxygen;
    [SerializeField] private float wobbleThreshAngle = 10;
    [SerializeField] private float stopWobbleSeconds = 1f;
    private float currentStopWobbleTime;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        if (!survivor && !TryGetComponent(out survivor))
            throw new MissingComponentException("no survivor");
        if (!survivorOxygen && !TryGetComponent(out survivorOxygen))
            throw new MissingComponentException("no survivorOxygen");
    }

    private void Update()
    {
        animator.SetBool(IsTreading, survivorOxygen.submerged);
        animator.SetBool(IsDead, survivorOxygen.Drowned);
        animator.SetBool(IsSaved, survivor.isRescued);
        animator.SetBool(IsGrabbed, survivor.isHeld);

        var currentStandingAngle = Quaternion.Angle(Quaternion.identity, survivor.transform.rotation);

        if (currentStandingAngle >= wobbleThreshAngle)
        {
            animator.SetBool(IsWobbly, true);
            currentStopWobbleTime = 0;
        }
        else if (currentStandingAngle < wobbleThreshAngle)
        {
            currentStopWobbleTime += Time.deltaTime;
            if (currentStopWobbleTime > stopWobbleSeconds)
            {
                animator.SetBool(IsWobbly, false);
            }
        }

    }
}
