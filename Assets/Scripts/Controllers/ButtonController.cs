using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {

    public SpriteRenderer sr;
    public Sprite hoverSprite;
    public UnityEvent action;
    public AudioClip clip;
    private Sprite mainSprite;

    private float alpha;


    void Start() {
        mainSprite = sr.sprite;
    }


    void OnMouseEnter() {
        sr.sprite = hoverSprite;
        alpha = sr.color.a;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.99f);
    }


    void OnMouseExit() {
        sr.sprite = mainSprite;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }


    private void OnMouseDown() {
        if (clip != null)
            MainManager.mm.PlaySound(clip);
        action.Invoke();
    }

    public void Action() {
        OnMouseDown();
    }
}
