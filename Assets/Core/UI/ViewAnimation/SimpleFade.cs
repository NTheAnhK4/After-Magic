using UnityEngine;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "View Animation/Simple Fade", fileName = nameof(ViewAnimationType.SimpleFade))]
    public class SimpleFade : ViewAnimation
    {
       

        public override Sequence PlayShowAnimation(UIView view)
        {
            _animation = DOTween.Sequence();

            view.CanvasGroup.alpha = 0f;

            Tween fadeIn = DOVirtual.Float(0, 1, 0.5f, a => view.CanvasGroup.alpha = a)
                .SetUpdate(true);
            _animation.Append(fadeIn);

            return _animation;
        }

        public override Sequence PlayHideAnimation(UIView view)
        {
            _animation = DOTween.Sequence();

            view.CanvasGroup.alpha = 1f;
            Tween fadeOut = DOVirtual.Float(1, 0, 0.5f, a => view.CanvasGroup.alpha = a)
                .SetUpdate(true);
            _animation.Append(fadeOut);

            return _animation;
        }
    }
}