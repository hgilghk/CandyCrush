using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {
    public Color red, yellow, blue, green, pink, purple;
    public GameObject redVariant, yellowVariant, blueVariant, greenVariant, orangeVariant;
    // Start is called before the first frame update
    void Start() {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite.sprite.name == "YELLOW") yellowVariant.SetActive(true);
        if (sprite.sprite.name == "RED") redVariant.SetActive(true);
        if (sprite.sprite.name == "BLUE") blueVariant.SetActive(true);
        if (sprite.sprite.name == "GREEN") greenVariant.SetActive(true);
        if (sprite.sprite.name == "PURPLE") orangeVariant.SetActive(true);
    }
}
