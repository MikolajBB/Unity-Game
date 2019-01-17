using System;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject textMesh;
    private TextMeshProUGUI gemsText;

    public GameObject BottomPanel;
    public GameObject ChooseLevelPanel;
    public GameObject NextLevelPanel;
    public GameObject GameOverPanel;

    public GameObject Parent;

    public GameObject BallCoordinator;

    public GameObject ShieldLeft;
    public GameObject ShieldRight;

    private GameObject currentLevel;
    private GameObject BallCoordinatorCopy;

    private int playerGems;

    public static int SHIELD_VALUE = 30;
    public static int ADD_BALLS_VALUE = 10;
    public static int DIVIDE_VALUE = 50;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Gem"))
        {
            PlayerPrefs.SetInt("Gem", 100);
        }
        playerGems = PlayerPrefs.GetInt("Gem");
        
        BallCoordinatorCopy = Instantiate(BallCoordinator);
        gemsText = textMesh.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (currentLevel != null)
        {
            if (GetAllCubesFromLevel() == 0)
            {
                if (BallCoordinator != null)
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

    public void StartGame(GameObject level)
    {
        currentLevel = level;
        StartLeveL(currentLevel);
    }

    public void NextLevel()
    {

        int nextLvlNum = Int32.Parse(currentLevel.tag) + 1;
        if (FindObject(Parent, nextLvlNum.ToString()) == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            currentLevel = FindObject(Parent, nextLvlNum.ToString());

            ShowAdd();
        }  
    }

    private void StartLeveL(GameObject level)
    {
        NextLevelPanel.SetActive(false);
        ChooseLevelPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        level.SetActive(true);
        BottomPanel.SetActive(true);
        BallCoordinator.SetActive(true);
        BallCoordinator.GetComponent<BounceScript>().Grid = currentLevel;
    }

    public void BackToHome()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        BottomPanel.SetActive(false);
        BallCoordinator.SetActive(false);
        currentLevel.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    #region BONUS
    public void DividePointsOnCubes()
    {
        playerGems = PlayerPrefs.GetInt("Gem", 0);
        if (playerGems >= DIVIDE_VALUE)
        {
            if (currentLevel != null)
            {
                playerGems -= DIVIDE_VALUE;
                PlayerPrefs.SetInt("Gem", playerGems);
                gemsText.SetText(playerGems.ToString());
                Transform[] trs = currentLevel.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trs)
                {
                    if (t.tag == "Cube")
                    {
                        t.GetComponent<CubeProperties>().DividePoints(2);
                    }
                }
            }
        } 
    }

    public void AddBalls(int addBallsNumber)
    {
        playerGems = PlayerPrefs.GetInt("Gem", 0);
        if (playerGems >= ADD_BALLS_VALUE)
        {
            playerGems -= ADD_BALLS_VALUE;
            PlayerPrefs.SetInt("Gem", playerGems);
            gemsText.SetText(playerGems.ToString());
            BallCoordinator.GetComponent<BounceScript>().AddBalls(addBallsNumber);
        }   
    }

    public void AddAdditionalBalls(int numberOfBalls)
    {
        BallCoordinator.GetComponent<BounceScript>().ShouldAddCollectedBalls(numberOfBalls);
    }

    public void ActivateShield()
    {
        playerGems = PlayerPrefs.GetInt("Gem", 0);
        if (playerGems >= SHIELD_VALUE)
        {
            var ball = BallCoordinator.GetComponent<BounceScript>().Ball;
            if (!ShieldRight.activeSelf || !ShieldLeft.activeSelf)
            {
                var leftRectShield = ShieldLeft.GetComponent<RectTransform>();
                var rightRectShield = ShieldRight.GetComponent<RectTransform>();

                if (!RectTransformUtility.RectangleContainsScreenPoint(leftRectShield, ball.transform.position))
                {
                    ShieldLeft.SetActive(true);
                }
                else if (!RectTransformUtility.RectangleContainsScreenPoint(rightRectShield, ball.transform.position))
                {
                    ShieldRight.SetActive(true);
                }
                playerGems -= SHIELD_VALUE;
                PlayerPrefs.SetInt("Gem", playerGems);
                gemsText.SetText(playerGems.ToString());
            }
        }
    }
    #endregion BONUS

    #region ADD
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
                BallCoordinatorCopy = Instantiate(BallCoordinator);
                var add5Gems = PlayerPrefs.GetInt("Gem", 0) + 20;
                PlayerPrefs.SetInt("Gem", add5Gems);
                StartGame(currentLevel);
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                BallCoordinator = BallCoordinatorCopy;
                BallCoordinatorCopy = Instantiate(BallCoordinator);
                StartGame(currentLevel);
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                BallCoordinator = BallCoordinatorCopy;
                BallCoordinatorCopy = Instantiate(BallCoordinator);
                StartGame(currentLevel);
                break;
        }
    }
    #endregion ADD
    private GameObject FindObject(GameObject parent, string tag)
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

    private int GetAllCubesFromLevel()
    {
        int count = 0;
        Transform[] trs = currentLevel.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.tag == "Cube")
            {
                count++;
            }
        }
        return count;
    }
}
