using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] Cursor cursor;
    [SerializeField] CharactersManager charactersManager;
    [SerializeField] MapGenerator mapGenerator;

    Character selectedCharacter;

    private void Start()
    {
        mapGenerator.Generate();
    }

    //クリックした場所を取得したい
    //クリック判定 =>Update関数の中でInputを使う
    //クリックしたオブジェクトを取得したい => クリックした場所にRayを飛ばしてオブジェクトを取得する

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(
                clickPosition,
                Vector2.down 
            );
            //Rayを飛ばしてヒットしたタイルを取得する
            if (hit2D && hit2D.collider)
            {
                cursor.SetPosition(hit2D.transform);
                TileObj tileObj = hit2D.collider.GetComponent<TileObj>();
                //選択タイルの座標
                
                //キャラの座標
                Character character = charactersManager.GetCharacter(tileObj.positionInt);
                if (character)
                {
                    Debug.Log("いる");
                    //選択キャラの保持
                    selectedCharacter = character;
                }
                else
                {
                    Debug.Log("クリックした場所にキャラがいない");
                    //キャラを保持しているなら、クリックしたタイルの場所に移動させる
                    if (selectedCharacter)
                    {
                        // selectedCharacterをtileObjまで移動させる
                        selectedCharacter.Move(tileObj.positionInt);
                        selectedCharacter = null;
                    }
                }
            }
        }
    }

}
