using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//カーソルを移動させたい
public class Cursor : MonoBehaviour
{
    
    public void SetPosition(Transform target)
    {
        transform.position = target.position; 
    }

}
