using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInfo : MonoBehaviour
{
    private CharacterMovement movement;

    public bool IsGoingRight { get; set; }


    private void Awake() => movement = GetComponent<CharacterMovement>();

    public void Move(float input)
    {
        movement.WalkInput = input;
    }
}
