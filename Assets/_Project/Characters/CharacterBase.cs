using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public CharacterMovement CharacterMovement { get; private set; }


    protected virtual void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
    }
}
