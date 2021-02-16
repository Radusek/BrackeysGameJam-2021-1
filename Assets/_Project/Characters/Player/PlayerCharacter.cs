using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    private void Update()
    {
        CharacterMovement.WalkInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            CharacterMovement.Jump();

    }
}
