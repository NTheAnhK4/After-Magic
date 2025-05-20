
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : ComponentBehavior
    {
        private Image panelImage;
        private CanvasGroup canvasGroup;

    

        protected override void LoadComponent()
        {
            base.LoadComponent();
            if (panelImage == null)
            {
                panelImage = transform.Find("Panel").GetComponent<Image>();
                Color panelImageColor = panelImage.color;
                panelImageColor.a = 0;
                panelImage.color = panelImageColor;
            }
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        public async void ShowUI(UIView view)
        {
            Time.timeScale = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = true;
        
            panelImage.DOFade(1f, 1).SetUpdate(true);
            await ViewAnimationController.PlayShowAnimation(view, view.ShowAnimation);
        }

        public async void HideUI(UIView view)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = false;
        
            panelImage.DOFade(0, 1).SetUpdate(true);
            await ViewAnimationController.PlayHideAnimation(view, view.HideAnimation);
            Time.timeScale = 1;
        }
    
    }
}

