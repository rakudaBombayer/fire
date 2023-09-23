using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class Character : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] new string name;
    [SerializeField] int hp;
    [SerializeField] int maxHp;
    [SerializeField] int at;
    [SerializeField] int df;
    [SerializeField] bool isEnemy;
    [SerializeField] Vector2Int positionInt;
    [SerializeField] int moveRange;
    bool isMoved;
    
    public Vector2Int Position { get => positionInt;}
    public bool IsEnemy { get => isEnemy; }
    public string Name { get => name; }
    public int Hp { get => hp; }
    public int At { get => at; }
    public int Df { get => df; } 
    public int MaxHp { get => maxHp; }
    public int MoveRange { get => moveRange; }
    public bool IsMoved { get => isMoved; }

    void Start()
    {
        transform.position = (Vector2)positionInt;
    }

    //キャラを移動させる
    public void Move(Vector2Int pos, List<TileObj> root)
    {   
        // Selectを使って、リストの中の特定の要素だけを取得したリストを作る
        Vector3[] path = root.Select(tile => tile.transform.position).ToArray();
        //経路に沿って移動する(経路, 移動時間);
        transform.DOPath(path,0.3f).SetEase(Ease.Linear);

        positionInt = pos;
        isMoved = true;
        // transform.position = (Vector2)positionInt;
        // transform.DOMove((Vector2)positionInt, 0.3f).SetEase(Ease.Linear);
    }

    public int Damage(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            hp = 0;
        }
        return value;
    }

    public int Attack(Character target)
    {
       return target.Damage(at);
    }

    public void OnBeginTurn()
    {
        isMoved = false;
    }
}


// 選択タイルの取得
// キャラの選択
// 選択タイルの座標とキャラの座標を比較する
//キャラの移動