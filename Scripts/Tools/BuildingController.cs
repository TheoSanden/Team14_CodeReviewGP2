using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingController : MonoBehaviour
{
    public bool updateBuilding = false;
    public bool randomizeBuildingColor = false;
    public bool randomizeBuildingHeight = false;
    public int minHeight = 2;
    public int maxHeight = 7;

    private void Awake()
    {
        BuildingArray[] buildingArrayObjects = FindObjectsOfType<BuildingArray>();
    }

    private void Update()
    {
        UpdateAllBuildings();
        RandomizeBuildingsHeights();
        RandomizeBuildingsColor();
    }

    private void UpdateAllBuildings()
    {
        if (updateBuilding)
        {
            BuildingArray[] buildingArrayObjects = FindObjectsOfType<BuildingArray>();

            for (int i = 0; i < buildingArrayObjects.Length; i++)
            {
                buildingArrayObjects[i].updateBuilding = true;
            }

            updateBuilding = false;
        }
    }


    private void RandomizeBuildingsHeights()
    {
        if (randomizeBuildingHeight)
        {
            BuildingArray[] buildingArrayObjects = FindObjectsOfType<BuildingArray>();

            for (int i = 0; i < buildingArrayObjects.Length; i++)
            {
                buildingArrayObjects[i].updateBuilding = true;
                buildingArrayObjects[i].size = Random.Range(minHeight, maxHeight);

            }

            randomizeBuildingHeight = false;
        }
    }

    private void RandomizeBuildingsColor()
    {
        if (randomizeBuildingColor)
        {
            BuildingArray[] buildingArrayObjects = FindObjectsOfType<BuildingArray>();

            for (int i = 0; i < buildingArrayObjects.Length; i++)
            {
                buildingArrayObjects[i]._randomizeMaterialIndex = true;

            }
        }
        else
        {
            BuildingArray[] buildingArrayObjects = FindObjectsOfType<BuildingArray>();

            for (int i = 0; i < buildingArrayObjects.Length; i++)
            {
                buildingArrayObjects[i]._randomizeMaterialIndex = false;

            }
        }
    }
}
