using UnityEngine;
using DG.Tweening;

namespace Game.UI
{
    public abstract class ViewAnimation : ScriptableObject
    {
        protected Sequence _animation;
      
        public abstract Sequence PlayShowAnimation(UIView view);
        public abstract Sequence PlayHideAnimation(UIView view);
    }
}