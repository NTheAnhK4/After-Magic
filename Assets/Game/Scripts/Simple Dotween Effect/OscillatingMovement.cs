using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OscillatingMovement : MonoBehaviour
{
    public Vector3 StartOffset;
    public Vector3 EndOffset;
    private Vector3 originalPosition;
    public float Speed = 1;
    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * Speed) + 1f) / 2f;
        transform.position = Vector3.Lerp(
            originalPosition + StartOffset,
            originalPosition + EndOffset,
            t
        );
    }
}
