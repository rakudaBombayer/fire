using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector2Int positonInt;
    void Start()
    {
        transform.position = (Vector2)positonInt; 
    }
}


// 選択タイルの取得
// キャラの選択
//キャラの移動