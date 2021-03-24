using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for the kart using a Rules Based System
/// </summary>
[RequireComponent(typeof(KartSensor))]
public class RBSKartController : MonoBehaviour
{

    private const float forwardMinAngle = -90;
    private const int forwardMaxAngle = 90;

    /// <summary>
    /// The sensor that provides this class with data about the kart
    /// </summary>
    KartSensor sensor;


    [SerializeField] bool debug = false;

    [SerializeField] float hightSpeed = 10;
    [SerializeField] float neutralSpeed = 0;

    [SerializeField] float largeForwardDistance = 10;
    [SerializeField] float neutralForwardDistance = 0;

    [SerializeField] float largeRightDistance = 10;
    [SerializeField] float neutralRightDistance = 0;

    [SerializeField] float largeLeftDistance = 10;
    [SerializeField] float neutralLeftDistance = 0;

    /// <summary>
    /// The inputs from <see cref="KartSensor"/>
    /// </summary>
    public enum Inputs
    {
        Speed
         , ForwardDistance
         , RightDistance
         , LeftDistance
         , ForwardSurfaceNormal
    }

    private void Awake()
    {
        sensor = GetComponent<KartSensor>();
    }


    /// <summary>
    /// Returns <c>(float, float)</c> tuple representing this class' confidence that actions (drive, turn) should be taken, and the polarity of those actions. 
    /// </summary>
    /// <returns>A <c>(float, float)</c> tuple representing the actions (drive, turn)</returns>
    public (float, float) GetInstructions()
    {
        var input = GenerateInputFromSensors();
        var output = EvaluateRules(input);


        if (!Input.GetKey(KeyCode.Space))
        {
            
            DebugOutLight(output);

        }


        return output;

    }

    private void DebugOutLight((float, float) output)
    {

        Debug.Log($"Output: Forward -> {output.Item2}");
        Debug.Log($"Output: LeftRight -> {output.Item1}");
     

    }

    private (float, float) EvaluateRules(Dictionary<Inputs, float> input) => (EvaluateLeftRight(input), EvauateForward(input));

    private float EvaluateLeftRight(Dictionary<Inputs, float> input)
    {
        float forwardDistance = input[Inputs.ForwardDistance];
        float surfaceNormal = input[Inputs.ForwardSurfaceNormal];
        float rightDistance = input[Inputs.RightDistance];
        float leftDistance = input[Inputs.LeftDistance];

        var output = (value: 0f, sum: 0f);


        if (forwardDistance >= 5 && surfaceNormal < 0)
        {
            output.value += -1f; output.sum += 1;
        } 
        
        if (forwardDistance >= 0 && forwardDistance < 2.5f && surfaceNormal < 0)
        {
            output.value += -1f; output.sum += 1;
        }  
        
        
        if (forwardDistance >= 5 && surfaceNormal > 0)
        {
            output.value += 1f; output.sum += 1;
        } 
        
        if (forwardDistance >= 0 && forwardDistance < 2.5f && surfaceNormal > 0)
        {
            output.value += 1f; output.sum += 1;
        }


        if ((rightDistance < 1.25f && rightDistance > -0.1f) || leftDistance < -0.9f)
        {
            output.value += -1f; output.sum += 1;
        }

        if ((leftDistance < 1.25f && leftDistance > -0.1f) || rightDistance < -0.9f)
        {
            output.value += +1f; output.sum += 1;
        }


        return output.value / (output.sum != 0? output.sum : 1);


    }


    private float EvauateForward(Dictionary<Inputs, float> input) => 1; // always drive

    private Dictionary<Inputs,float> GenerateInputFromSensors()
    {

        float speed = sensor.Speed;

        float forwardDistance = sensor.ForwardRayHitDistance ?? -1;

        float rightDistance = sensor.RightRayHitDistance ?? -1;

        float leftDistance = sensor.LeftRayHitDistance ?? -1;

        Vector3? forwardSucfaceNormal = sensor.ForwardRayCollisionNormal;

        float surfaceAngle = SurfaceAngle(forwardSucfaceNormal, forwardMinAngle, forwardMaxAngle);

        Dictionary<Inputs, float> input = new Dictionary<Inputs, float>();


        input[Inputs.Speed] = speed;
        input[Inputs.ForwardDistance] = forwardDistance;
        input[Inputs.RightDistance] = rightDistance;
        input[Inputs.LeftDistance] = leftDistance;
        input[Inputs.ForwardSurfaceNormal] = surfaceAngle;
        return input;
    }

    private float SurfaceAngle(Vector3? surfaceNormalInpt, float minAngle, float maxAngle)
    {
        float surfaceAngle;
        if (surfaceNormalInpt.HasValue)
        {
            Vector2 ourNormal = -(new Vector2(sensor.KartForward.x, sensor.KartForward.z));
            Vector2 surfaceNormal = new Vector2(surfaceNormalInpt.Value.x, surfaceNormalInpt.Value.z);

            var angle = Vector2.SignedAngle(surfaceNormal, ourNormal); // positive is to the right, negative is to the left
            //var angleMag = Mathf.Abs(angle);

            angle *= -1; // reflect angle difference round, positive right negative left from cart's perspective now

            surfaceAngle = angle;
        }
        else
        {
            surfaceAngle = 0; // if no collision is detected return 0 as sentinel value. not ideal but safest option
        }

        return surfaceAngle;
    }



}
