using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartSensor : MonoBehaviour
{
    public Rigidbody rb { get; private set; }

    public const float highSpeed = 10;

    public Vector3 KartForward => transform.forward;
    public Vector3 KartRight => transform.right;
    public Vector3 KartLeft => -transform.right;


    public float Speed => rb.velocity.magnitude;


    public float? ForwardRayHitDistance => RayHitDistance(ForwardRay, forwardRayLength);
    public Vector3? ForwardRayCollisionNormal => RayHitSurfaceNormal(ForwardRay, forwardRayLength);

    public float? RightRayHitDistance => RayHitDistance(RightRay, rightRayLength);
    public Vector3? RightRayCollisionNormal => RayHitSurfaceNormal(RightRay, rightRayLength);

    public float? LeftRayHitDistance => RayHitDistance(LeftRay, leftRayLength);
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(ForwardRay.origin, ForwardRay.origin + ForwardRay.direction * forwardRayLength);
        Gizmos.DrawLine(RightRay.origin, RightRay.origin + RightRay.direction * rightRayLength);
        Gizmos.DrawLine(LeftRay.origin, LeftRay.origin + LeftRay.direction * leftRayLength);
    }
}
