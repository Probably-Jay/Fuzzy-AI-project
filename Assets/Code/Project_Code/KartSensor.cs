using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sensor system so the kart can know its state in the environment
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class KartSensor : MonoBehaviour
{
    /// <summary>
    /// Kart's <see cref="UnityEngine.Rigidbody"/> used to control physics
    /// </summary>
    public Rigidbody rb { get; private set; }

    /// <summary>
    /// The maximum speed of the cart
    /// </summary>
    public const float highSpeed = 10;

    /// <summary>
    /// Forward vector of the car
    /// </summary>
    public Vector3 KartForward => transform.forward;
    /// <summary>
    /// Right vector of the car
    /// </summary>
    public Vector3 KartRight => transform.right;
    /// <summary>
    /// Left vector of the car
    /// </summary>
    public Vector3 KartLeft => -transform.right;

    /// <summary>
    /// The current speed of the car
    /// </summary>
    public float Speed => rb.velocity.magnitude;

    /// <summary>
    /// The distance to the wall hit by the forward ray. Returns <c>null</c> if nothing is hit
    /// </summary>
    public float? ForwardRayHitDistance => RayHitDistance(ForwardRay, forwardRayLength);
    /// <summary>
    /// The surface normal of the wall hit by the forward ray. Returns <c>null</c> if nothing was hit
    /// </summary>
    public Vector3? ForwardRayCollisionNormal => RayHitSurfaceNormal(ForwardRay, forwardRayLength);

    /// <summary>
    /// The distance to the wall hit by the right ray. Returns <c>null</c> if nothing is hit
    /// </summary>
    public float? RightRayHitDistance => RayHitDistance(RightRay, rightRayLength);
    /// <summary>
    /// The surface normal of the wall hit by the right ray. Returns <c>null</c> if nothing was hit
    /// </summary>
    public Vector3? RightRayCollisionNormal => RayHitSurfaceNormal(RightRay, rightRayLength);

    /// <summary>
    /// The distance to the wall hit by the left ray. Returns <c>null</c> if nothing is hit
    /// </summary>
    public float? LeftRayHitDistance => RayHitDistance(LeftRay, leftRayLength);
    /// <summary>
    /// The surface normal of the wall hit by the left ray. Returns <c>null</c> if nothing was hit
    /// </summary>
    public Vector3? LeftRayCollisionNormal => RayHitSurfaceNormal(LeftRay, leftRayLength);





    private static Vector3? RayHitSurfaceNormal(Ray ray, float rayLength)
    {
        RaycastHit? hit = Raycast(ray, rayLength);
        if (hit.HasValue)
        {
            return hit.Value.normal;
        }
        else
        {
            return null;
        }
    }

    private static float? RayHitDistance(Ray ray, float rayLength)
    {
        RaycastHit? hit = Raycast(ray, rayLength);

        if (hit.HasValue)
        {
            if (hit.Value.transform.gameObject.CompareTag("Track"))
            {
                return hit.Value.distance;
            }
        }

        return null;

    }


    private static RaycastHit? Raycast(Ray ray, float rayLength)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, rayLength))
        {
            return raycastHit;
        }
        else
        {
            return null;
        }
    }



    private Ray ForwardRay => new Ray(transform.position, KartForward);
    private float forwardRayLength = 10;

    private Ray RightRay => new Ray(transform.position, KartRight);
    private float rightRayLength = 5;

    private Ray LeftRay => new Ray(transform.position, KartLeft);
    private float leftRayLength = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(ForwardRay.origin, ForwardRay.origin + ForwardRay.direction * forwardRayLength);
        Gizmos.DrawLine(RightRay.origin, RightRay.origin + RightRay.direction * rightRayLength);
        Gizmos.DrawLine(LeftRay.origin, LeftRay.origin + LeftRay.direction * leftRayLength);
    }
}
