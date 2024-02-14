using System.Collections;

using UnityEngine;

namespace LighthouseGames.SceneUtilities
{
    public abstract class SceneTransition : MonoBehaviour
    {
        public abstract IEnumerator AnimateTransitionIn();
        public abstract IEnumerator AnimateTransitionOut();
    }
}
