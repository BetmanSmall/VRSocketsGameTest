using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
namespace _Game.SocketInteractors.Scripts {
    [RequireComponent(typeof(XRGridSocketInteractor))]
    public class AddToGridRandomPeg : NetworkBehaviour {
        [SerializeField] private XRGridSocketInteractor xrGridSocketInteractor;
        [SerializeField] private float percentPegAdd = 0.5f;
        [SerializeField] private GameObject[] pegsPrefabs;
        [SerializeField] private List<GameObject> pegsGenerated;

        [ServerCallback]
        private void Start() {
            xrGridSocketInteractor = GetComponent<XRGridSocketInteractor>();
            if (xrGridSocketInteractor && xrGridSocketInteractor.Grid != null) {
                foreach (Transform transformCell in xrGridSocketInteractor.Grid) {
                    if (Random.Range(0f, 1f) >= percentPegAdd) {
                        int prefabIndex = Random.Range(0, pegsPrefabs.Length);
                        GameObject pegGameObject = Instantiate(pegsPrefabs[prefabIndex], transformCell.position, Quaternion.identity);
                        pegsGenerated.Add(pegGameObject);
                        NetworkServer.Spawn(pegGameObject);
                    }
                }
            }
        }
    }
}