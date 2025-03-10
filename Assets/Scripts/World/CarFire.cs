using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFire : MonoBehaviour
{
    public GameObject fire;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.value < 0.3f)
        {
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
        }
    }
}
