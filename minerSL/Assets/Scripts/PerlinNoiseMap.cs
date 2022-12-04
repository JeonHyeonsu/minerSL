using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_plains;
    public GameObject prefab_forest;
    public GameObject prefab_hills;
    public GameObject prefab_mountains;

    int map_width = 50;
    int map_height = 50;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    // recommend 4 to 20
    float magnification = 7.0f;

    int x_offset = 0; // <- +>
    int y_offset = 0; // v- +^

    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    void CreateTileset()
    {
        /** 쉽게 액세스할 수 있도록 ID 코드를 수집하고 타일 프리팹에 할당합니다.
            대지 고도에 가장 적합한 순서 **/

        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_plains);
        tileset.Add(1, prefab_forest);
        tileset.Add(2, prefab_hills);
        tileset.Add(3, prefab_mountains);
    }

    void CreateTileGroups()
    {
        /** 동일한 유형의 타일을 그룹화하기 위한 빈 게임 개체 만들기 **/

        tile_groups = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    void GenerateMap()
    {
        /** Perlin 노이즈 기능을 사용하여 2D 그리드를 생성하고 다음과 같이 저장합니다.
            원시 ID 값과 타일 게임 개체 모두 **/

        for (int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for (int y = 0; y < map_height; y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        /** 그리드 좌표 입력을 사용하여 다음과 같은 Perlin 노이즈 값을 생성합니다.
            타일 ID 코드로 변환되었습니다. 정규화된 Perlin 값의 크기 조정
            사용 가능한 타일 수만큼. **/

        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification
        );
        float clamp_perlin = Mathf.Clamp01(raw_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;

        // 4를 타일 세트로 대체합니다.타일을 쉽게 추가할 수 있도록 카운트
        if (scaled_perlin == tileset.Count)
        {
            scaled_perlin = (tileset.Count - 1);
        }
        return Mathf.FloorToInt(scaled_perlin);
    }

    void CreateTile(int tile_id, int x, int y)
    {
        /** 형식 ID 코드를 사용하여 새 타일을 만듭니다. 공통으로 그룹화합니다.
            타일, 위치를 설정하고 게임 객체를 저장합니다. **/

        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);

        tile_grid[x].Add(tile);
    }
}
