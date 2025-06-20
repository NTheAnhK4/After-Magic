
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public class ScaleBtn : ButtonEffectBase
{
   
  

    protected override async UniTask RunEffect()
    {
        Transform transform1;
        (transform1 = transform).DOKill(); 
        transform1.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0.85f, 0.1f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.SetUpdate(true);
        await seq.AsyncWaitForCompletion();
    }
}
