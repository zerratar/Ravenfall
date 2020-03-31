using System;
using UnityEngine;
using UnityEngine.AI;

public class EntityNavigation : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float sprintMultiplier = 2.5f;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    private bool isSprinting = false;
    private float oldMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (!navMeshAgent) navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateAnimator();
    }

    internal void MoveTo(Vector3 point, bool sprinting)
    {
        SetMovement(sprinting);
        navMeshAgent.SetDestination(point);
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
        MoveTo(transform.position, false);
    }
}
