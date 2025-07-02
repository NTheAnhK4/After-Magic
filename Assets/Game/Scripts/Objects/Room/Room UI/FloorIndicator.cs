using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorIndicator : ComponentBehaviour
{
    [SerializeField] private TextMeshProUGUI inforTxt;
    public override void LoadComponent()
    {
        base.LoadComponent();
        inforTxt = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (InGameManager.Instance != null)
        {
            int curDepth = InGameManager.Instance.CurrentDepth;
            int maxDepth = InGameManager.Instance.MaxDepth;
            inforTxt.text = curDepth.ToString() + "/" + maxDepth.ToString();
        }
    }
}
