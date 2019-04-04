using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTextureGenerator : MonoBehaviour
{
    public Texture2D texture;
    public int resolutionX, resolutionY;

    private WaveParticleSpawner waveParticleSpawner;
    private float[,] textureColorValues;
    private Material material;
    private float worldToTexScaleX, worldToTexScaleY;

    // Start is called before the first frame update
    void Start()
    {
        waveParticleSpawner = FindObjectOfType<WaveParticleSpawner>();

        textureColorValues = new float[resolutionX, resolutionY];

        texture = new Texture2D(resolutionX, resolutionY);

        texture.wrapMode = TextureWrapMode.Clamp;

        material = GetComponent<Renderer>().material;
        material.SetTexture("_HeightFieldTex", texture);
        material.SetTexture("_MainTex", texture);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textureColorValues = new float[resolutionX, resolutionY];

        foreach (WaveParticleComponent particle in waveParticleSpawner.waveParticles)
        {
            worldToTexScaleX = resolutionX / particle.rightBound;
            worldToTexScaleY = resolutionY / particle.topBound;
            circleBres(particle);
        }

        Divide(ref textureColorValues, Max(textureColorValues));

        for(int y = 0; y < resolutionY; y++)
        {
            for(int x = 0; x < resolutionX; x++)
            {
                float grayValue = textureColorValues[x, y];
                texture.SetPixel(x, y, new Color(grayValue, grayValue, grayValue));
            }
        }

       
        texture.Apply();
    }

    float Max(float[,] array)
    {
        float max = 0;
        for (int y = 0; y < resolutionY; y++)
        {
            for (int x = 0; x < resolutionX; x++)
            {
                if (array[x, y] > max)
                    max = array[x, y];
            }
        }

        return max;
    }

    void Divide(ref float[,] array, float d)
    {
        for (int y = 0; y < resolutionY; y++)
        {
            for (int x = 0; x < resolutionX; x++)
            {
                array[x, y] /= d;
            }
        }
    }

    void plot1(int x, int y, WaveParticleComponent particle)
    {
        float worldPosX = x + particle.transform.position.x;
        float worldPosY = y + particle.transform.position.z;

        Vector3 newPos = GetWavePosition(worldPosX, worldPosY, particle);
        newPos.x *= worldToTexScaleX;
        newPos.z *= worldToTexScaleY;

        if (newPos.x < 0 || newPos.x >= resolutionX || newPos.z < 0 || newPos.z >= resolutionY)
            return;

        textureColorValues[Mathf.FloorToInt(newPos.x), Mathf.FloorToInt(newPos.z)] += newPos.y;
    }


    void plot8(int x, int y, WaveParticleComponent particle)
    {
        for (int i = -x; i <= x; i++)
        {
            plot1(i, y, particle);
        }

        for (int i = -x; i <= x; i++)
        {
            plot1(i, -y, particle);
        }

        for (int i = -y; i <= y; i++)
        {
            plot1(i, x, particle);
        }

        for (int i = -y; i <= y; i++)
        {
            plot1(i, -x, particle);
        }

        //plot1(-x, y, particle);
        //plot1(x, y, particle);
        //plot1(-x, -y, particle);
        //plot1(x, -y, particle);
        //plot1(-y, x, particle);
        //plot1(y, x, particle);
        //plot1(-y, -x, particle);
        //plot1(y, -x, particle);
    }

    void circleBres(WaveParticleComponent particle)
    {
        int r = (int)particle.radius;
        int x = 0, y = r;
        int d = 3 - 2 * r;
        plot8(x, y, particle);
        while (y >= x)
        {
            // for each pixel we will 
            // draw all eight pixels 

            x++;

            // check for decision parameter 
            // and correspondingly  
            // update d, x, y 
            if (d > 0)
            {
                y--;
                d = d + 4 * (x - y) + 10;
            }
            else
                d = d + 4 * x + 6;
            plot8(x, y, particle);
        }
    }


    Vector3 GetWavePosition(float x, float z, WaveParticleComponent particle)
    {
        Vector3 finalPos = new Vector3(x, 0, z);
        Vector3 samplePos = new Vector3(x, 0, z);

        float relativeDistance = Vector3.Magnitude(samplePos - particle.transform.position);

        // Vertical Deviation
        float rectFunc = relativeDistance / (2 * particle.radius) < 1.5f ? 1 : 0;

        // Di
        finalPos.y += (particle.amplitude / 2) * (Mathf.Cos(relativeDistance / particle.radius) + 1) * rectFunc;

        // DiL
        finalPos += particle.horizontalDeviationAmplitude * Mathf.Sin(relativeDistance / particle.radius) * rectFunc * particle.direction.normalized;

        return finalPos;
    }

}
