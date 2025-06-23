using System;
using System.Collections.Generic;
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

        public async UniTask ShowPanel(float duration = .3f)
        {
            if(MusicManager.Instance != null) MusicManager.Instance.PauseMusic();
            await panelImage.DOFade(1f, duration).SetUpdate(true).AsyncWaitForCompletion();
        }

        public async UniTask HidePanel(float duration = .3f)
        {
            if(MusicManager.Instance != null) MusicManager.Instance.UnPauseMusic();
            await panelImage.DOFade(0, duration).SetUpdate(true).AsyncWaitForCompletion();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            Time.timeScale = 1;
        }
     
        public async UniTask ShowUI<T>() where T : UIView
        {
           
            T view = GetUIView<T>();
            if (view == null)
            {
                Debug.LogWarning(typeof(T) + " is not in resource");
                return;
            }

            if (uiViewStack.Count > 0 &&  view == uiViewStack.Peek())
            {
                Debug.LogWarning(typeof(T) + " has already been shown");
                return;
            }

            await ShowUIViewInternal(view);
        }

        public async UniTask ShowUI(GameObject uiPrefab)
        {
            if (uiPrefab == null)
            {
                Debug.LogWarning("UI prefab is null");
                return;
            }

            GameObject ui = PoolingManager.Spawn(uiPrefab, UIViewHolder);
            ui.transform.SetAsLastSibling();

            RectTransform rectTransform = ui.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            UIView view = ui.GetComponent<UIView>();
            if (view == null)
            {
                Debug.LogWarning("Spawned UI does not contain UIView component");
                return;
            }

            await ShowUIViewInternal(view);
        }

        private async UniTask ShowUIViewInternal(UIView view)
        {
            view.transform.SetAsLastSibling();

            Time.timeScale = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            await UniTask.WhenAll(
                ShowPanel(),
                ViewAnimationController.PlayShowAnimation(view, view.ShowAnimation)
            );

            uiViewStack.Push(view);
        }

      
        public async UniTask HideUI<T>(bool skipReset = false, Action afterHide = null) where T : UIView
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
            }

           
            if (uiViewStack.Count == 0 && !skipReset)
            {
               

                await UniTask.WhenAll(
                    HidePanel(),
                    ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide)
                );
                   


                Time.timeScale = 1;
            }
            else await  ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide);
        }

        public async UniTask HideUIOnTop(bool skipReset = false, Action afterHide = null)
        {
            if (uiViewStack.Count <= 0)
            {
                Debug.LogWarning("UI on top is empty");
                return;
            }

            UIView view = uiViewStack.Pop();
        
            if (uiViewStack.Count == 0 && !skipReset)
            {
               

                await UniTask.WhenAll(
                    HidePanel(),
                    ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide)
                );
               


                
            }
            else     await  ViewAnimationController.PlayHideAnimation(view, view.HideAnimation, afterHide);
        }
        public async UniTask ShowAfterHide<T>( Action afterHide = null) where T : UIView
        {
            await HideUIOnTop(true, afterHide);
            await ShowUI<T>();
        }
        

    }
 
}

