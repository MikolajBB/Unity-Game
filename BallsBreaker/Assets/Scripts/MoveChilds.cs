using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChilds : MonoBehaviour {

    public bool canMoveDown = true;
    public float offSet;
    public GameObject Ball;
    public GameObject BottomPanel;
    public GameObject GameOver;

    public void MoveDown()
    {
        if (CheckIfChildIsAboveBottomPanel())
        {
            if (canMoveDown)
            {
                transform.position = transform.position + new Vector3(0, offSet);
                canMoveDown = false;
            }
        }   
    }

    private bool CheckIfChildIsAboveBottomPanel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
        
            var offSetBottomPanel = BottomPanel.transform.GetComponent<RectTransform>().anchorMax.y;
            if ((transform.GetChild(i).transform.position.y + offSet) <= (BottomPanel.transform.position.y + offSetBottomPanel))
            {
                GameOver.SetActive(true);
                return false;
            }
        }
        return true;
    }
}
