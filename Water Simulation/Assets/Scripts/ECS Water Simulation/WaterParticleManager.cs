using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace WaterSimulation.ECS
{
    public class WaterParticleManager : MonoBehaviour
    {
        public static WaterParticleManager instance = null;

        public GameObject waterParticlePrefab;
        public int amountToSpawn = 100;
        public float maxRadius = 2;
        public float moveSpeed = 0.1f;
        [Space]
        public float topBound, bottomBound, rightBound, leftBound;

        EntityManager manager;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                topBound = this.transform.position.z + topBound;
                bottomBound = this.transform.position.z + bottomBound;
                rightBound = this.transform.position.x + rightBound;
                leftBound = this.transform.position.x + leftBound;
            }
            else if (instance != this)
                Destroy(this.gameObject);

            
        }

        // Start is called before the first frame update
        void Start()
        {
            manager = World.Active.GetOrCreateManager<EntityManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddWaterParticles(amountToSpawn);
            }
        }

        void AddWaterParticles(int amount)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(amount, Allocator.Temp);
            manager.Instantiate(waterParticlePrefab, entities);

            for (int i = 0; i < amount; i++)
            {
                Vector3 direction = Random.insideUnitSphere;
                direction.y = 0;
                Vector3 position = direction * maxRadius + this.transform.position;
                direction = direction.normalized;

                manager.SetComponentData(entities[i], new Position { Value = new Unity.Mathematics.float3(position.x, position.y, position.z) });
                manager.SetComponentData(entities[i], new Direction { Value = new Unity.Mathematics.float3(direction.x, direction.y, direction.z) });
                manager.SetComponentData(entities[i], new MoveSpeed { Value = moveSpeed });
            }

            entities.Dispose();
        }
    }
}
