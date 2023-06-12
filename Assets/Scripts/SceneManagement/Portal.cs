using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIndendifier
        {
            A, B, C, D, E, F, G, H
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPointTransform;
        [SerializeField] DestinationIndendifier destination;

        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0) 
            { 
                Debug.LogError("Scene is not set."); 
                yield break; 
            }
            // This portal
            DontDestroyOnLoad(gameObject);
            // find the persistent fader
            Fader fader = FindObjectOfType<Fader>();
            // Fade out
            yield return fader.FadeOut(fadeOutTime);

            // New Scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            // wait a bit them fade in
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPointTransform.position);
            player.transform.rotation = otherPortal.spawnPointTransform.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) { continue; }

                if (portal.destination != destination) { continue; }

                return portal;
            }
            return null;
        }
    }
}