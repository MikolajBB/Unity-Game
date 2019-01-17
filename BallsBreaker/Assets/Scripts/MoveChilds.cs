using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChilds : MonoBehaviour {

    public bool canMoveDown = true;
    public float offSet;
    public int additionalBalls = 0;
    public GameObject BottomPanel;
    public GameObject GameManager;


    private void Start()
    {
        if(additionalBalls > 0)
            GameManager.GetComponent<GameManager>().AddAdditionalBalls(additionalBalls);
    }

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
            if (transform.GetChild(i).tag == "AddBall") break;
            var offSetBottomPanel = BottomPanel.transform.GetComponent<RectTransform>().anchorMax.y;
            if ((transform.GetChild(i).transform.position.y + offSet - 0.5f) <= (BottomPanel.transform.position.y + offSetBottomPanel))
            {
                GameManager.GetComponent<GameManager>().GameOver();
                return false;
            }
        }
        return true;
    }
}
