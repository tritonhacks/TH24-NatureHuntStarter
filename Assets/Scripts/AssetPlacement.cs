using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPlacement : MonoBehaviour {
    [System.Serializable]
    public class NatureAsset {
        public string name;
        [SerializeField] public GameObject[] prefabs;
        [SerializeField] public int amount;
        [SerializeField] public Vector2 scaleRange;
        [SerializeField] public Vector2 heightRange;
        [SerializeField] public float groundingOffset; // amount to be "shoved" into the ground, mainly for trees
    }

    public NatureAsset[] assets;
    public GameObject water;
    public int xBound = 100;
    public int yBound = 100;
    public int globalScale = 1;
    private Vector3 meshSize;

    void Start() {
        PlaceAssets();
    }

    public void PlaceAssets() {
        meshSize = new Vector3(xBound, 100, yBound);

        for (int i = 0; i < assets.Length; i++) {
            NatureAsset currentAsset = assets[i];
            GameObject parent = new GameObject(currentAsset.name + " Container");

            for (int j = 0; j < currentAsset.amount; j++) {
                bool placed = false;

                while (!placed) {
                    Vector3 randomPos = new Vector3(Random.Range(-meshSize.x, meshSize.x), 0, Random.Range(-meshSize.z, meshSize.z));

                    RaycastHit hit;
                    if (Physics.Raycast(new Vector3(randomPos.x, 1000, randomPos.z), Vector3.down, out hit, Mathf.Infinity)) {
                        if (hit.point.y > currentAsset.heightRange.x && hit.point.y < currentAsset.heightRange.y && hit.collider.gameObject != water) {
                            Vector3 placementPos = new Vector3(randomPos.x, hit.point.y - currentAsset.groundingOffset, randomPos.z);
                            Quaternion placementRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            GameObject placementPrefab = currentAsset.prefabs[Random.Range(0, currentAsset.prefabs.Length)];

                            GameObject clone = Instantiate(placementPrefab, placementPos, placementRot, parent.transform);
                            clone.transform.localScale *= globalScale * Random.Range(currentAsset.scaleRange.x, currentAsset.scaleRange.y);
                            placed = true;
                        }
                    }
                }
            }
        }
    }
}
