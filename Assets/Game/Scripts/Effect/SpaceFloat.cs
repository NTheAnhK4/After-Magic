using DG.Tweening;
using UnityEngine;

public class SpaceFloat : MonoBehaviour
{
    [Header("Lơ lửng")]
    public float floatDistance = 0.5f;
    public float floatDuration = 2f;

    [Header("Xoay")]
    public Vector3 rotationSpeed = new Vector3(0, 10, 0);

    void Start()
    {
        
        transform.DOMoveY(transform.position.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

       
        DOTween.To(() => transform.eulerAngles, 
                x => transform.eulerAngles = x, 
                rotationSpeed, 360f / rotationSpeed.y)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}