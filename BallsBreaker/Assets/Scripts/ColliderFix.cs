using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFix : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball" || collision.gameObject.tag == "BallClone")
        {
            var position = collision.gameObject.GetComponent<Rigidbody2D>().transform.position;
            collision.gameObject.GetComponent<Rigidbody2D>().MovePosition(new Vector2(position.x + 1,position.y+0.2f));
        }
    }
}
