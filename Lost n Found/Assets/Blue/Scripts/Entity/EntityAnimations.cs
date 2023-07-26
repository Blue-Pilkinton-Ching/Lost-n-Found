using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EntityAnimations : MonoBehaviour
{
    Animator animator;
    int intentionKey;
    int lookRightKey;
    int lookLeftKey;
    int walkKey;
    int sprintKey;
    int lookAroundKey;
    private void Awake() {
        animator = GetComponent<Animator>();

        intentionKey = Animator.StringToHash("Intention");
        lookRightKey = Animator.StringToHash("Look Right");
        lookLeftKey = Animator.StringToHash("Look Left");
        walkKey = Animator.StringToHash("Walk");
        sprintKey = Animator.StringToHash("Sprint");
        lookAroundKey = Animator.StringToHash("Look Around");
    }
}
