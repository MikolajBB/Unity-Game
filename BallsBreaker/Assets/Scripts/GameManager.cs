using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject BottomPanel;
    public GameObject BallCoordinator;
    public GameObject ChooseLevelPanel;
    private GameObject currentLevel;

	public void StartGame(GameObject level)
    {
        currentLevel = level;
        ChooseLevelPanel.SetActive(false);
        StartLeveL(level);
    }

    void Update()
    {
        if (currentLevel != null)
        {
            if(currentLevel.transform.childCount <= 0)
            {
                Debug.Log("Koniec tej planszy");
            }
        }
    }

    void StartLeveL(GameObject level)
    {
        level.SetActive(true);
        BottomPanel.SetActive(true);
        BallCoordinator.SetActive(true);
        BallCoordinator.GetComponent<BounceScript>().Grid = level;
    }
}
