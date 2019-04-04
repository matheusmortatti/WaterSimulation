using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace WaterSimulation.ECS
{
    public class MovementSystem : JobComponentSystem
    {
        [BurstCompile]
        struct MovementJob : IJobProcessComponentData<Position, Direction, MoveSpeed>
        {
            public float topBound;
            public float bottomBound;
            public float rightBound;
            public float leftBound;
            public float deltaTime;

            public void Execute(ref Position position, ref Direction direction, [ReadOnly] ref MoveSpeed moveSpeed)
            {
                float3 value = position.Value;

                value += deltaTime * moveSpeed.Value * direction.Value;


                // Reflect direction when it touches the edges
                if (value.z < bottomBound)
                {
                    direction.Value = math.reflect(direction.Value, new float3(0, 0, 1));
                }
                else if(value.z > topBound)
                {
                    direction.Value = math.reflect(direction.Value, new float3(0, 0, -1));
                }
                else if(value.x < leftBound)
                {
                    direction.Value = math.reflect(direction.Value, new float3(1, 0, 0));
                }
                else if(value.x > rightBound)
                {
                    direction.Value = math.reflect(direction.Value, new float3(-1, 0, 0));
                }

                position.Value = value;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            MovementJob moveJob = new MovementJob
            {
                topBound = WaterParticleManager.instance.topBound,
                bottomBound = WaterParticleManager.instance.bottomBound,
                rightBound = WaterParticleManager.instance.rightBound,
                leftBound = WaterParticleManager.instance.leftBound,
                deltaTime = Time.deltaTime
            };

            JobHandle moveHandle = moveJob.Schedule<MovementJob>(this, inputDeps);

            return moveHandle;
        }
    }
}
