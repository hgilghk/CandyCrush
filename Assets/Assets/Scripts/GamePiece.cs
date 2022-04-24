using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    public int x, y;
    public GridScript.PieceType type;
    public GridScript grid;
    public ColorPiece colorComponent;
    public bool isMovable;
    public GameObject missile;
    public bool hasFired = false;

    private ClearablePiece clearableComponent;

    public ClearablePiece ClearableComponent {
        get { return clearableComponent; }
    }

    private void Awake() {
        colorComponent = GetComponent<ColorPiece>();
        clearableComponent = GetComponent<ClearablePiece>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }
    
    private void OnMouseEnter() {
        if (grid.selectedProfile == null) {
            print("No profile selected");
            return;
        }
        grid.EnterPiece(this);
    }

    private void OnMouseDown() {
        if (grid.selectedProfile == null) {
            print("No profile selected");
            return;
        }
        grid.PressPiece(this);
    }

    private void OnMouseUp() {
        if (grid.selectedProfile == null) {
            print("No profile selected");
            return;
        }
        grid.ReleasePiece();   
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Init(int _x, int _y, GridScript _grid, GridScript.PieceType _type) {
        x = _x;
        y = _y;
        grid = _grid;
        type = _type;
    }

    public bool IsColored() {
        return colorComponent != null;
    }

    public bool IsClearable() {
        return clearableComponent != null;
    }

    private void OnDestroy() {
        FireMissile();
    }

    public void Fire() {
        FireMissile();
    }

    private void FireMissile() {
        if (gameObject.tag != "normalPiece" || hasFired == true) return;
        Vector3 fuitPosition;
        fuitPosition = transform.position;
        hasFired = true;
        Instantiate(missile, fuitPosition, Quaternion.identity);
    }
}

