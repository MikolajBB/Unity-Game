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
        Debug.Log(collision);
        if (collision.gameObject.tag == "Stop")
        {
            Debug.Log("test ball: "+transform.GetComponent<Rigidbody2D>().position);
            transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            isFreezed = true;
        }
    }
}
