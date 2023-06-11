using RPG.Control;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool cinematicsTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !cinematicsTriggered)
            {
                GetComponent<PlayableDirector>().Play();
                cinematicsTriggered |= true;
            }
        }
    }
}
