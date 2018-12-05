using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeProperties : MonoBehaviour
{
    public GameObject Cube;
    private GameObject textOnCube;
    public int hits;

    void Start()
    {
        textOnCube = transform.Find("Text").gameObject;
        textOnCube.GetComponent<Text>().text = hits.ToString();
    }

    void Update()
    {
        if (hits == 0)
        {
            Destroy(Cube);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            hits--;
            textOnCube.GetComponent<Text>().text = hits.ToString();
        }
    }
}
