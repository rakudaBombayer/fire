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
        PlayerCharacterCommandSelection,// コマンド選択
        PlayerCharacterTargetSelection,// 攻撃対象選択
        EnemyCharacterSelection,
        EnemyCharacterMoveSelection,
    }

    //選択キャラの保持
    Character selectedCharacter;
    //選択キャラの移動可能範囲の保持
    List<TileObj> movableTiles = new List<TileObj>();
    //選択キャラの攻撃範囲の保持
    List<TileObj> attackableTiles = new List<TileObj>();


    [SerializeField] Phase phase;
    [SerializeField] CharactersManager charactersManager;
    [SerializeField] MapManager mapManager;
    [SerializeField] ActionCommandUI actionCommandUI;
    [SerializeField] StatusUI statusUI;
    [SerializeField] DamageUI damageUI;

    private void Start()
    {   
        damageUI.OnEndAnim += OnPlayerTurnEnd;
        phase = Phase.PlayerCharacterSelection;
        actionCommandUI.Show(false);
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
            case Phase.PlayerCharacterTargetSelection:
                PlayerCharacterTargetSelection();
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
                    statusUI.Show(selectedCharacter);
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
                            phase = Phase.PlayerCharacterCommandSelection;
                            // コマンドの表示
                            actionCommandUI.Show(true);
                        }
                            mapManager.ResetMovablePanels(movableTiles);                       
                }
        }
    void PlayerCharacterTargetSelection()
   {    
        //TODO選択したキャラを移動させる
        TileObj clickTileObj = mapManager.GetClickTileObj();

        // 攻撃の範囲内をクリックしたら
        if (attackableTiles.Contains(clickTileObj))
        {
            //敵キャラクターがいるなら 
            Character targetChara = charactersManager.GetCharacter(clickTileObj.positionInt);
            if (targetChara && targetChara.IsEnemy)
            {   
                //攻撃処理
                int damage = selectedCharacter.Attack(targetChara);
                mapManager.ResetAttackablePanels(attackableTiles);
                actionCommandUI.Show(false);
                damageUI.Show(targetChara, damage);            
            }
        }
    }
    public void OnAttackButton()
    {
        phase = Phase.PlayerCharacterTargetSelection;
        //攻撃範囲の表示    
        mapManager.ResetAttackablePanels(attackableTiles);
        mapManager.ShowAttackablePanels(selectedCharacter, attackableTiles);
        actionCommandUI.ShowAttackButton(false);
    }
    public void OnWaiteButton()
    {
        OnPlayerTurnEnd();
    }

    void OnPlayerTurnEnd()
    {
        //敵のフェーズへ
        Debug.Log("相手ターン");
        phase = Phase.EnemyCharacterSelection;
        actionCommandUI.Show(false);
        selectedCharacter = null;
        mapManager.ResetAttackablePanels(attackableTiles);
        EnemyCharacterSelection();
    }

    //キャラ選択
    void EnemyCharacterSelection()
    {   
        Debug.Log("敵キャラ選択");
        //--- characterManagerからランダムに敵を持ってくる
        selectedCharacter = charactersManager.GetRandomEnemy();

        mapManager.ResetMovablePanels(movableTiles);
        //移動範囲を表示
        mapManager.ShowMovablePanels(selectedCharacter, movableTiles);  
        EnemyCharacterMoveSelection();
    }
    // 移動
    void EnemyCharacterMoveSelection()
    {
        //---
        Debug.Log("敵キャラの移動");
        //ランダムに移動場所を決めて移動する
        int r = Random.Range(0, movableTiles.Count);
        selectedCharacter.Move(movableTiles[r].positionInt);
        mapManager.ResetMovablePanels(movableTiles);
        OnEnemyTurnEnd();
    }

    void OnEnemyTurnEnd()
    {
        //プレイヤーのフェーズへ
        Debug.Log("敵ターン終了");
        selectedCharacter = null;
        phase = Phase.PlayerCharacterSelection;
    }
} 
