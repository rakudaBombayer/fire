using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj : MonoBehaviour
{
    public Vector2Int positionInt;

    [SerializeField] GameObject movablePanel;

    public void ShowMovablePanel(bool isActive)
    {
        movablePanel.SetActive(isActive);
    }
}

// TODO:地形に依存した移動範囲の表示
// ・地形の移動コストをそれぞれのタイルに設定する
// ・移動コストに基づいて、移動範囲を計算する