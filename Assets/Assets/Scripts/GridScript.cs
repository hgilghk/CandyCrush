using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridScript : MonoBehaviour {
    public int clearedTimes = 0;
    public TextMeshProUGUI text;
    public enum PieceType {
        EMPTY,
        NORMAL,
        BUBBLE,
        COUNT,
    };

    [System.Serializable]
    public struct PiecePrefab {
        public PieceType type;
        public GameObject prefab;
    }

    public GameObject selectedProfile;

    public int xDim;
    public int yDim;
    public float fillTime;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;

    private Dictionary<PieceType, GameObject> piecePrefabDict;
    private GamePiece[,] pieces;

    private GamePiece pressedPiece, enteredPiece;

    // Start is called before the first frame update
    void Start() {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();
        for (int i = 0; i < piecePrefabs.Length; i++) {
            if (!piecePrefabDict.ContainsKey (piecePrefabs[i].type)) {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDim; x++) {
            for (int y = 0; y < yDim; y++) {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }


        pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++) {
            for (int y = 0; y < yDim; y++) {
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }

        }


        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update() {
        text.text = "Damage: " + clearedTimes.ToString();
    }

    public IEnumerator Fill() {
        bool needsRefill = true;

        while (needsRefill) {
            yield return new WaitForSeconds(fillTime);

            while (FillStep()) {
                yield return new WaitForSeconds(fillTime);
            }
            needsRefill = ClearAllValidMatches();
        }


    }

    public bool FillStep() {
        bool movedPiece = false;

        for (int y = yDim-2; y >= 0; y--) {
            for (int x = 0; x < xDim; x++) {
                GamePiece piece = pieces[x, y];
                    if (piece.isMovable) {
                    GamePiece pieceBelow = pieces[x, y + 1];
                    if (pieceBelow.type == PieceType.EMPTY) {
                        Destroy(pieceBelow.gameObject);
                        piece.GetComponent<MovablePiece>().Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }
        for (int x = 0; x < xDim; x++) {
            GamePiece pieceBelow = pieces[x, 0];
            if (pieceBelow.type == PieceType.EMPTY) {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].GetComponent<MovablePiece>().Move(x, 0, fillTime);
                pieces[x, 0].colorComponent.SetColor((ColorPiece.ColorTye)Random.Range(0, pieces[x, 0].colorComponent.NumColors));

                movedPiece = true;
            }
        }
        return movedPiece;
    }

    public Vector2 GetWorldPosition(int x, int y) {
        return new Vector2(transform.position.x - xDim / 2.0f + x, transform.position.y + yDim/2.0f - y);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type) {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);
        return pieces[x, y];
    }

    public bool IsAdjacent(GamePiece piece1, GamePiece piece2) {
        return (piece1.x == piece2.x && (int)Mathf.Abs(piece1.y - piece2.y) == 1)
            || (piece1.y == piece2.y && (int)Mathf.Abs(piece1.x - piece2.x) == 1);
    }

    public void SwapPieces(GamePiece piece1, GamePiece piece2) {
        if (piece1.isMovable && piece2.isMovable) {
            pieces[piece1.x, piece1.y] = piece2;
            pieces[piece2.x, piece2.y] = piece1;

            if (GetMatch(piece1, piece2.x, piece2.y) != null || GetMatch (piece2, piece1.x, piece1.y) != null) {

            int piece1X = piece1.x;
            int piece1Y = piece1.y;

            piece1.GetComponent<MovablePiece>().Move(piece2.x, piece2.y, fillTime);
            piece2.GetComponent<MovablePiece>().Move(piece1X, piece1Y, fillTime);
            clearedTimes = 0;
            ClearAllValidMatches();
            StartCoroutine(Fill());
            } else {
                pieces[piece1.x, piece1.y] = piece1;
                pieces[piece2.x, piece2.y] = piece2;
            }
        }
    }

    public void PressPiece(GamePiece piece) {
        pressedPiece = piece;
    }

    public void EnterPiece(GamePiece piece) {
        enteredPiece = piece;
    }

    public void ReleasePiece() {
        if (IsAdjacent(pressedPiece, enteredPiece)) {
            SwapPieces(pressedPiece, enteredPiece);
        }
    }

    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY) {
        if (piece.IsColored()) {
            ColorPiece.ColorTye color = piece.colorComponent.color;
            List<GamePiece> horizontalPieces = new List<GamePiece>();
            List<GamePiece> verticalPieces = new List<GamePiece>();
            List<GamePiece> matchPieces = new List<GamePiece>();

            // first check horizontal
            horizontalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++) {
                for (int xOffset = 1; xOffset < xDim; xOffset++) {
                    int x;

                    if (dir == 0) { // left
                        x = newX - xOffset;
                    } else { // right
                        x = newX + xOffset;
                    }

                    if (x < 0 || x >= xDim) {
                        break;
                    }

                    if (pieces[x, newY].IsColored() && pieces[x, newY].colorComponent.color == color) {
                        horizontalPieces.Add(pieces[x, newY]);
                    } else {
                        break;
                    }
                }
            }
            
            if (horizontalPieces.Count >= 3) {
                for (int i = 0; i < horizontalPieces.Count; i++) {
                    matchPieces.Add(horizontalPieces[i]);
                }
            }

            if (matchPieces.Count >= 3) {
                return matchPieces;
            }

            // Ddind't find anything going horizontally first,
            // So now check vertically
            verticalPieces.Add(piece);

            for (int dir = 0; dir <= 1; dir++) {
                for (int yOffset = 1; yOffset < yDim; yOffset++) {
                    int y;

                    if (dir == 0) { // Up
                        y = newY - yOffset;
                    } else { // Down
                        y = newY + yOffset;
                    }

                    if (y < 0 || y >= yDim) {
                        break;
                    }

                    if (pieces[newX, y].IsColored() && pieces[newX, y].colorComponent.color == color) {
                        verticalPieces.Add(pieces[newX, y]);
                    } else {
                        break;
                    }
                }
            }

            if (verticalPieces.Count >= 3) {
                for (int i = 0; i < verticalPieces.Count; i++) {
                    matchPieces.Add(verticalPieces[i]);
                }
            }

            if (matchPieces.Count >= 3) {
                return matchPieces;
            }
        }
        return null;
    }

    public bool ClearAllValidMatches () {
        bool needsRefill = false;

        for (int y = 0; y <yDim; y++) {
            for (int x = 0; x < xDim; x++) {
                if (pieces[x, y].IsClearable()) {
                    List<GamePiece> match = GetMatch(pieces[x, y], x, y);

                    if (match != null) {
                        for (int i = 0; i < match.Count; i++) {
                            if (ClearPiece (match[i].x, match[i].y)) {
                                needsRefill = true;
                            }
                        }
                    }
                }
            }
        }
        return needsRefill;
    }

    public bool ClearPiece(int x, int y) {
        if (pieces[x, y].IsClearable() && !pieces[x, y].ClearableComponent.IsBeingCleared) {
            pieces[x, y].ClearableComponent.Clear();
            SpawnNewPiece(x, y, PieceType.EMPTY);
            clearedTimes++;

            return true;
        } else return false;
    }
}
