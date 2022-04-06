using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    public int x, y;
    public GridScript.PieceType type;
    public GridScript grid;
    public ColorPiece colorComponent;
    public bool isMovable;

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
        grid.EnterPiece(this);
    }

    private void OnMouseDown() {
        grid.PressPiece(this);
    }

    private void OnMouseUp() {
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
}
