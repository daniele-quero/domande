using System;
using UnityEngine;

[System.Serializable]
public class Domanda
{
    [SerializeField] private string _n;
    [Multiline(7)]
    [SerializeField] private string _quesito;
    [SerializeField] private Risposta[] _risposte = new Risposta[5];

    public string N { get => _n; }
    public string Quesito { get => _quesito; }
    public Risposta[] Risposte { get => _risposte; }

    public Domanda(string n, string quesito, Risposta[] risposte)
    {
        _n = n ?? throw new ArgumentNullException(nameof(n));
        _quesito = quesito ?? throw new ArgumentNullException(nameof(quesito));
        _risposte = risposte ?? throw new ArgumentNullException(nameof(risposte));
    }

    public Domanda Build()
    {
        _quesito = _quesito.Replace("\n", " ");
        DurstenfeldShuffler<Risposta>.shuffle(_risposte);
        for (int i = 0; i < _risposte.Length; i++)
        {
            _risposte[i].Finalize(i);
        }
        return this;
    }
}
