using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject prefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (prefab == null) return;
            
            GameObject instance =  PoolingManager.Spawn(prefab, transform);
            //instance.transform.localScale = prefab.transform.localScale;

            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            // Đảm bảo stretch full màn hình
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
