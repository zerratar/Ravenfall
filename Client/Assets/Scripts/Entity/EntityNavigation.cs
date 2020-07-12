using UnityEngine;
using UnityEngine.AI;

public class EntityNavigation : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float sprintMultiplier = 2.5f;
    [SerializeField] private float positionUpdateThreshold = 1f;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    private bool isSprinting = false;
    private float oldMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (!navMeshAgent) navMeshAgent = GetComponent<NavMeshAgent>();
        if (!animator) animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    internal void MoveTo(Vector3 destination, bool sprinting)
    {
        MoveTo(transform.position, destination, sprinting);
    }

    internal void MoveTo(Vector3 currentPosition, Vector3 destination, bool sprinting)
    {
        SetMovement(sprinting);
        // to ensure we get a smooth transition and don't jump too much. We wont 
        // care about small position changes and instead just update the destination.
        if (Vector3.Distance(transform.position, currentPosition) > positionUpdateThreshold)
        {
            navMeshAgent.Warp(currentPosition);
        }
        navMeshAgent.SetDestination(destination);
    }

    internal void SetMovement(bool sprinting)
    {
        navMeshAgent.speed = movementSpeed * (sprinting ? sprintMultiplier : 1f);
        isSprinting = sprinting;
    }

    private void UpdateAnimator()
    {
        if (animator)
        {
            var speed =
                !navMeshAgent.isOnNavMesh ||
                navMeshAgent.isStopped ||
                navMeshAgent.velocity == Vector3.zero ||
                navMeshAgent.remainingDistance <= 0.21f
                ? 0f : isSprinting ? 1f : 0.5f;

            if (oldMovementSpeed != speed)
            {
                animator.SetFloat("MovementSpeed", speed);
                oldMovementSpeed = speed;
            }
        }
    }

    internal void StopMoving()
    {
        MoveTo(transform.position, transform.position, false);
    }
}
