using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup = null;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        //public IEnumerator FadeOutIn()
        //{
        //    yield return FadeOut(3f);
        //    print("out");
        //    yield return FadeIn(3f);
        //    print("in");
        //}

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}