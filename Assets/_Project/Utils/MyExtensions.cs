using UnityEngine;

public static class MyExtensions
{
    public static void AddForceTimesDrag(this Rigidbody2D rb, Vector2 force, ForceMode2D mode = ForceMode2D.Force) => rb.AddForce(force * rb.drag, mode);
    public static bool IsFalling(this Rigidbody2D rb) => rb.velocity.y < 0f;

    public static void StopVertically(this Rigidbody2D rb)
    {
        Vector2 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;
    }

    public static Vector3 ResetZ(this Vector3 vector)
    {
        vector.z = 0f;
        return vector;
    }

    public static int GetOnlyLayers(params string[] names)
    {
        int layerMask = 0;
        foreach (var name in names)
            layerMask |= 1 << LayerMask.NameToLayer(name);

        return layerMask;
    }

    public static int GetLayersExcept(params string[] names) => ~GetOnlyLayers(names);

    public static Vector2 ToVector2(this float radians) => new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

    public static bool IsInLayer(this int myLayer, int layerMask) => layerMask == (layerMask | (1 << myLayer));
    public static bool IsInLayer(this int myLayer, LayerMask layerMask) => layerMask.value == (layerMask.value | (1 << myLayer));

    public static int Modulo(int a, int b) => (a % b + b) % b;

    public static int ExcludeMyLayerFrom(this GameObject go, int layerMask) => layerMask & ~(1 << go.layer);
}
