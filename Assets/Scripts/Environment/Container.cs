using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour {

    public Door door;
    public Drawer drawer;
    BoxCollider bc;

    static List<Container> containers;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        containers = new List<Container>(FindObjectsOfType<Container>()); 
    }

    public static bool CheckContained(GameObject go)
    {
        foreach (Container c in containers)
        {
            if (c.Contains(go))
            {
                return true;
            }
        }

        return false;
    }

    public bool Contains(GameObject obj)
    {
        bool thingIsOpen = (door && door.isOpen) || (drawer && drawer.isOpen);
        return !thingIsOpen && bc.bounds.Contains(obj.transform.position);
    }
}
