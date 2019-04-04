using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveParticleComponent : MonoBehaviour
{
    public float radius;
    public float velocity;
    public float amplitude;
    public float horizontalDeviationAmplitude;
    [HideInInspector]
    public float amplitudeDecay;
    public float amplitudeThreshold;
    public float lifetime;
    public Vector3 direction;
    [Space]
    public float topBound;
    public float bottomBound;
    public float leftBound;
    public float rightBound;
}
