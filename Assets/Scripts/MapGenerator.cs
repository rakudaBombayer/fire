using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップの生成
//タイルを生成して、配置する
//大きさを決める
//prefab生成
/// </summary>
public class MapGenerator : MonoBehaviour
{
    [SerializeField] TileObj grassPrefab;
    [SerializeField] TileObj forestPrefab;
    [SerializeField] TileObj waterPrefab;
    [SerializeField] Transform tileParent;

    const int WIDTH = 15;
    const int HEIGHT = 9;
    const int WATER_RATE = 10;
    const int FOREST_RATE = 30;
    private void Start()
    {   
        Generate();
    }

    void Generate()
    {   
        Vector2 offset = new Vector2(-WIDTH/2, -HEIGHT/2);
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {   
                Vector2 pos = new Vector2(x, y) + offset;
                int rate = Random.Range(0, 100); //0~99までの数字がランダムで1つ出る
                if (rate < WATER_RATE)
                {
                    Instantiate(waterPrefab, pos, Quaternion.identity, tileParent);
                }
                else if (rate < FOREST_RATE)
                {
                    Instantiate(forestPrefab, pos, Quaternion.identity, tileParent);
                }
                else
                {
                    Instantiate(grassPrefab, pos, Quaternion.identity, tileParent);
                }
            }
        }
    }
}
