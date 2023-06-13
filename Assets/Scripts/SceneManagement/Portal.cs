using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Saving;

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

            // Save Current Level
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();

            // New Scene
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Load Current Level
            savingWrapper.Load();

            // Move player to new position
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            // Another save to eliminate portal swapping
            savingWrapper.Save();

            // wait a bit them fade in
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPointTransform.position);
            player.transform.rotation = otherPortal.spawnPointTransform.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
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