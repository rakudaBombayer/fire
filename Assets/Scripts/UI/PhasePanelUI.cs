using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;


public class PhasePanelUI : MonoBehaviour
{
    private void Start()
    {   
        // ・rotationを90にする
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    // OnEndAnimは渡された関数: 何も渡されなかったらnull
    public IEnumerator PanelAnim(UnityAction OnEndAnim = null)
    {   
        // 1フレーム待機
        yield return new WaitForSeconds(0.5f);
        // ・0にちかづける(DOTween) WaitForCompletion:アニメーション終了まで待ってくれる
        yield return transform.DORotate(new Vector3(0, 0, 0), 0.7f).WaitForCompletion();
        yield return new WaitForSeconds(1f);
        //・90に戻す(DOTween)
        yield return transform.DORotate(new Vector3(90, 0, 0), 0.7f).WaitForCompletion();

        // パネル表示が終わってやりたいこと
        OnEndAnim?.Invoke();
    }
}

//やること
// ・フェーズアニメーションを実際にゲームの中に入れていく
// ・"ENEMY TURN"などの表示をかえる
