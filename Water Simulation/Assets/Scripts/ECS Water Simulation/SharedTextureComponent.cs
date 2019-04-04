using System;
using Unity.Entities;

namespace WaterSimulation.ECS
{
    [Serializable]
    public struct SharedTexture : ISharedComponentData
    {
        public float radius;
        public float amplitude;
    }

    public class SharedTextureComponent : SharedComponentDataWrapper<SharedTexture> { }
}
