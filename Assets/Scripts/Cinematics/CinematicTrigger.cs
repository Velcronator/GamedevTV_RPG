using RPG.Control;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
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

        public object CaptureState()
        {
            return cinematicsTriggered;
        }

        public void RestoreState(object state)
        {
            cinematicsTriggered = (bool)state;
        }
    }
}
