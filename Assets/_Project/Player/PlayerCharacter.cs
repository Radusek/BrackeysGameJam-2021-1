using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    public static PlayerCharacter Instance { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void Update()
    {
        if (GamePause.Instance.IsPaused)
            return;

        CharacterMovement.WalkInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            CharacterMovement.Jump(true);

    }

    private void OnDestroy() => Instance = null;
}
