using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeProperties : MonoBehaviour
{
    public ParticleSystem particleEffect;
    private GameObject textOnCube;
    private Image image;
    public int hits;

    void Start()
    {
        textOnCube = transform.Find("Text").gameObject;
        textOnCube.GetComponent<Text>().text = hits.ToString();

        var gameObject = transform.Find("Image").gameObject;
        image = gameObject.GetComponent<Image>();
    }


    void Update()
    {
        if (hits <= 0)
        {
            ParticleSystem explosionEffect = Instantiate(particleEffect);
            explosionEffect.transform.position = transform.position;
            explosionEffect.Play();
            Destroy(gameObject);
        }
    }

    public void DividePoints(int divider)
    {
        this.hits = (int)hits / divider;
        textOnCube.GetComponent<Text>().text = hits.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var color =(Color32) image.color;
        if (collision.gameObject.tag == "Ball" || collision.gameObject.tag == "BallClone")
        {
            hits--;
            var r = color.r + 20 >= 255 ? 10 : color.r + 20;
            var g = color.g + 5 >= 255 ? 10 : color.g + 5;
            var b = color.b + 2 >= 255 ? 50 : color.b + 2;

            image.color = new Color32((byte)r,(byte)g,(byte)b,color.a);
            textOnCube.GetComponent<Text>().text = hits.ToString();
        }
    }
}
