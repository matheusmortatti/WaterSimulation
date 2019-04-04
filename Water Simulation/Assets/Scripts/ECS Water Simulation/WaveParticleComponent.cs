using System;
using Unity.Entities;

namespace WaterSimulation.ECS
{
    [Serializable]
    public struct WaveParticle : IComponentData
    {
        public float radius;
        public float amplitude;
    }

    public class WaterParticleComponent : ComponentDataWrapper<WaveParticle> { }
}
