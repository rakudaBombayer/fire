using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int hp;
    [SerializeField] int at;
    [SerializeField] bool isEnemy;
    [SerializeField] Vector2Int positionInt;
    
    public Vector2Int Position { get => positionInt;}
    public bool IsEnemy { get => isEnemy; }

    void Start()
    {
        transform.position = (Vector2)positionInt;
    }

    //キャラを移動させる
    public void Move(Vector2Int pos)
    {
        positionInt = pos;
        transform.position = (Vector2)positionInt;
    }

    public void Damage(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            hp = 0;
        }
    }

    public void Attack(Character target)
    {
        target.Damage(at);
    }

}


// 選択タイルの取得
// キャラの選択
// 選択タイルの座標とキャラの座標を比較する
//キャラの移動