using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace LighthouseGames.SceneUtilities
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }

        [SerializeField] private GameObject _transitionContainer;
        [SerializeField] private SceneTransition[] _transitions;


        [SerializeField] private Slider _progressBar;

        void Awake()
        {
            if (Instance == null) // Singleton
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _transitions = _transitionContainer.GetComponentsInChildren<SceneTransition>();
        }

        public void LoadScene(string sceneName, string transitionName)
        {
            StartCoroutine(LoadSceneAsync(sceneName, transitionName));
        }

        private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
        {
            SceneTransition transition = _transitions.First(element => element.name.Equals(transitionName));

            AsyncOperation scene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;

            yield return transition.AnimateTransitionIn();

            _progressBar.gameObject.SetActive(true);

            do
            {
                //_progressBar.value = scene.progress;
                _progressBar.value = Mathf.SmoothStep(_progressBar.value, scene.progress, .05f);
                yield return null;

            } while (scene.progress < .9f || _progressBar.value < 0.875f);
            //} while (scene.progress < 0.9f);
            //} while (_progressBar.value < 0.9f);

            scene.allowSceneActivation = true;
            _progressBar.gameObject.SetActive(false);

            yield return transition.AnimateTransitionOut();
        }
    }
}
