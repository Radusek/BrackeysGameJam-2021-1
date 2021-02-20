using UnityEngine;

public class PlatformChecker : MonoBehaviour
{
    [SerializeField] private LayerMask checkLayer;
    [Space]
    [SerializeField] private Vector2 edgeCheckBoxSize;
    [SerializeField] private Vector2 edgeCheckBoxOffset;
    [Space]
    [SerializeField] private Vector2 wallCheckBoxSize;
    [SerializeField] private Vector2 wallCheckBoxOffset;


    public bool IsOnLeftEdge() => !Check(edgeCheckBoxOffset, false);
    public bool IsOnRightEdge() => !Check(edgeCheckBoxOffset, true);

    public bool IsAtLeftWall() => Check(wallCheckBoxOffset, false);
    public bool IsAtRightWall() => Check(wallCheckBoxOffset, true);


    private bool Check(Vector2 offset, bool rightSide)
    {
        return Physics2D.OverlapBox(transform.position + GetOffset(offset, rightSide), edgeCheckBoxSize, 0f, checkLayer);
    }

    private void OnDrawGizmosSelected()
    {
        var color = Gizmos.color;
        Gizmos.color = Color.red;
        DrawBoxAt(edgeCheckBoxSize, edgeCheckBoxOffset, false);
        DrawBoxAt(edgeCheckBoxSize, edgeCheckBoxOffset, true);
        Gizmos.color = Color.yellow;
        DrawBoxAt(wallCheckBoxSize, wallCheckBoxOffset, false);
        DrawBoxAt(wallCheckBoxSize, wallCheckBoxOffset, true);
        Gizmos.color = color;
    }

    private void DrawBoxAt(Vector2 size, Vector2 offset, bool rightSide)
    {
        Gizmos.DrawCube(transform.position + GetOffset(offset, rightSide), size);
    }

    private Vector3 GetOffset(Vector2 originalOffset, bool rightSide)
    {
        return originalOffset - (rightSide ? Vector2.zero : 2f * originalOffset.x * Vector2.right);
    }
}
