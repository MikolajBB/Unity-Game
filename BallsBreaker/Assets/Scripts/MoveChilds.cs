using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChilds : MonoBehaviour {

    public bool canMoveDown = true;
    public float offSet;
    public GameObject Ball;

	void Start () {

    }
	
	void Update () {
    }

    public void MoveDown()
    {
        if(Ball.transform.position.y < transform.position.y)
        {
            if (canMoveDown)
            {
                transform.position = transform.position + new Vector3(0, offSet);
                Debug.Log(transform.position);
                Debug.Log(Ball.transform.position + "Ball");
                //Pozycja najnizszego klocka musi byc sprawdzona czy czasem nie jest za nisko
                canMoveDown = false;
            }
        }   
    }
}
