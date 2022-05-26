using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileLogic : MonoBehaviour
{
    private GameObject grid;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float speed = 1;
    private float time = 0;
    private float startTime;
    private void Awake() {
        grid = GameObject.FindGameObjectWithTag("grid");
        startPosition = gameObject.transform.position;
        if (grid.GetComponent<GridScript>().selectedProfile != null) endPosition = grid.GetComponent<GridScript>().selectedProfile.transform.position; else Destroy(gameObject);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        time = (Time.time - startTime) * speed;
        Vector3 tempPosition = Vector3.Lerp(startPosition, endPosition, time);
        transform.position = tempPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        print(collision.gameObject.name);
    }
}
