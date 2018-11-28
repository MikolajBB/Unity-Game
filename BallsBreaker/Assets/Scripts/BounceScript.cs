using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    private const float SPEED = 10f;
    private const float TIME_OFFSET = 0.1f / 32;
    private bool firstRun = true;
    public GameObject Ball;
    private List<GameObject> balls;
    public int numberOfBalls;
    private Vector2 clickPosition;
    public Vector2 stopFirstBallPosition = new Vector2(0, 0);

    void Start()
    {
        balls = new List<GameObject>();
        balls.Add(Ball);
        CreateNewBalls();
        IgnoreCollisionBetweenBalls();
    }

    void Update()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].GetComponent<StopBalls>().isFreezed)
            {
                if (stopFirstBallPosition.x == 0 && stopFirstBallPosition.y == 0)
                {
                    stopFirstBallPosition.x = balls[i].GetComponent<Rigidbody2D>().position.x;
                    stopFirstBallPosition.y = balls[i].GetComponent<Rigidbody2D>().position.y;
                }
                else
                {
                    //TODO - odmrozic scalic, zamrozic
                   // Debug.Log("Docelowa pozycja: " + stopFirstBallPosition);
                    balls[i].GetComponent<StopBalls>().DisableConstraints();
                    balls[i].GetComponent<StopBalls>().MoveToPosition(stopFirstBallPosition);
                }

            }

        }


        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(ShootBall(0.1f, touch.position));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            int countStoppedBalls = 0;
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].GetComponent<StopBalls>().isFreezed)
                {
                    countStoppedBalls++;
                }
            }
            Debug.Log("Ilosc zliczonych: " + countStoppedBalls);
            if (countStoppedBalls == balls.Count)
            {
                stopFirstBallPosition = new Vector2(0, 0);

                Debug.Log("Pozycja wyczyszczona" + stopFirstBallPosition);
                StartCoroutine(ShootBall(0.1f, Input.mousePosition));
                for (int j = 0; j < balls.Count; j++)
                {
                    balls[j].GetComponent<StopBalls>().isFreezed = false;
                    balls[j].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                }
            }

        }
    }



    IEnumerator ShootBall(float time, Vector2 position)
    {
        int index = 0;
        while (true)
        {
            yield return new WaitForSeconds(time);
            time += TIME_OFFSET;

            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(position);
            clickPosition.z = balls[index].transform.position.z;
            Vector2 vectorToTest = (clickPosition - balls[index].transform.position).normalized;
            Vector2 newVelocity = (clickPosition - balls[index].transform.position).normalized * SPEED;
            balls[index].GetComponent<Rigidbody2D>().velocity = newVelocity;
            index++;
            if (index == balls.Count) break;
        }
    }

    IEnumerator MoveToStopFirstBallPosition(float time, Vector2 position, int ballIndex)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            balls[ballIndex].GetComponent<Rigidbody2D>().position = Vector2.Lerp(balls[ballIndex].GetComponent<Rigidbody2D>().position, position, time);
            time += 0.1f;
            if (time >= 1f)
            {
                Debug.Log("Git");
                break;
            }

        }

    }

    private void IgnoreCollisionBetweenBalls()
    {
        for (int i = 0; i < balls.Count - 1; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                Physics2D.IgnoreCollision(balls[i].GetComponent<Collider2D>(), balls[j].GetComponent<Collider2D>(), true);
            }
        }
    }

    private void CreateNewBalls()
    {
        for (int i = 1; i < numberOfBalls; i++)
        {
            Vector2 startPosition = Ball.GetComponent<Rigidbody2D>().position;
            GameObject newBall = Instantiate(Ball, transform);
            balls.Add(newBall);
        }
    }




}
