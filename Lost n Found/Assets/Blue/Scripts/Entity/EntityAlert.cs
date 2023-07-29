using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAlert
{
    public Vector3 Position {get; private set;}
    public float Strength { get; private set; } = 1;
    public float AccuracyMultiplier { get; private set; } = 1;
    public float AlertTime { get; private set; } = 1;
    public AnimationCurve AlertImpact { get; private set; } = new AnimationCurve();

    public EntityAlert (Vector3 position, float strength, float accuracyMultiplier, float AlertTime, AnimationCurve alertImpact)
    {
        Position = position;
        Strength = strength;
        AccuracyMultiplier = accuracyMultiplier;
    }
}
