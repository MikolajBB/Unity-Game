﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBalls : MonoBehaviour {

    public bool isFreezed;
    public bool isStoppedNextToFirstBall = true;

	void Start () {
        isFreezed = true;
	}
	
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stop")
        {
            EnableConstraints();
            isFreezed = true;
        }
    }

    public void DisableConstraints()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        transform.GetComponent<Rigidbody2D>().isKinematic = false;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetComponent<Rigidbody2D>().angularVelocity = 0f;
    }

    public void EnableConstraints()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        transform.GetComponent<Rigidbody2D>().isKinematic = true;

    }

    public void MoveToPosition(Vector2 targetPosition)
    {
        if(this.gameObject.activeSelf)
            StartCoroutine(MoveToStopFirstBallPosition(0.1f, 0.005f, targetPosition));
    }

    IEnumerator MoveToStopFirstBallPosition(float time,float speed, Vector2 position)
    {
        bool isNormalized = false;
        float move = 0.1f;
        while (speed < 1f)
        {
            if ((Vector2)transform.position == position)
            {
                isFreezed = true;
                isStoppedNextToFirstBall = true;
                break;
            } 
            yield return new WaitForSeconds(time);
            transform.position = Vector2.Lerp(transform.position, position, move);
            move += speed;
            if (speed > 1f && !isNormalized)
            {
                speed = 1f;
                isNormalized = true;
            }
        }
    }
}
