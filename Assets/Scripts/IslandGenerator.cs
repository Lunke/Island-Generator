using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{

    public MeshFilter meshFilter;
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;

    public Gradient gradient;

    private float minTerrainHeight;
    private float maxTerrainHeight;

    public int seed = 0;
    public float noiseScale = 10f;
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;
    public Vector2 offset;
    public AnimationCurve dropOffCurve;

    public float yScale = 2f;
    void Start()
    {
		createAndUpdateMesh();
    }

    public void createAndUpdateMesh()
	{
		createMesh();
		updateMesh();
	}

    public void createMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int sd = seed == 0 ? Random.Range(1, int.MaxValue) : seed;
        
        float[,] noiseMap = Noise.generateNoiseMap(xSize + 1, zSize + 1, sd, noiseScale, octaves, persistance, lacunarity, offset);

        float maxDist = new Vector2(xSize/2, zSize/2).sqrMagnitude;

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float dist = new Vector2(xSize/2-x, zSize/2-z).sqrMagnitude;
                float mult = dropOffCurve.Evaluate(Mathf.InverseLerp(0.0000001f, maxDist, dist));
                noiseMap[x, z] *= mult;

                float y = noiseMap[x, z] * yScale;
                vertices[i] = new Vector3(x, y, z);

                if(y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }

    }

    public void updateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    private void OnValidate()
    {
        if (xSize < 1)
            xSize = 1;
        if (zSize < 1)
            zSize = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }
}

