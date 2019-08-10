﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {

    public SpriteRenderer sr;
    public Sprite hoverSprite;
    public UnityEvent action;
    public AudioClip clip;
    private Sprite mainSprite;


    void Start() {
        mainSprite = sr.sprite;
    }


    void OnMouseEnter() {
        sr.sprite = hoverSprite;
    }


    void OnMouseExit() {
        sr.sprite = mainSprite;
    }


    private void OnMouseDown() {
        if (clip != null)
            GameObject.Find("MainManager").GetComponent<MainManager>().PlaySound(clip);
        action.Invoke();
    }
}
