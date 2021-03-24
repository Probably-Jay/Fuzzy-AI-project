using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that inherits from <see cref="KartGame.KartSystems.BaseInput"/>, used by Unity's kart to drive the kart by overriding <see cref="KartGame.KartSystems.BaseInput.GenerateInput"/>
/// </summary>
public class KartControls : KartGame.KartSystems.BaseInput
{
    /// <summary>
    /// The name of the horizontal axis used in <see cref="KartControls.ControlMode.Keyboard"/> control mode
    /// </summary>
    public string Horizontal = "Horizontal";
    /// <summary>
    /// The name of the vertical axis used in <see cref="KartControls.ControlMode.Keyboard"/> control mode
    /// </summary>
    public string Vertical = "Vertical";

    /// <summary>
    /// The fuzzy system input generator
    /// </summary>
    [SerializeField] FuzzyCartController fuzzyCartController;
    /// <summary>
    /// The rules based system input generator
    /// </summary>
    [SerializeField] RBSKartController RBSCartController;

    /// <summary>
    /// The mode the <see cref="KartControls"/> is currently using to control the kart
    /// </summary>
    public enum ControlMode
    {
        Keyboard /// input driven by human user with keyboard
        , Fuzzy /// input driven by the <see cref="FuzzyLogic"/> system
        , RuleBasedSystem /// input driven by the <see cref="RBSCartController"/> rules based system
    }

    /// <summary>
    /// The current <see cref="KartControls.ControlMode"/> of the kart
    /// </summary>
    public ControlMode controlMode;

    /// <summary>
    /// Generates the input for the kart to use.
    /// </summary>
    /// <returns>A <see cref="UnityEngine.Vector2"/> in the form <c>Vector2(Left/Right,Forward/Backwards)</c></returns>
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
            case ControlMode.RuleBasedSystem:
                return GetRBSInput();
            default:
                return Vector2.zero;
        }


    }

    private Vector2 GetRBSInput()
    {
        var (turnInput, driveInput) = RBSCartController.GetInstructions();

        return new Vector2(turnInput, driveInput);
    }

    private Vector2 GetFuzzyInput()
    {
        var (driveInput, turnInput) = fuzzyCartController.GetInstructions();

        return new Vector2(turnInput,driveInput);

    }
}