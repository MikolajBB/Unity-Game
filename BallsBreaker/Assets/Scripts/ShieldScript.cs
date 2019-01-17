using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldScript : MonoBehaviour {

    public Image image;

    public float timeAmt = 10;
    float time;

    void Start()
    {
        time = timeAmt;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            image.fillAmount = time / timeAmt;
        }
        else
        {
            gameObject.SetActive(false);
            time = timeAmt;
        }
    }
}
