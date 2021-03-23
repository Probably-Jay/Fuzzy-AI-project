﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuzzyLogic;

[RequireComponent(typeof(KartSensor))]
public class FuzzyCartController : MonoBehaviour
{
    private const float forwardMinAngle = -90;
    private const int forwardMaxAngle = 90;
    KartSensor sensor;

    [SerializeField] float hightSpeed = 10;
    [SerializeField] float lowSpeed = 0;

    [SerializeField] float largeForwardDistance = 10;
    [SerializeField] float neutralForwardDistance = 0;   
    
    [SerializeField] float largeRightDistance = 10;
    [SerializeField] float neutralRightDistance = 0;    
    
    [SerializeField] float largeLeftDistance = 10;
    [SerializeField] float neutralLeftDistance = 0;

    FuzzySystem fuzzySystem = new FuzzySystem();

    private void Awake()
    {
        sensor = GetComponent<KartSensor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        CrispInput input = GenerateInputFromSensors();

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

    private float NormaliseSurfaceAngle(Vector3? surfaveNormal, float minAngle, float maxAngle)
    {
        float normalisedForwardSurfaceNormal;
        if (surfaveNormal.HasValue)
        {
            Vector2 ourNormal = -sensor.KartForward;
            Vector2 surfaceNormal = surfaveNormal.Value;

            var angle = Vector2.SignedAngle(ourNormal, surfaceNormal); // positive is to the right, negative is to the left
            var angleMag = Mathf.Abs(angle);

            angle *= -1; // reflect angle difference round, positive right negative left from cart's perspective now

            normalisedForwardSurfaceNormal = FuzzyUtility.NormaliseValue(minAngle, maxAngle, angle);
        }
        else
        {
            normalisedForwardSurfaceNormal = 0; // if no collision is detected return 0 as sentinel value. not ideal but safest option
        }

        return normalisedForwardSurfaceNormal;
    }

    private float NormaliseSpeed(float speed) => FuzzyUtility.NormaliseValue(lowSpeed, hightSpeed, speed);

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
