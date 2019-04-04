using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class WaveParticleSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int count = 10000;
    public float radius = 4.0F;

    public List<WaveParticleComponent> waveParticles = new List<WaveParticleComponent>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddParticles();
        }
    }

    void AddParticles()
    {
        for (int i = 0; i != count; i++)
        {
            Vector2 position2D = Random.insideUnitCircle * Random.Range(0, radius);
            Vector3 position3D = new Vector3(position2D.x, 0, position2D.y);
            var obj = Instantiate(prefab, position3D + transform.position + new Vector3(64, 0, 64), Random.rotation);
            obj.GetComponent<WaveParticleComponent>().direction = position3D.normalized;

            waveParticles.Add(obj.GetComponent<WaveParticleComponent>());
        }
    }
}
