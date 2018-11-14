using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    private const float SPEED = 10f;
    private const float TIME_OFFSET = 0.1f / 32;
    public GameObject Ball;
    private List<GameObject> balls;
    public int numberOfBalls;
    private Vector2 clickPosition;

    void Start()
    {
        balls = new List<GameObject>();
        balls.Add(Ball);
        CreateNewBalls();
        IgnoreCollisionBetweenBalls();
    }

    void Update()
    {
        
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                StartCoroutine(ExecuteAfterTime(0.1f,touch.position));
                //TODO - zapytac sie o stala predkosc
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            int countStoppedBalls = 0;
            Debug.Log(balls[1].GetComponent<StopBalls>().isFreezed);
            for(int i=0; i < balls.Count; i++)
            {
                if (balls[i].GetComponent<StopBalls>().isFreezed)
                {
                    countStoppedBalls++;
                }
            }
            if (countStoppedBalls == balls.Count)
            {

                Debug.Log(Input.mousePosition);
                StartCoroutine(ExecuteAfterTime(0.1f, Input.mousePosition));
                for(int j=0; j < balls.Count; j++)
                {
                    Debug.Log("nie wiem co jest");
                    balls[j].GetComponent<StopBalls>().isFreezed = false;
                    balls[j].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                }
            }
           
        }
    }



    IEnumerator ExecuteAfterTime(float time, Vector2 position)
    {
        int index = 0;
        while (true)
        {
            yield return new WaitForSeconds(time);
            time += TIME_OFFSET;
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(position);
            clickPosition.z = balls[index].transform.position.z;
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
            // GameObject newBall = Instantiate(Ball, Ball.GetComponent<Rigidbody2D>().position, transform.rotation);
            Vector2 startPosition = Ball.GetComponent<Rigidbody2D>().position;
            GameObject newBall = Instantiate(Ball, transform);
            balls.Add(newBall);
        }
    }



    
}
