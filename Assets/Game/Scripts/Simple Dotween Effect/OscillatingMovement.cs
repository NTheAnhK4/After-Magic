
using UnityEngine;


public class OscillatingMovement : MonoBehaviour
{
    public Vector3 StartOffset;
    public Vector3 EndOffset;
    private Vector3 originalPosition;
    public float Speed = 1;
   
    

    public void Initialized()
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
