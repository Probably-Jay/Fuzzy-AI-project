using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuzzyLogic;
using System;

[RequireComponent(typeof(KartSensor))]
public class FuzzyCartController : MonoBehaviour
{
    private const float forwardMinAngle = -90;
    private const int forwardMaxAngle = 90;
    KartSensor sensor;

    [SerializeField] FuzzySystem fuzzySystem;


    [SerializeField] bool debug = false;

    [SerializeField] float hightSpeed = 10;
    [SerializeField] float neutralSpeed = 0;

    [SerializeField] float largeForwardDistance = 10;
    [SerializeField] float neutralForwardDistance = 0;   
    
    [SerializeField] float largeRightDistance = 10;
    [SerializeField] float neutralRightDistance = 0;    
    
    [SerializeField] float largeLeftDistance = 10;
    [SerializeField] float neutralLeftDistance = 0;


    private void Awake()
    {
        sensor = GetComponent<KartSensor>();
    }




    /// <summary>
    /// Returns <c>(float, float)</c> tuple representing <see cref="FuzzyCartController.fuzzySystem"/>'s confidence that actions (drive, turn) should be taken, and the polarity of those actions. 
    /// Based off the current values from <see cref="FuzzyCartController.sensor"/>
    /// </summary>
    /// <returns>A <c>(float, float)</c> tuple representing the actions (drive, turn)</returns>
    public (float, float) GetInstructions()
    {
        CrispInput input = GenerateInputFromSensors();
        CrispOutput output = fuzzySystem.EvaluateFuzzyLogic(input);
        

        if (!Input.GetKey(KeyCode.Space))
        {
            DebugOutLight(output);
        }
        else if (debug)
        {
            DebugOutLots(input, output);
        }

        output = ValidateOutput(output);

        return (output[CrispOutput.Outputs.ForwardBackwards], output[CrispOutput.Outputs.LeftRight]);

    }

    private CrispOutput ValidateOutput(CrispOutput output)
    {
        if (!FuzzyUtility.ValidInstruction(output[CrispOutput.Outputs.LeftRight])){
            output[CrispOutput.Outputs.LeftRight] = 0;
        }
        return output;
    }

    private void DebugOutLight(CrispOutput outputVals)
    {
        foreach (var output in CrispOutput.OutputEnumvalues)
        {
            Debug.Log($"Output: {output.ToString()} -> {outputVals[output]}");
        }
    }

    private void DebugOutLots(CrispInput inputVals, CrispOutput outputVals)
    {
        Debug.Log("=====Case=====");
        foreach (var input in CrispInput.InputEnumvalues)
        {
            Debug.Log($"Input: {input.ToString()} -> {inputVals[input]}");
        }
        Debug.Log("=====Ouputs=====");
        foreach (var output in CrispOutput.OutputEnumvalues)
        {
            Debug.Log($"Output: {output.ToString()} -> {outputVals[output]}");
        }
        Debug.Log("=====End-Case=====");
    }

    private CrispInput GenerateInputFromSensors()
    {
        CrispInput input;
        float speed = sensor.Speed;
        float normalisedSpeed = NormaliseSpeed(speed);

        float? forwardDistance = sensor.ForwardRayHitDistance;
        float normalisedForwardDistance = NormaliseDistance(forwardDistance, neutralForwardDistance, largeForwardDistance);

        float? rightDistance = sensor.RightRayHitDistance;
        float normalisedRighDistance = NormaliseDistance(rightDistance, neutralRightDistance, largeRightDistance);

        float? leftDistance = sensor.LeftRayHitDistance;
        float normalisedLeftDistance = NormaliseDistance(leftDistance, neutralLeftDistance, largeLeftDistance);

        Vector3? forwardSucfaceNormal = sensor.ForwardRayCollisionNormal;
        float normalisedForwardSurfaceNormal;

        normalisedForwardSurfaceNormal = NormaliseSurfaceAngle(forwardSucfaceNormal, forwardMinAngle, forwardMaxAngle);


        input = new CrispInput();

        input[CrispInput.Inputs.Speed] = normalisedSpeed;
        input[CrispInput.Inputs.ForwardDistance] = normalisedForwardDistance;
        input[CrispInput.Inputs.RightDistance] = normalisedRighDistance;
        input[CrispInput.Inputs.LeftDistance] = normalisedLeftDistance;
        input[CrispInput.Inputs.ForwardSurfaceNormal] = normalisedForwardSurfaceNormal;
        return input;
    }

    private float NormaliseSurfaceAngle(Vector3? surfaceNormalInpt, float minAngle, float maxAngle)
    {
        float normalisedForwardSurfaceNormal;
        if (surfaceNormalInpt.HasValue)
        {
            Vector2 ourNormal = -(new Vector2(sensor.KartForward.x, sensor.KartForward.z));
            Vector2 surfaceNormal = new Vector2(surfaceNormalInpt.Value.x, surfaceNormalInpt.Value.z);

            var angle = Vector2.SignedAngle(surfaceNormal, ourNormal); // positive is to the right, negative is to the left
            //var angleMag = Mathf.Abs(angle);

            angle *= -1; // reflect angle difference round, positive right negative left from cart's perspective now

            normalisedForwardSurfaceNormal = FuzzyUtility.NormaliseValue(minAngle, maxAngle, angle);
        }
        else
        {
            normalisedForwardSurfaceNormal = 0; // if no collision is detected return 0 as sentinel value. not ideal but safest option
        }

        return normalisedForwardSurfaceNormal;
    }

    private float NormaliseSpeed(float speed) => FuzzyUtility.NormaliseValueUneven(-1, neutralSpeed, hightSpeed, speed);

    private float NormaliseDistance(float? unNormalisedDistance, float neutralDistance, float largeDistance)
    {
        float normalisedForwardDistance;
        if (unNormalisedDistance.HasValue)
        {
            normalisedForwardDistance = FuzzyUtility.NormaliseValueUneven(-1, neutralDistance, largeDistance, unNormalisedDistance.Value);
        }
        else
        {
            normalisedForwardDistance = -1;
        }

        return normalisedForwardDistance;
    }
}
