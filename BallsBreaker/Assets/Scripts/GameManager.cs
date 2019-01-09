﻿using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{

    public GameObject BottomPanel;
    public GameObject BallCoordinator;
    public GameObject ChooseLevelPanel;
    public GameObject NextLevelPanel;
    public GameObject Parent;
    private GameObject currentLevel;
    private GameObject BallCoordinatorCopy;

    public void StartGame(GameObject level)
    {
        currentLevel = level;
        
        StartLeveL(level);
    }

    public void NextLevel()
    {
        int nextLvlNum = Int32.Parse(currentLevel.tag) + 1;
        currentLevel = FindObject(Parent, nextLvlNum.ToString());
        ShowAdd();
    }

    private void ShowAdd()
    {

        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }


    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                BallCoordinator = BallCoordinatorCopy;
                StartGame(currentLevel);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                BallCoordinator = BallCoordinatorCopy;
                StartGame(currentLevel);
                break;
        }
    }

    private void Start()
    {
        BallCoordinatorCopy = Instantiate(BallCoordinator);
    }

    void Update()
    {
        if (currentLevel != null)
        {
            if (currentLevel.transform.childCount <= 0)
            {
                if(BallCoordinator != null)
                {
                    if (BallCoordinator.GetComponent<BounceScript>().IsAllBallsStopped())
                    {
                        currentLevel.SetActive(false);
                        BottomPanel.SetActive(false);
                        BallCoordinator.SetActive(false);
                        DestroyImmediate(BallCoordinator);
                        NextLevelPanel.SetActive(true);
                    }
                }
            }
        }
    }

    void StartLeveL(GameObject level)
    {
        NextLevelPanel.SetActive(false);
        ChooseLevelPanel.SetActive(false);
        level.SetActive(true);
        BottomPanel.SetActive(true);
        BallCoordinator.SetActive(true);
        BallCoordinator.GetComponent<BounceScript>().Grid = level;
    }

    public GameObject FindObject(GameObject parent, string tag)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.tag == tag)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}