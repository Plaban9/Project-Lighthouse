namespace Menu.ClickableMenuObjects
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class ClickableMenuObject : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("Function")]
        [SerializeField] public UnityEvent onClickPerform;

        private Material _mat;
        private Vector3 _originalScale;
        void Start()
        {
            _originalScale = transform.localScale;
            _mat = gameObject.GetComponent<Renderer>().material;
            DisableHighlight();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate()
        {
            onClickPerform.Invoke();
        }
        public void Highlight()
        {
            _mat.EnableKeyword("GLOW_ON");
        }

        public void DisableHighlight()
        {
            gameObject.transform.localScale = _originalScale;
            _mat.DisableKeyword("GLOW_ON");

        }
    }

}
