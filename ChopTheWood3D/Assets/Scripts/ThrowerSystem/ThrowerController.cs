using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerController : MonoBehaviour
{
    [SerializeField] [Range(10, 50)] private int _resolution;

    [SerializeField] private Thrower[] _throwers;
    public Thrower[] Throwers
    {
        get
        {
            return _throwers;
        }
    }

    [SerializeField] private float _showSimulation;
    public float ShowSimulation
    {
        get
        {
            return _showSimulation;
        }
    }

}
