using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class EntityMovement : NetworkBehaviour
{
    public Transform destination;
    [SerializeField] private Animator animator;
    NavMeshAgent agent;
    int variationKey;
    int lookRightKey;
    int lookLeftKey;
    int walkKey;
    int sprintKey;
    int lookAroundKey;
    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();

        variationKey = Animator.StringToHash("Variation");
        lookRightKey = Animator.StringToHash("Look Right");
        lookLeftKey = Animator.StringToHash("Look Left");
        walkKey = Animator.StringToHash("Walk");
        sprintKey = Animator.StringToHash("Sprint");
        lookAroundKey = Animator.StringToHash("Look Around");
    }
    private void Update()
    {
        agent.destination = destination.position;
        Animate();
    }

    private void Animate()
    {
        animator.ResetTrigger(lookRightKey);
        animator.ResetTrigger(lookLeftKey);
        animator.ResetTrigger(walkKey);
        animator.ResetTrigger(sprintKey);
        animator.ResetTrigger(lookAroundKey);

        animator.SetFloat(variationKey, (Mathf.Sin(Time.time) * 0.5f) + 0.5f);

        animator.speed = 1;

        float velocity = agent.velocity.magnitude;

        if (velocity > 0)
        {
            animator.SetTrigger(sprintKey);

            animator.speed = velocity / agent.speed;
        }
        else
        {
            animator.SetTrigger(lookAroundKey);
        }
    }
}
