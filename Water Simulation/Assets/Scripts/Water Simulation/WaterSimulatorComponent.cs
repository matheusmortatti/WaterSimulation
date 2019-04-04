using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class WaterSimulatorComponent : MonoBehaviour
{
    public int xSize, zSize;
    public float scale;

    public MeshGenerator meshGenerator;
    private MeshFilter meshFilter;
    private WaveParticleSpawner waveParticleSpawner;


    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        waveParticleSpawner = FindObjectOfType<WaveParticleSpawner>();

        meshGenerator = new MeshGenerator();
        KeyValuePair<Vector3[], int[]> mesh =  meshGenerator.GenerateMesh(xSize, zSize, scale);
        meshFilter.mesh.vertices = mesh.Key;
        meshFilter.mesh.triangles = mesh.Value;
        meshFilter.mesh.RecalculateNormals();
    }

    void FixedUpdate()
    {
        //GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        Vector3 newPosition = new Vector3(0, 0, 0);
        for (int x = 0; x < xSize + 1; x++)
        {
            for (int z = 0; z < zSize + 1; z++)
            {
                //float terrainHeight = GetWaveHeight(x * scale, z * scale);
                //vertices[x * (waterEntity.waterSimulator.xSize + 1) + z].y = terrainHeight;
                GetWavePosition(x * scale, z * scale, ref newPosition);
                vertices[x * (xSize + 1) + z].x = newPosition.x;
                vertices[x * (xSize + 1) + z].y = newPosition.y;
                vertices[x * (xSize + 1) + z].z = newPosition.z;
            }
        }
        meshFilter.mesh.vertices = vertices;
    }

    float GetWaveHeight(float x, float z)
    {
        float heightDeviation = 0;
        Vector3 samplePos = new Vector3(x, 0, z);
        foreach (var particle in waveParticleSpawner.waveParticles)
        {
            Vector3 particlePos = particle.transform.position;
            float relativeDistance = Vector3.Distance(samplePos, particlePos);
            float radius = particle.radius;

            // Vertical Deviation
            float rectFuncParam = relativeDistance / (2 * radius);
            int rectFunc = rectFuncParam < 1.5f ? 1 : 0;
            if (rectFunc == 0) break;


            float amp = particle.amplitude;
            Vector3 direction = particle.direction.normalized;


            float Di = (amp / 2) * (Mathf.Cos(relativeDistance / radius) + 1);

            // Horizontal Deviation
            Vector3 DiL = -Mathf.Sin(relativeDistance / radius) * rectFunc * direction * Di;


            heightDeviation += Di * rectFunc;
        }


        return heightDeviation;
    }

    // Return 
    void GetWavePosition(float x, float z, ref Vector3 finalPos)
    {
        finalPos.x = x;
        finalPos.y = 0;
        finalPos.z = z;
        Vector3 samplePos = new Vector3(x, 0, z);
        foreach (var particle in waveParticleSpawner.waveParticles)
        {
            float relativeDistance = Vector3.Magnitude(samplePos - particle.transform.position);

            // Vertical Deviation
            float rectFunc = relativeDistance / (2 * particle.radius) < 1.5f ? 1 : 0;

            // Di
            finalPos.y += (particle.amplitude / 2) * (Mathf.Cos(relativeDistance / particle.radius) + 1) * rectFunc;

            // DiL
            finalPos += particle.horizontalDeviationAmplitude * Mathf.Sin(relativeDistance / particle.radius) * rectFunc * particle.direction.normalized;
        }
    }
}

//public class WaterSimulatorSystem : ComponentSystem
//{
//    private struct WaveParticleEntity
//    {
//        public WaveParticleComponent waveParticle;
//        //public Rigidbody rigidbody;
//        public Transform transform;
//    }

//    private struct WaterSimulatorEntity
//    {
//        public WaterSimulatorComponent waterSimulator;
//        public MeshFilter meshFilter;
//    }

//    protected override void OnStartRunning()
//    {
//        base.OnStartRunning();

//        foreach(var entity in GetEntities<WaterSimulatorEntity>())
//        {
//            entity.waterSimulator.meshGenerator = new MeshGenerator(entity.waterSimulator.waterMesh, entity.meshFilter);
//            entity.waterSimulator.meshGenerator.GenerateMesh(entity.waterSimulator.xSize, entity.waterSimulator.zSize, entity.waterSimulator.scale);
//        }
//    }

//    protected override void OnUpdate()
//    {
//        foreach(var waterEntity in GetEntities<WaterSimulatorEntity>())
//        {
//            GenerateTerrain(waterEntity);
//        }
//    }

//    void GenerateTerrain(WaterSimulatorEntity waterEntity)
//    {
//        float scale = waterEntity.waterSimulator.scale;
//        for (int x = 0; x < waterEntity.waterSimulator.xSize + 1; x++)
//        {
//            for (int z = 0; z < waterEntity.waterSimulator.zSize + 1; z++)
//            {
//                //float terrainHeight = GetWaveHeight(x * scale, z * scale);
//                //vertices[x * (waterEntity.waterSimulator.xSize + 1) + z].y = terrainHeight;
//                waterEntity.waterSimulator.meshGenerator.mesh.vertices[x * (waterEntity.waterSimulator.xSize + 1) + z] = GetWavePosition(x * scale, z * scale);
//            }
//        }

//        waterEntity.waterSimulator.meshGenerator.UpdateMesh();
//    }

//    float GetWaveHeight(float x, float z)
//    {
//        float heightDeviation = 0;
//        Vector3 samplePos = new Vector3(x, 0, z);
//        foreach (var particle in GetEntities<WaveParticleEntity>())
//        {
//            float amp = particle.waveParticle.amplitude;
//            float radius = particle.waveParticle.radius;
//            Vector3 direction = particle.waveParticle.direction.normalized;
//            Vector3 particlePos = particle.transform.position;
//            float relativeDistance = Vector3.Distance(samplePos, particlePos);

//            // Vertical Deviation
//            float rectFuncParam = relativeDistance / (2 * radius);
//            float rectFunc = rectFuncParam < 1.5f ? 1 : 0;
//            float Di = (amp / 2) * (Mathf.Cos(relativeDistance / radius) + 1);

//            // Horizontal Deviation
//            Vector3 DiL = - Mathf.Sin(relativeDistance / radius) * rectFunc * direction * Di;


//            heightDeviation += Di * rectFunc;
//        }


//        return heightDeviation;
//    }

//    // Return 
//    Vector3 GetWavePosition(float x, float z)
//    {
//        Vector3 finalPos = new Vector3(x, 0, z);
//        Vector3 samplePos = new Vector3(x, 0, z);
//        foreach(var particle in GetEntities<WaveParticleEntity>())
//        {
//            float relativeDistance = Vector3.Magnitude(samplePos - particle.transform.position);

//            // Vertical Deviation
//            float rectFunc = relativeDistance / (2 * particle.waveParticle.radius) < 1.5f ? 1 : 0;

//            // Di
//            finalPos.y += (particle.waveParticle.amplitude / 2) * (Mathf.Cos(relativeDistance / particle.waveParticle.radius) + 1) * rectFunc;

//            // DiL
//            finalPos += particle.waveParticle.horizontalDeviationAmplitude * Mathf.Sin(relativeDistance / particle.waveParticle.radius) * rectFunc * particle.waveParticle.direction.normalized;
//        }


//        return finalPos;
//    }
//}
