using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    public int x, y;
    public GridScript.PieceType type;
    public GridScript grid;
    public ColorPiece colorComponent;
    public bool isMovable;

    private void Awake() {
        colorComponent = GetComponent<ColorPiece>();
    }

    // Start is called before the first frame update
    void Start() {
        
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
}
