using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Guns/Trail Config", order = 4)]
public class TrailConfigurationSO : ScriptableObject
{
    public Material Material;
    public AnimationCurve WidthCurve;
    public float Duration;
    public float MinVertextDistance = 0.1f;
    public Gradient Color;
    public float Time = 0.5f;
    public int CornerVertices;
    public int EndCapVertices;


    public float MissDitance = 100f;
    public float SimulationSpeed = 100f;

    public void SetupTrail(TrailRenderer TrailRenderer)
    {
        TrailRenderer.widthCurve = WidthCurve;
        TrailRenderer.time = Time;
        TrailRenderer.minVertexDistance = MinVertextDistance;
        TrailRenderer.colorGradient = Color;
        TrailRenderer.sharedMaterial = Material;
        TrailRenderer.numCornerVertices = CornerVertices;
        TrailRenderer.numCapVertices = EndCapVertices;
    }


}

