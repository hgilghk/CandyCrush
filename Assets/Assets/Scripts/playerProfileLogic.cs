using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProfileLogic : MonoBehaviour {

    public GameObject grid;
    // Start is called before the first frame update
    private void Awake() {
        grid = GameObject.FindGameObjectWithTag("grid");
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnMouseDown() {
        grid.GetComponent<GridScript>().selectedProfile = gameObject;
    }
}
