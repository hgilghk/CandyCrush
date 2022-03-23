using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour {
    public enum ColorTye {
        YELLOW,
        PURPLE,
        RED,
        BLUE,
        GREEN,
        PINK,
        ANY,
        COUNT
    };

    [System.Serializable]
    public struct ColorSprite {
        public ColorTye color;
        public Sprite sprite;
    };

    public ColorSprite[] colorSprites;

    private ColorTye color;

    private ColorTye Color {
        get { return Color; }
        set { SetColor(value); }
    }

    public int NumColors {
        get { return colorSprites.Length; }
    }

    private SpriteRenderer sprite;
    private Dictionary<ColorTye, Sprite> colorSpriteDict;

    private void Awake() {
        sprite = transform.GetComponent<SpriteRenderer>();

        colorSpriteDict = new Dictionary<ColorTye, Sprite>();

        for (int i = 0; i < colorSprites.Length; i++) {
            if (!colorSpriteDict.ContainsKey(colorSprites[i].color)) {
                colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetColor(ColorTye newColor) {
        color = newColor;
        if (colorSpriteDict.ContainsKey(newColor)) {
            sprite.sprite = colorSpriteDict[newColor];
        }
    }
}
