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
    [SerializeField] TileObj tileObjPrefab;

    private void Start()
    {
        Generate();
    }

    void Generate()
    {
        Instantiate(tileObjPrefab);
    }
}
