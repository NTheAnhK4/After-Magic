
using System;
using UnityEngine;


public class OscillatingMovement : MonoBehaviour
{
    public Vector3 StartOffset;
    public Vector3 EndOffset;
    private Vector3 originalPosition;
    public float Speed = 1;

    private bool isInitialized;
    

    public void Initialized()
    {
        originalPosition = transform.position;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;
        float t = (Mathf.Sin(Time.time * Speed) + 1f) / 2f;
        transform.position = Vector3.Lerp(
            originalPosition + StartOffset,
            originalPosition + EndOffset,
            t
        );
    }

    private void OnDisable()
    {
        transform.position = originalPosition;
        isInitialized = false;
    }
}
