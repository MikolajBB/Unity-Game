using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBallScript : MonoBehaviour {

    public GameObject GameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" || collision.gameObject.tag == "BallClone")
        {
            GameManager.GetComponent<GameManager>().AddAdditionalBalls(1);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Stop")
        {
            Destroy(gameObject);
        }
    }
}
