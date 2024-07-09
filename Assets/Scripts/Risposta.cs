using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Risposta
{
    public static Dictionary<int, string> indices = new Dictionary<int, string>() { { 0, "A) " }, { 1, "B) " }, { 2, "C) " }, { 3, "D) " }, { 4, "E) " }, };

    [Multiline(2)]
    [SerializeField] private string _testo;
    [SerializeField] private bool _isEsatta;

    public string Testo { get => _testo; }
    public bool IsEsatta { get => _isEsatta; }

    public Risposta(string testo, bool isEsatta)
    {
        _testo = testo ?? throw new ArgumentNullException(nameof(testo));
        if (this._testo.Length > 3)
            this._testo = this._testo.Substring(3);
        _isEsatta = isEsatta;
    }

    public void Finalize(int i)
    {
        this._testo = indices[i] + this._testo;
    }
}
