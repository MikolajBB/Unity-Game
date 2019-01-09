﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    private const float SPEED = 10f;
    private const float TIME_OFFSET = 0.1f / 32;

    public GameObject Ball;
    public GameObject Grid;
    public int numberOfBalls;

    private bool firstRun = true;
    private List<GameObject> balls;
    private Vector2 clickPosition;
    private Vector2 stopFirstBallPosition = new Vector2(0, 0);

    void Start()
    {
        balls = new List<GameObject>();
        balls.Add(Ball);
        CreateNewBalls();
        IgnoreCollisionBetweenBalls();
    }

    void Update()
    {
        if (IsAllBallsStopped() && !firstRun)
        {
            Grid.GetComponent<MoveChilds>().MoveDown();
        }
        MoveBallsToFirstBallStopped();
        OnClickListener();
    }

    private void OnClickListener()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(ShootBall(0.1f, touch.position));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (IsAllBallsStopped())
            {
                stopFirstBallPosition = new Vector2(0, 0);
                StartCoroutine(ShootBall(0.1f, Input.mousePosition));
                for (int j = 0; j < balls.Count; j++)
                {
                    balls[j].GetComponent<StopBalls>().isFreezed = false;
                    firstRun = false;
                    Grid.GetComponent<MoveChilds>().canMoveDown = true;
                }
            }

        }
    }

    public bool IsAllBallsStopped()
    {
        int countStoppedBalls = 0;
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].GetComponent<StopBalls>().isFreezed)
            {
                countStoppedBalls++;
            }
        }
        return countStoppedBalls == balls.Count;
    }

    private void MoveBallsToFirstBallStopped()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (balls[i].GetComponent<StopBalls>().isFreezed)
            {
                if (IsFirstBallStopped())
                {
                    stopFirstBallPosition = balls[i].GetComponent<Rigidbody2D>().position;
                }
                else
                {
                    balls[i].GetComponent<StopBalls>().DisableConstraints();
                    balls[i].GetComponent<StopBalls>().MoveToPosition(stopFirstBallPosition);
                }

            }

        }
    }

    private bool IsFirstBallStopped()
    {
        return stopFirstBallPosition.x == 0 && stopFirstBallPosition.y == 0;
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
