using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Item itemType;
    [SerializeField] private float glowTime;
    [SerializeField] private float highlightTime;
    [SerializeField] private GameObject glow;

    public Item ItemType => itemType;

    private Renderer _renderer;
    private float _timer;
    private float _highlightTimer;
    private static readonly int FresnelPower = Shader.PropertyToID("_FresnelPower");

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        glow.SetActive(_timer > 0);
        _timer -= Time.deltaTime;
        _highlightTimer -= Time.deltaTime;
        _renderer.materials[1].SetFloat(FresnelPower, _highlightTimer * 3);
    }

    public void ResetGlowTimer()
    {
        _timer = glowTime;
    }

    public void Highlight()
    {
        _highlightTimer = highlightTime;
    }
}

public enum Item
{
    Key,
    Candle,
    Photo
}