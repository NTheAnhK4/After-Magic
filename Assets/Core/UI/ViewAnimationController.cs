using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Defines;

namespace Game.UI
{
	public class ViewAnimationController
	{
		public const string ViewAnimationResource = "ScriptableObject/ViewAnimation/";
		
		private static Dictionary<ViewAnimationType, ViewAnimation> _animationDictionary = new();
		

		private static ViewAnimation GetAnimation(ViewAnimationType type)
		{
			if (!_animationDictionary.ContainsKey(type))
			{
				var handler = Resources.Load<ViewAnimation>(ViewAnimationResource + type);
				_animationDictionary.Add(type, handler);
			}
			return _animationDictionary[type];
		}
		

		public static async UniTask PlayShowAnimation(UIView target, ViewAnimationType type)
		{
			if (target == null)
			{
				Debug.LogError("View Target is null!");
				return;
			}
			target.Show();
			Sequence sequence = GetAnimation(type).PlayShowAnimation(target);
			await UniTask.WaitUntil(() => !sequence.IsActive());

			target.OnFinishedShow();
		}

		public static async UniTask PlayHideAnimation( UIView target, ViewAnimationType type)
		{
			if (target == null)
			{
				Debug.LogError("View Target is null!");
				return;
			}
			Sequence sequence = GetAnimation(type).PlayHideAnimation(target);
			await UniTask.WaitUntil(() => !sequence.IsActive());
			target.Hide();
		}
	}
}