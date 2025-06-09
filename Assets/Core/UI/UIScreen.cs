
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

        protected Dictionary<Type, UIView> uiDict = new Dictionary<Type, UIView>();
        public const string UIViewPath = "UI View/";
        protected RectTransform UIViewHolder;
        public Canvas Canvas;
        public T GetUIView<T>() where T : UIView
        {
            Type type = typeof(T);
            if (!uiDict.ContainsKey(type))
            {
                Debug.LogError("Can not find  " + typeof(T).Name);
                return null;
            }

            return (T)uiDict[type];
        }

        protected void AddUIView<T>() where T : UIView
        {
            if (uiDict.ContainsKey(typeof(T))) return;
            Transform holder = transform.parent.Find(UIViewPath + typeof(T).Name);
            if (holder == null)
            {
                Debug.LogWarning("Can not find holder for " + typeof(T).Name);
                return;
            }
            T handler = holder.GetComponent<T>();
            handler.UIScreen = this;
            handler.gameObject.SetActive(false);
            uiDict.Add(typeof(T), handler);
            
        }
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (Canvas == null) Canvas = transform.GetComponentInParent<Canvas>();
            if (panelImage == null)
            {
                panelImage = transform.Find("Panel").GetComponent<Image>();
                Color panelImageColor = panelImage.color;
                panelImageColor.a = 0;
                panelImage.color = panelImageColor;
            }
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            if (UIViewHolder == null)
            {
                UIViewHolder = transform.parent.Find("UI View").GetComponent<RectTransform>();
                if (UIViewHolder == null)
                {
                    UnityEngine.Debug.LogWarning("UI View Holder is null");
                }
            }
        }
     
        public async void ShowUI<T>() where T: UIView
        {
            T view = GetUIView<T>();
            if (view == null)
            {
                Debug.LogWarning(typeof(T) + " is not in resource");
                return;
            }

            Time.timeScale = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            panelImage.DOFade(1f, .3f).SetUpdate(true);
            await ViewAnimationController.PlayShowAnimation(view, view.ShowAnimation);
            uiViewStack.Push(view);
        }

        public async void HideUI<T>(bool skipReset = false, Action afterHide = null) where T : UIView
        {
            T view = GetUIView<T>();
            if (view == null)
            {
                Debug.LogWarning(typeof(T) + " is not in resource");
                return;
            }
            if (uiViewStack.Count > 0)
            {
                UIView viewOnTop = uiViewStack.Pop();
                if (viewOnTop.GetType() != typeof(T))
                {
                    Debug.LogWarning($"[UI Manager] Attempted to hide UI '{view.name}' which is not on top of the stack.");
                    return;
                }
                await  ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide);
                if (uiViewStack.Count == 0 && !skipReset)
                {
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    panelImage.DOFade(0, .3f).SetUpdate(true);


                    Time.timeScale = 1;
                }
            }
        }

        public async void HideUIOnTop(bool skipReset = false, Action afterHide = null)
        {
            if (uiViewStack.Count <= 0)
            {
                Debug.LogWarning("UI on top is empty");
                return;
            }

            UIView view = uiViewStack.Pop();
            await  ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide);
            if (uiViewStack.Count == 0 && !skipReset)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                panelImage.DOFade(0, .3f).SetUpdate(true);


                Time.timeScale = 1;
            }
        }
        public void ShowAfterHide<T>(bool skipReset = false, Action afterHide = null) where T : UIView
        {
            HideUIOnTop(skipReset, afterHide);
            ShowUI<T>();
           
        }


    }
 
}

