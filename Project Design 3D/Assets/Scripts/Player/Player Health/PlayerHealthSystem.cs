using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealthSystem : MonoBehaviour
{
    public static int Health = 3;

    public Image[] Hearts;

    void Awake()
    {
        Health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Image heart in Hearts)
        {
            heart.enabled = false;
        }
        for (int i = 0; i < Health; i++)
        {
            Hearts[i].enabled = true;
        }
    }
}
