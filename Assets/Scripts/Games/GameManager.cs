using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //フェーズの管理
    enum Phase
    {
        PlayerCharacterSelection, // キャラ選択
        PlayerCharacterMoveSelection,// キャラ移動
        EnemyCharacterSelection,
        EnemyCharacterMoveSelection,
    }

    //選択キャラの保持
    Character selectedCharacter;
    //選択キャラの移動可能範囲の保持
    List<TileObj> movableTiles = new List<TileObj>();

    [SerializeField] Phase phase;
    [SerializeField] CharactersManager charactersManager;
    [SerializeField] MapManager mapManager;

    private void Start()
    {
        phase = Phase.PlayerCharacterSelection;
    }
    //PlayerCharacterSelection, //キャラ選択
    // PlayerCharacterSelection, // キャラ移動
    // Playerがクリックしたら処理したい

    private void Update()
    {   
        // Playerがクリックしたら処理したい
        if (Input.GetMouseButtonDown(0))
        {
            PlayerClickAction();
        }
    }

    void PlayerClickAction()
    {
        switch (phase)
        {
            case Phase.PlayerCharacterSelection:
                PlayerCharacterSelection();
                break;
            case Phase.PlayerCharacterMoveSelection:
                PlayerCharacterMoveSelection();
                break;
        }
    }

    //PlayerCharacterSelectionフェーズでクリックした時にやりたいこと

    bool IsClickCharacter(TileObj clickTileObj)
    {
        Character character = charactersManager.GetCharacter(clickTileObj.positionInt);
                if (character)
                {
                    //選択キャラの保持
                    selectedCharacter = character;
                    mapManager.ResetMovablePanels(movableTiles);
                    //移動範囲を表示
                    mapManager.ShowMovablePanels(selectedCharacter, movableTiles);      
                    return true;
                }
                return false;
    }

   void PlayerCharacterSelection()
   {    
        //クリックしたタイルを取得して
        //その上にキャラがいるなら
        TileObj clickTileObj = mapManager.GetClickTileObj();
        if(IsClickCharacter(clickTileObj))
        {
            phase = Phase.PlayerCharacterMoveSelection;
        }
   } 

   void PlayerCharacterMoveSelection()
   {    
        //TODO選択したキャラを移動させる

        // クリックした場所が移動範囲なら移動させて、敵のフェーズへ
        TileObj clickTileObj = mapManager.GetClickTileObj();
        //キャラを取得して、移動範囲を表示
        // キャラクターがいるなら
        if(IsClickCharacter(clickTileObj))
        {
            return;
        }
        
            if (selectedCharacter)
                    {
                        // クリックしたタイルtileObjが移動範囲に含まれるなら
                        if (movableTiles.Contains(clickTileObj))
                        {
                            // selectedCharacterをtileObjまで移動させる
                            selectedCharacter.Move(clickTileObj.positionInt);
                        }
                            mapManager.ResetMovablePanels(movableTiles);
                            selectedCharacter = null;                       
                    }
    }
} 
