using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Defines;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIView : ComponentBehavior
    {

        public UIScreen UIScreen;
        public ViewAnimationType ShowAnimation;
        public ViewAnimationType HideAnimation;
        private CanvasGroup canvasGroup;
        public Transform Container;

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (canvasGroup == null)  canvasGroup = GetComponent<CanvasGroup>();
                return canvasGroup;
            }
            set => canvasGroup = value;
        }

        

        protected override void LoadComponent()
        {
            base.LoadComponent();
            canvasGroup = GetComponent<CanvasGroup>();
            Container = transform;
        }

        public virtual UniTask Initialize()
        {
            return UniTask.CompletedTask;
        }

        public virtual async void Show()
        {
            await OnShow();
            CanvasGroup.interactable = true;
            gameObject.SetActive(true);
        }

        protected virtual UniTask OnShow()
        {
            return UniTask.CompletedTask;
        }

        public virtual async void Hide()
        {
            CanvasGroup.interactable = false;
            await OnHide();
            gameObject.SetActive(false);
        }

        protected virtual UniTask OnHide()
        {
            return UniTask.CompletedTask;
        }

        public virtual void OnFinishedShow()
        {
            
        }
    }
}

