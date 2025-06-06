using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAutoScale : ComponentBehavior
{
    [SerializeField] private SpriteRenderer background;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (background == null) background = GetComponent<SpriteRenderer>();
        
        Scale();
    }

    void Scale()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        Vector3 spriteSize = background.sprite.bounds.size;

        transform.localScale = new Vector3(screenWidth / spriteSize.x, screenHeight / spriteSize.y, 1);
    }
}
