using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableController : MonoBehaviour
{
    [SerializeField] private Choppable[] _choppables;

    public Choppable[] Choppables
    {
        get
        {
            return _choppables;
        }
    }

}
