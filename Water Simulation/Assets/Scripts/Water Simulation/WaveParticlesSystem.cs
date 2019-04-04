using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class WaveParticlesSystem : ComponentSystem
{

    private struct WaveParticleEntity
    {
        public WaveParticleComponent waveParticleComponent;
        //public Rigidbody rigidbody;
        public Transform transform;
    }

    protected override void OnUpdate()
    {
        foreach(var entity in GetEntities<WaveParticleEntity>())
        {
            // Update Wave Particle Movement
            UpdateParticle(entity);
        }
    }

    private void UpdateParticle(WaveParticleEntity particle)
    {

        Vector3 newPosition = particle.transform.position;

        newPosition += Time.deltaTime * particle.waveParticleComponent.velocity * particle.waveParticleComponent.direction;


        // Reflect direction when it touches the edges
        if (newPosition.z < particle.waveParticleComponent.bottomBound)
        {
            particle.waveParticleComponent.direction = Vector3.Reflect(particle.waveParticleComponent.direction, new Vector3(0, 0, 1));
        }
        else if (newPosition.z > particle.waveParticleComponent.topBound)
        {
            particle.waveParticleComponent.direction = Vector3.Reflect(particle.waveParticleComponent.direction, new Vector3(0, 0, -1));
        }
        else if (newPosition.x < particle.waveParticleComponent.leftBound)
        {
            particle.waveParticleComponent.direction = Vector3.Reflect(particle.waveParticleComponent.direction, new Vector3(1, 0, 0));
        }
        else if (newPosition.x > particle.waveParticleComponent.rightBound)
        {
            particle.waveParticleComponent.direction = Vector3.Reflect(particle.waveParticleComponent.direction, new Vector3(-1, 0, 0));
        }

        particle.transform.position = newPosition;

        // Update position
        //Vector3 direction = particle.waveParticleComponent.direction.normalized;
        //float velocity = particle.waveParticleComponent.velocity;

        //particle.transform.position = (velocity * direction * Time.deltaTime + particle.transform.position);

        //// Update Amplitude
        //float threshold = particle.waveParticleComponent.amplitudeThreshold;

        //particle.waveParticleComponent.amplitude = Mathf.SmoothDamp(
        //                                                    particle.waveParticleComponent.amplitude,
        //                                                    0,
        //                                                    ref particle.waveParticleComponent.amplitudeDecay,
        //                                                    particle.waveParticleComponent.lifetime
        //                                                );

        //if (particle.waveParticleComponent.amplitude < threshold)
        //{
        //    GameObject.Destroy(particle.waveParticleComponent.gameObject);
        //}

        //particle.rigidbody.velocity = particle.waveParticleComponent.velocity * particle.rigidbody.velocity.normalized;
    }
}
