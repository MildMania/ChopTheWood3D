using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float _gravity;
    public float Gravity
    {
        get
        {
            return _gravity;
        }
    }
}
