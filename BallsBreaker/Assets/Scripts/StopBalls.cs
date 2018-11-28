using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBalls : MonoBehaviour {

    public bool isFreezed;

	// Use this for initialization
	void Start () {
        isFreezed = true;
	}
	
	// Update is called once per frame
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
        StartCoroutine(MoveToStopFirstBallPosition(0.1f, 0.005f, targetPosition));
    }

    IEnumerator MoveToStopFirstBallPosition(float time,float speed, Vector2 position)
    {
        bool isNormalized = false;
        float move = 0.1f;
        while (speed < 1f)
        {
            if ((Vector2)transform.position == position) break;
            yield return new WaitForSeconds(time);
            transform.position = Vector2.Lerp(transform.position, position, move);
            move += speed;
            if (speed > 1f && !isNormalized)
            {
                speed = 1f;
                isNormalized = true;
            }
            Debug.Log("Przenoszenie trwa");
        }
        Debug.Log("Przenoszenie powiodło się");
        isFreezed = true;
    }
}
