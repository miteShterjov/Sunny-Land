using System.Collections;
using UnityEngine;

namespace LevelMech
{
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private Transform[] transformWaypoints;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool waitAtWaypoints;
        [SerializeField] private float waitTime = 1f;
        [SerializeField] private Transform platform;

        private Vector3[] waypoints;
        private int currentWaypointIndex; 
        private bool isWaiting;       

        private void Start()
        {
            ConvertTransformsToVectors();
        }

        private void Update()
        {
            if (isWaiting) return;
            PlatformMovement();
        }

        private void PlatformMovement()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Vector3 targetWaypoint = waypoints[currentWaypointIndex];

            if (!platform) return;
            platform.position = Vector3.MoveTowards(platform.position, targetWaypoint, moveSpeed * Time.deltaTime);

            if (!(Vector3.Distance(platform.position, targetWaypoint) < 0.01f)) return;
            if (waitAtWaypoints) StartCoroutine(WaitAtWaypoint());

            currentWaypointIndex++;

            if (currentWaypointIndex < waypoints.Length) return;
            if (loop) currentWaypointIndex = 0;
            else enabled = false;
        }

        private IEnumerator WaitAtWaypoint()
        {
            isWaiting = true; 
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;
        }

        private void ConvertTransformsToVectors()
        {
            waypoints = new Vector3[transformWaypoints.Length];

            int index = 0;
            foreach (Transform waypoint in transformWaypoints)
            {
                if (waypoint == null) continue;
                waypoints[index] = waypoint.position;
                index++;
            }
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null) return;

            Gizmos.color = Color.blue;
            foreach (Vector3 waypoint in waypoints) Gizmos.DrawWireSphere(waypoint, 0.2f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) other.transform.SetParent(platform);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) other.transform.SetParent(null);
        }
    }
}