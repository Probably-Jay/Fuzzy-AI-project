using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartControls : KartGame.KartSystems.BaseInput
{
    public string Horizontal = "Horizontal";
    public string Vertical = "Vertical";

    [SerializeField] FuzzyCartController fuzzyCartController;

    public enum ControlMode
    {
        Keyboard
            , Fuzzy
    }

    public ControlMode controlMode;

    public override Vector2 GenerateInput()
    {

        switch (controlMode)
        {
            case ControlMode.Keyboard:
                return new Vector2
                {
                    x = Input.GetAxis(Horizontal),
                    y = Input.GetAxis(Vertical)
                };
            case ControlMode.Fuzzy:
                return GetFuzzyInput();
            default:
                return Vector2.zero;
        }


    }

    private Vector2 GetFuzzyInput()
    {
        var (driveInput, turnInput) = fuzzyCartController.GetInstructions();

        return new Vector2(turnInput,driveInput);

    }
}