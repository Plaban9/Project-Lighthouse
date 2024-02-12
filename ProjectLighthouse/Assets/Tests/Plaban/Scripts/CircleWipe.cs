using DG.Tweening;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace LighthouseGames.SceneUtilities.Transition.Effects
{
    public class CircleWipe : SceneTransition
    {
        [SerializeField] private Image _circle;

        public override IEnumerator AnimateTransitionIn()
        {
            _circle.rectTransform.anchoredPosition = new Vector2(-2250f, 0f);
            var tweener = _circle.rectTransform.DOAnchorPosX(0f, 1f);

            yield return tweener.WaitForCompletion();
        }

        public override IEnumerator AnimateTransitionOut()
        {
            var tweener = _circle.rectTransform.DOAnchorPosX(2250f, 1f);

            yield return tweener.WaitForCompletion();
        }
    }
}
