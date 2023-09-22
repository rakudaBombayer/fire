using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DamageUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text hpText;
    [SerializeField] Text damageText;
    [SerializeField] Image hpBar;

    public UnityAction OnEndAnim; // ゲージのアニメーションが終了した時に実行したいことを登録

    // Statusの表示
    public void Show(Character character, int damage)
    {
        gameObject.SetActive(true);
        nameText.text = character.Name;
        hpText.text = $"HP:{character.Hp}/{character.MaxHp}";
        damageText.text = $"{damage}ダメージ";
        // hpBar.fillAmount = (float)character.Hp / (float)character.MaxHp;
        float endValue = (float)character.Hp / (float)character.MaxHp;
        hpBar.DOFillAmount(endValue, 0.3f).SetEase(Ease.Linear);
        //表示の消える速度
        Invoke("Hide", 1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        // この時にGameManagerのOnPlayerTurnを実行したい
        OnEndAnim?.Invoke();
    }
}
