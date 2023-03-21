using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingArray : MonoBehaviour
{
    public bool updateBuilding = false;

    [Header("Building Size")]
    public int size = 1;

    [Header("Prefabs & Sizes")]

    public GameObject prefabBottom;
    public float bottomSize;

    public GameObject prefabMid;
    public float midSize;

    public GameObject prefabTop;


    private Vector3 prefabSize;
    private GameObject[] meshHolder;
    private GameObject instanceHolder;

    [Header("Building Materials")]
    public Material[] materials;

    private int randomMaterialIndex;
    public bool _randomizeMaterialIndex;


    private void Awake()
    {
        instanceHolder = transform.Find("Instances").gameObject;
    }

    private void Update()
    {
        Array();
    }


    private void Array()
    {
        instanceHolder.transform.position = transform.position;
        instanceHolder.transform.parent = transform;

        Vector3 currentPosition = gameObject.transform.position;

        if (updateBuilding)
        {
            GetRandomMaterialIndex();
            if (_randomizeMaterialIndex)
            {
                Renderer bottomRenderer = prefabBottom.GetComponent<Renderer>();
                bottomRenderer.material = materials[randomMaterialIndex];
                Renderer midRenderer = prefabMid.GetComponent<Renderer>();
                midRenderer.material = materials[randomMaterialIndex];
                Renderer topRenderer = prefabTop.GetComponent<Renderer>();
                topRenderer.material = materials[randomMaterialIndex];
            }

            ClearBuilding();

            var _bottom = Instantiate(prefabBottom, currentPosition, gameObject.transform.rotation);
            _bottom.transform.parent = instanceHolder.transform;

            currentPosition.y = currentPosition.y + bottomSize;

            meshHolder = new GameObject[size];


            for (int i = 0; i < size; i++)
            {
                meshHolder[i] = Instantiate(prefabMid, currentPosition, gameObject.transform.rotation);
                meshHolder[i].transform.parent = instanceHolder.transform;

                currentPosition.y = currentPosition.y + midSize;
            }

            var _top = Instantiate(prefabTop, currentPosition, gameObject.transform.rotation);
            _top.transform.parent = instanceHolder.transform;

            updateBuilding = false;
            Debug.Log("Placed Building");
        }

        meshHolder = new GameObject[0];
    }

    private void GetRandomMaterialIndex()
    {
        randomMaterialIndex = Random.Range(0, materials.Length);

        print(randomMaterialIndex);
    }

    private void ClearBuilding()
    {
        Transform[] children = instanceHolder.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child != instanceHolder.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}