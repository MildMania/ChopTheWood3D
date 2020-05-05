using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallBehaviour : MonoBehaviour
{
    [SerializeField] private float _gravity;

    private IEnumerator _freeFallRoutine;

    public void StartFreeFall()
    {
        _freeFallRoutine = FreeFallProgress();
        StartCoroutine(_freeFallRoutine);
    }

    public void StopFreeFall()
    {
        if (_freeFallRoutine != null)
            StopCoroutine(_freeFallRoutine);
    }

    private IEnumerator FreeFallProgress()
    {
        Vector3 velocity = Vector3.zero;

        while(true)
        {
            velocity += -Vector3.up * _gravity * Time.unscaledDeltaTime;

            Vector3 newPos = transform.position;

            newPos += velocity * Time.unscaledDeltaTime;

            transform.position = newPos;

            yield return null;
        }
    }
}
