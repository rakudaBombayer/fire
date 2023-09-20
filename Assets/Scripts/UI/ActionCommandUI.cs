using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCommandUI : MonoBehaviour
{
    // ActionCommandのUIを管理する
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject waiteButton;

    public void Show(bool isActive)
    {
        attackButton.SetActive(isActive);
        waiteButton.SetActive(isActive);
    }
    public void ShowAttackButton(bool isActive)
    {
        attackButton.SetActive(isActive);
    }
    public void ShowWaiteButton(bool isActive)
    {
        waiteButton.SetActive(isActive);
    }
}
