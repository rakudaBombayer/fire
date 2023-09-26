using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

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
        EnemyCharacterTargetSelection, // 攻撃対象選択
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
    [SerializeField] PhasePanelUI phasePanelUI;
    [SerializeField] GameObject turnEndButton;

    private void Start()
    {   
        damageUI.OnEndAnim += OnAttacked;
        phase = Phase.PlayerCharacterSelection;
        actionCommandUI.Show(false);
        StartCoroutine(phasePanelUI.PanelAnim("ふみやのターン"));
        turnEndButton.SetActive(true);
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //UIをクリックした場合
            return;
        }

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
                    // キャラのステータスを表示
                    statusUI.Show(selectedCharacter);
                    // もし自分のキャラが動いていないなら,移動範囲を表示
                    if(character.IsMoved == false && character.IsEnemy == false)
                    {
                        mapManager.ResetMovablePanels(movableTiles);
                        //移動範囲を表示
                        mapManager.ShowMovablePanels(selectedCharacter, movableTiles);
                        return true;
                    }
                }
                return false;
        }

   void PlayerCharacterSelection()
   {    
        //クリックしたタイルを取得して
        TileObj clickTileObj = mapManager.GetClickTileObj();

        //その上にキャラがいるなら
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
                            //TODO: 経路を取得して、移動する
                            selectedCharacter.Move(clickTileObj.positionInt, mapManager.GetRoot(selectedCharacter,clickTileObj), null);
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
        // OnPlayerTurnEnd();
        actionCommandUI.Show(false);
        selectedCharacter = null;
        mapManager.ResetAttackablePanels(attackableTiles);
        phase = Phase.PlayerCharacterSelection;
    }

    // 攻撃が終わったよ
    void OnAttacked()
    {   
        if(phase == Phase.PlayerCharacterTargetSelection)
        {
            actionCommandUI.Show(false);
            selectedCharacter = null;
            mapManager.ResetAttackablePanels(attackableTiles);
            phase = Phase.PlayerCharacterSelection;
        }
        if(phase == Phase.EnemyCharacterTargetSelection)
        {
            actionCommandUI.Show(false);
            selectedCharacter = null;
            mapManager.ResetAttackablePanels(attackableTiles);
            EnemyCharacterSelection();
        }
        
    }
    void OnPlayerTurnEnd()
    {   
        foreach (var chara in charactersManager.characters)
        {
            if (chara.IsEnemy)
            {
                chara.OnBeginTurn();
            }
        }
        //敵のフェーズへ
        Debug.Log("相手ターン");
        phase = Phase.EnemyCharacterSelection;
        actionCommandUI.Show(false);
        selectedCharacter = null;
        mapManager.ResetAttackablePanels(attackableTiles);
        StartCoroutine(phasePanelUI.PanelAnim("ジーズのターン", EnemyCharacterSelection));//フェーズアニメを実行
        //EnemyCharacterSelection();//フェーズアニメが終わったら実行したい
    }

    //キャラ選択
    void EnemyCharacterSelection()
    {   
        Debug.Log("敵キャラ選択");
        //--- characterManagerからランダムに敵を持ってくる
        selectedCharacter = charactersManager.GetMovableEnemy();
        if(selectedCharacter)
        {
            mapManager.ResetMovablePanels(movableTiles);
            //移動範囲を表示
            mapManager.ShowMovablePanels(selectedCharacter, movableTiles);  
            EnemyCharacterMoveSelection();
        }
        else
        {
            OnEnemyTurnEnd();
        }
    }

    //ここが怪しい↓EnemyCharacterMoveSelection();が記述していない
    void EnemyCharacterMoveSelection()
    {   
        //手順
        // ターゲトとなるPlayerを見つける => 一番近いPlayer
        Character target = charactersManager.GetClosetCharacter(selectedCharacter);
        // 移動範囲の中で、Playerに近い場所を探す
        TileObj targetTile = movableTiles
            .OrderBy(tile => Vector2.Distance(target.Position, tile.positionInt))//小さい順に並べ変える
            .FirstOrDefault(); // 最初のタイルを渡す

        selectedCharacter.Move(targetTile.positionInt,mapManager.GetRoot(selectedCharacter,targetTile), EnemyCharacterTargetSelection);
        mapManager.ResetMovablePanels(movableTiles);
    }

    // 敵の攻撃
    void EnemyCharacterTargetSelection()
    {    
        phase = Phase.EnemyCharacterTargetSelection;
        // 攻撃範囲の取得
        //範囲内にPlayerのキャラがいれば取得
        //Playerがいるなら攻撃を実行する

        mapManager.ResetAttackablePanels(attackableTiles);
        mapManager.ShowAttackablePanels(selectedCharacter, attackableTiles);
        //範囲内にPlayerのキャラがいれば取得
        Character targetChara = null;
        foreach (var tile in attackableTiles) 
        {   
            Character character = charactersManager.GetCharacter(tile.positionInt);
            if(character && character.IsEnemy == false)
            {
                targetChara = character;
            }
        }
        //ターゲットがいるなら攻撃を実行する
        if(targetChara)
        {
            //攻撃処理
            int damage = selectedCharacter.Attack(targetChara);
            mapManager.ResetAttackablePanels(attackableTiles);
            actionCommandUI.Show(false);
            damageUI.Show(targetChara, damage); 
        }
        else
        {
            EnemyCharacterSelection();
        }
    }

    void OnEnemyTurnEnd()
    {
        //プレイヤーのフェーズへ
        selectedCharacter = null;
        phase = Phase.PlayerCharacterSelection;
        StartCoroutine(phasePanelUI.PanelAnim("ふみやのターン"));
        mapManager.ResetAttackablePanels(attackableTiles);
        turnEndButton.SetActive(true);
        foreach (var chara in charactersManager.characters)
        {
            if (chara.IsEnemy == false)
            {
                chara.OnBeginTurn();
            }
        }
    }

    public void OnTurnENdButton()
    {
        OnPlayerTurnEnd();
        turnEndButton.SetActive(false);
    }
} 

// 全ての敵が動いたらPlayerのターンになる TODO
// ・移動したかどうかを確認していなければターン終了


