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
    public EntityAnimationState EntityAnimationState {get; private set;}
    private EntityAnimationState oldEntityAnimationState;
    [SerializeField] private float stoppingVelocity = 0.1f;
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
    // Update is called once per frame

    private void Start() {    
    }
    void Update()
    {
        agent.destination = destination.position;
        Animate();
    }

    private void Animate()
    {
        animator.SetFloat(variationKey, (Mathf.Sin(Time.time) * 0.5f) + 0.5f);

        if (agent.velocity.sqrMagnitude > stoppingVelocity)
        {
            EntityAnimationState = EntityAnimationState.Sprinting;
        }
        else
        {
            EntityAnimationState = EntityAnimationState.Idle;
        }

        if (EntityAnimationState != oldEntityAnimationState)
        {
            switch (EntityAnimationState)
            {
                case EntityAnimationState.Sprinting:
                    animator.SetTrigger(sprintKey);
                    break;
                case EntityAnimationState.Idle:
                    animator.SetTrigger(lookAroundKey);
                    break;
            }

        }

        oldEntityAnimationState = EntityAnimationState;
    }
}
