using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    public Color red, yellow, blue, green, pink, purple;
    private GamePiece piece;
    // Start is called before the first frame update
    void Start() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite.sprite.name == "YELLOW") sprite.color = yellow;
        if (sprite.sprite.name == "RED") sprite.color = red;
        if (sprite.sprite.name == "BLUE") sprite.color = blue;
        if (sprite.sprite.name == "GREEN") sprite.color = green;
        if (sprite.sprite.name == "PINK") sprite.color = pink;
        if (sprite.sprite.name == "PURPLE") sprite.color = purple;
    }
}
