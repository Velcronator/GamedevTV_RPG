using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Start()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            player = GameObject.FindWithTag("Player");

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {   // State where the AI is in range and can attack
                timeSinceLastSawPlayer = 0f;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {   //Suspicion State
                SuspicionBehaviour();
            }
            else
            {   // Back to the guard position
                GaurdBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void GaurdBehaviour()
        {
            mover.StartMoveAction(guardPosition);
            //todo think about rotating the player when he gets back
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //fighter.Cancel();

        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {   // called by unity
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}