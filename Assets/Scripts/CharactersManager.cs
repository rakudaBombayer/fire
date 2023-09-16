using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    //キャラ全てを管理する
    public List<Character> characters = new List<Character>();
    void Start()
    {
        //データ型が一致する小要素を取得する
        GetComponentsInChildren(characters);
    }

}
