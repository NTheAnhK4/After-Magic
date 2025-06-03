using UnityEngine;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "View Animation/Dip To Black", fileName = "DipToBlack")]
    public class DipToBlack : ViewAnimation
    {
        public float Duration = 0.5f;
        public float StartAlpha = 0f;
        public float EndAlpha = 1f;

        public override Sequence PlayShowAnimation(UIView view)
        {
            _animation = DOTween.Sequence();
           
            view.CanvasGroup.alpha = StartAlpha;

            _animation.Append(DOVirtual.Float(StartAlpha, EndAlpha, Duration, a => view.CanvasGroup.alpha = a))
                .SetUpdate(true);

            return _animation;
        }

        public override Sequence PlayHideAnimation(UIView view)
        {
            _animation = DOTween.Sequence();

            view.CanvasGroup.alpha = EndAlpha;

            _animation.Append(DOVirtual.Float(EndAlpha, StartAlpha, Duration, a => view.CanvasGroup.alpha = a))
                .SetUpdate(true);

            return _animation;
        }
    }
}