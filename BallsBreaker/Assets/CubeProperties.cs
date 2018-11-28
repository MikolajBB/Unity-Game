using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeProperties : MonoBehaviour {

    public GameObject Cube;
    public int hits = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(hits == 0)
        {
            Destroy(Cube);
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            hits--;
        }
    }
}
