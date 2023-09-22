using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;


public class PhasePanelUI : MonoBehaviour
{   
    [SerializeField] Text text;
    private void Start()
    {   
        // ・rotationを90にする
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    // OnEndAnimは渡された関数: 何も渡されなかったらnull
    public IEnumerator PanelAnim(string message, UnityAction OnEndAnim = null)
    {   
        text.text = message;
        // 1フレーム待機
        yield return new WaitForSeconds(0.3f);
        // ・0にちかづける(DOTween) WaitForCompletion:アニメーション終了まで待ってくれる
        yield return transform.DORotate(new Vector3(0, 0, 0), 0.3f).WaitForCompletion();
        yield return new WaitForSeconds(0.7f);
        //・90に戻す(DOTween)
        yield return transform.DORotate(new Vector3(90, 0, 0), 0.3f).WaitForCompletion();
        yield return new WaitForSeconds(0.2f);
        // パネル表示が終わってやりたいこと
        OnEndAnim?.Invoke();
    }
}

//やること
// ・フェーズアニメーションを実際にゲームの中に入れていく
// ・"ENEMY TURN"などの表示をかえる
