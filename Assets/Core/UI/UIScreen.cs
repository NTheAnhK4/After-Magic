
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{ 
    [RequireComponent(typeof(CanvasGroup))]
   
    public class UIScreen : Singleton<UIScreen>
    {
        protected Image panelImage;
        private CanvasGroup canvasGroup;

        private Stack<UIView> uiViewStack = new Stack<UIView>();

     
      
        public override void LoadComponent()
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
        protected void InitUI<T>(ref T uiField, Transform uiViewHolder) where T : UIView
        {
            if (uiField != null) return;
        
            uiField = uiViewHolder.GetComponentInChildren<T>();
            if (uiField == null) return;
            uiField.UIScreen = this;
            uiField.gameObject.SetActive(false);
        }
        public async void ShowUI(UIView view)
        {
         
            if (view == null)
            {
                Debug.LogWarning("View is null when show ui");
                return;
            }
            Time.timeScale = 0f;

           
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
           
            
            panelImage.DOFade(1f, .3f).SetUpdate(true);
            
            await ViewAnimationController.PlayShowAnimation(view, view.ShowAnimation);
            uiViewStack.Push(view);
           
        }

        public async void HideUI(UIView view = null, bool skipReset = false, Action afterHide = null)
        {

            if (uiViewStack.Count > 0)
            {
                UIView viewOnTop = uiViewStack.Pop();
                if (view == null) view = viewOnTop;
                else
                {
                    if (view != viewOnTop)
                    {
                        Debug.LogWarning($"[UI Manager] Attempted to hide UI '{view.name}' which is not on top of the stack.");
                        return;
                    }
                }
            }

            if (view != null) await ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide);
            if (uiViewStack.Count == 0 && !skipReset)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                panelImage.DOFade(0, .3f).SetUpdate(true);


                Time.timeScale = 1;
            }

        }



    }
 
}

