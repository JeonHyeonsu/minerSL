using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [Header("Tile Atlas")]
    public TileAtlas tileAtlas;
    public float seed;

    public BiomeClass[] biomes;


    [Header("Biomes")]
    public float biomeFrequency;
    public Gradient biomeColors; // Gradient 컬러에 변화
    public Texture2D biomeMap;

    [Header("Generation Settings")]
    public int chunkSize = 16;
    public int worldSize = 100; // 월드 크기
    public int dirtLayerHeight = 5;
    public bool generateCaves = true; // 동굴생성 bool
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f; // 높이 높낮이 생성 ?
    public int heightAddition = 25; // 높이 더하기 ?

    [Header("Noise Settings")]
    public float caveFreq = 0.05f; // 동굴 빈도
    public float terrainFreq = 0.05f;
    public Texture2D caveNoiseTexture;

    [Header("Ore Settings")]
    public OreClasses[] ores;

    private GameObject[] worldChunks;
    private List<Vector2> worldTiles = new List<Vector2>();

    private void OnValidate()
    {
        DrawTextures();
    }

    private void Start()
    {
        seed = Random.Range(-10000, 10000);

        DrawTextures();

        CreateChunks();
        GenerateTerrain();
    }

    public void DrawTextures()
    {

        biomeMap = new Texture2D(worldSize, worldSize);
        DrawBiomeTexture();

        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i].caveNoiseTexture = new Texture2D(worldSize, worldSize);
            for(int o = 0; o < biomes[i].ores.Length; o++)
            {
                biomes[i].ores[o].spreadTexture = new Texture2D(worldSize, worldSize);
            }

            GenerateNoiseTexture(biomes[i].caveFreq, biomes[i].surfaceValue, biomes[i].caveNoiseTexture);

            for (int o = 0; o < biomes[i].ores.Length; o++)
            {
                GenerateNoiseTexture(biomes[i].ores[o].rarity, biomes[i].ores[o].size, biomes[i].ores[o].spreadTexture);
            }
        }
    }

    public void DrawBiomeTexture()
    {
        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * biomeFrequency, (y + seed) * biomeFrequency);
                Color col = biomeColors.Evaluate(v);
                biomeMap.SetPixel(x, y, col);
            }
        }

        biomeMap.Apply();
    }

    public void CreateChunks()
    {
        int numChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numChunks];
        for (int i = 0; i < numChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform; // parent 오브젝트의 부모를 설정하거나 반환한다
            worldChunks[i] = newChunk;
        }
    }
    /** 청크라는 단위 개념을 이용해 끝없이 로드되는 지형을 생성한다.
     하나의 거대한 덩어리라는 뜻으로 끝없는 세계를 구현할 때 맵 데이터를 관리하기 위해 사용
     청크를 사용하는 이유는 무엇인가?
         절차적으로 만들어진 세계는 시드가 동일한 이상 언제나 생성되는 결과물은 동일하다.
         이러한 점을 이용하여 동적으로 메모리에 맵을 생성하고 지울 수 있는데, 맵을 청크 단위가 아닌 블록 단위(1 * 1 * 1)로 생성하게 되면 메모리에 부하가 생기고 소요되는 시간 또한 길어진다.
         그래서 여러 개의 블록을 하나로 묶어 관리하게 되면 맵을 지우거나 로드할 때 빠른 속도로 처리할 수 있다.**/

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;

            ///float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) + heightAddition;
            //Mathf.PerlinNoise 입력 값(x,y)의 변화에 따라 서서히 변화하는 난수를 생성 얻을 수 있는 값은 0~1 x와 y의 값이 같으면 항상 같은 값을 반환

            for (int y = 0; y < height; y++)
            {
                Sprite[] tileSprites; // 타일 생성 하는곳
                if (y < height - dirtLayerHeight)
                {
                    tileSprites = tileAtlas.stone.tileSprites;

                    if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].maxSpawnHeight)
                        tileSprites = tileAtlas.coal.tileSprites;
                    if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].maxSpawnHeight)
                        tileSprites = tileAtlas.iron.tileSprites;
                    if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].maxSpawnHeight)
                        tileSprites = tileAtlas.gold.tileSprites;
                    if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].maxSpawnHeight)
                        tileSprites = tileAtlas.diamond.tileSprites;
                    //ores[0] 석탄 ores[1] 철 ores[2] 금 ores[3] 다이아몬드
                }
                else if(y < height - 1)
                {
                    tileSprites = tileAtlas.dirt.tileSprites;
                }
                else
                {
                    // 가장높은 타일스프라이트
                    tileSprites = tileAtlas.grass.tileSprites;
                }
                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)// 동굴생성
                    {
                        PlaceTile(tileSprites, x, y);
                    }
                }
                else
                {
                    PlaceTile(tileSprites, x, y);
                }
            }
        }
    }

    public void GenerateNoiseTexture(float frequency, float limit , Texture2D noiseTexture)
    {

        for(int x = 0; x < noiseTexture.width; x++)
        {
            for(int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                if(v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
            }
        }

        noiseTexture.Apply();
    }

    public void PlaceTile(Sprite[] tileSprites, int x, int y)
    {
        if (!worldTiles.Contains(new Vector2Int(x, y)))
        {
            GameObject newTile = new GameObject();

            float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize);
            chunkCoord /= chunkSize;

            newTile.transform.parent = worldChunks[(int)chunkCoord].transform;


            newTile.AddComponent<SpriteRenderer>();

            int spriteIndex = Random.Range(0, tileSprites.Length);
            newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[spriteIndex];


            newTile.name = tileSprites[0].name;
            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

            //worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f)); // 나무 생성에 쓰임.
        }
    }
}
