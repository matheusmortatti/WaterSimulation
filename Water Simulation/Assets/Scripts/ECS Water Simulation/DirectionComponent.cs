using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace WaterSimulation.ECS
{
    [Serializable]
    public struct Direction : IComponentData
    {
        public float3 Value;
    }

    public class DirectionComponent : ComponentDataWrapper<Direction> { }
}
