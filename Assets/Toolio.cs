using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolio : MonoBehaviour
{
    public bool waterSelected;
    public bool treeSelected;
    public bool humanSelected;
    public bool creatureSelected;

    [SerializeField]
    GameObject waterPrefab;
    [SerializeField]
    GameObject treePrefab;
    [SerializeField]
    GameObject humanPrefab;
    [SerializeField]
    GameObject creaturePrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {

                if (waterSelected)
                    CreateObjectAtPoint(waterPrefab, hit.point);
                if (treeSelected)
                    CreateObjectAtPoint(treePrefab, hit.point);
                if (humanSelected)
                    CreateObjectAtPoint(humanPrefab, hit.point);
                if (creatureSelected)
                    CreateObjectAtPoint(creaturePrefab, hit.point);

            }
        }
    }

    void CreateObjectAtPoint(GameObject prefab, Vector3 location)
	{
        GameObject created = Instantiate(prefab);
        created.transform.position = location;
	}

    public void SetWater()
	{
        waterSelected = true;
        treeSelected = false;
        humanSelected = false;
        creatureSelected = false;
	}
    public void SetTree()
	{
        waterSelected = false;
        treeSelected = true;
        humanSelected = false;
        creatureSelected = false;
	}
    public void SetHuman()
	{
        waterSelected = false;
        treeSelected = false;
        humanSelected = true;
        creatureSelected = false;
	}
    public void SetCreature()
	{
        waterSelected = false;
        treeSelected = false;
        humanSelected = false;
        creatureSelected = true;
	}
}
