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
            DontDestroyOnLoad(gameObject);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

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