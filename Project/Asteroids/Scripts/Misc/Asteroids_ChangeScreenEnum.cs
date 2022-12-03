using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids_ChangeScreenEnum : MonoBehaviour
{

    [SerializeField]
    private UIScreen NewScreen;

    public void ChangeScreen()
    {
        Asteroids_UIManager.Instance.ChangeScreen(NewScreen);
    }
}