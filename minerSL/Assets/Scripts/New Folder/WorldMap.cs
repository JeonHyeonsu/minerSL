using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public int worldSize = 100;

    public float terDetail;  //Mathf.PerlinNoise적용
    public float terHeight;  //Mathf.PerlinNoise적용
    int seed;                //Mathf.PerlinNoise적용



    public GameObject[] blocks; //블록 저장

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(100000, 999999);
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //땅 만들기 함수
    void GenerateTerrain()
    {

        Vector2 pos;
        pos = GameObject.FindGameObjectWithTag("Block").transform.position;

        //땅의 좌우,상하 크기까지 반복하며 블록복사 배치 
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                GameObject block = Instantiate(blocks[0], new Vector3(pos.x + x, pos.y + y, 0), Quaternion.identity) as GameObject;
                block.transform.SetParent(GameObject.FindGameObjectWithTag("Block").transform);
            }
        }
    }
}
