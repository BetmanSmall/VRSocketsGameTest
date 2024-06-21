using System.Collections;
using Mirror;
using UnityEngine;
namespace MirrorExamplesVR.Scripts {
    public class VRNetworkSpawner : NetworkBehaviour {
        [SerializeField] private GameObject objectPrefab;
        [SerializeField] private int maxObjectsToSpawn = 20;
        private int _currentobjectsSpawned;
        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;
        private bool _canSpawn = true;

        [ServerCallback]
        private void Start() {
            _spawnPosition = gameObject.transform.position;
            _spawnRotation = gameObject.transform.rotation;
            SpawnObject();
        }

        [ServerCallback]
        private void OnTriggerExit(Collider otherCollider) {
            if (_canSpawn && _currentobjectsSpawned < maxObjectsToSpawn && objectPrefab && otherCollider.name.Contains("Peg")) {
                SpawnObject();
            }
        }

        [ServerCallback]
        private void SpawnObject() {
            _currentobjectsSpawned++;
            GameObject obj = Instantiate(objectPrefab, _spawnPosition, _spawnRotation);
            NetworkServer.Spawn(obj);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown() {
            _canSpawn = false;
            yield return new WaitForSeconds(0.1f);
            _canSpawn = true;
        }
    }
}