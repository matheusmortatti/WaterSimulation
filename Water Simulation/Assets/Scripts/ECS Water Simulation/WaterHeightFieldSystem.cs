using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace WaterSimulation.ECS
{
    public class WaterHeightFieldSystem : JobComponentSystem
    {
        [BurstCompile]
        struct MovementJob : IJobProcessComponentData<Position, WaveParticle>
        {

            public void Execute([ReadOnly] ref Position position, [ReadOnly] ref WaveParticle waveParticle)
            {
                
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob moveJob = new MovementJob
            {
            };

            JobHandle moveHandle = moveJob.Schedule<MovementJob>(this, inputDeps);

            return moveHandle;
        }
    }
}
