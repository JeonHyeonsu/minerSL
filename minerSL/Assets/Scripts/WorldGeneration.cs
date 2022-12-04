using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public int dirtLayerHeight = 5;

    public Sprite grass;
    public Sprite dirt;
    public Sprite stone;

    public bool generateCaves = true; // 동굴생성 bool
    public float surfaceValue = 0.25f;
    public int worldSize = 50; // 월드 크기
    public float caveFreq = 0.05f; // 동굴 빈도
    public float terrainFreq = 0.05f;
    public float heightMultiplier = 4f; // 높이 높낮이 생성 ?
    public int heightAddition = 25; // 높이 더하기 ?

    public float seed;
    public Texture2D noiseTexture;

    public List<Vector2> worldTiles = new List<Vector2>();

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            //float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;

            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) + heightAddition;
            //Mathf.PerlinNoise 입력 값(x,y)의 변화에 따라 서서히 변화하는 난수를 생성 얻을 수 있는 값은 0~1 x와 y의 값이 같으면 항상 같은 값을 반환

            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y < height - dirtLayerHeight)
                {
                    tileSprite = stone;
                }
                else if(y < height - 1)
                {
                    tileSprite = dirt;
                }
                else
                {
                    // 가장높은 타일스프라이트
                    tileSprite = grass;
                }
                if (generateCaves)
                {
                    if (noiseTexture.GetPixel(x, y).r > surfaceValue)// 동굴생성
                    {
                        PlaceTile(tileSprite, x, y);
                    }
                }
                else
                {
                    PlaceTile(tileSprite, x, y);
                }
            }
        }
    }

    public void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);

        for(int x = 0; x < noiseTexture.width; x++)
        {
            for(int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise(x * caveFreq, y * caveFreq);
                noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }

        noiseTexture.Apply();
    }

    public void PlaceTile(Sprite tileSprite, float x, float y)
    {
        GameObject newTile = new GameObject();
        newTile.transform.parent = this.transform;
        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

        worldTiles.Add(newTile.transform.position); // 나무 생성에 쓰임.
    }
}
