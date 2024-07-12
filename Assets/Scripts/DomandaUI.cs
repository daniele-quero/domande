using UnityEngine;
using UnityEngine.UI;

public class DomandaUI : MonoBehaviour
{
    [SerializeField] private Domanda _domanda;
    [SerializeField] private Text _quesito;
    [SerializeField] private RispostaUI[] _risposteUI;
    [SerializeField] private int _test;
    private int _evtCounter = 0;

    public void Init(Domanda domanda, Transform parent, int test)
    {
        _test = test;
        _domanda = domanda;
        _quesito.text = _domanda.N + " - " + _domanda.Quesito;

        _risposteUI = GetComponentsInChildren<RispostaUI>();
        for (int i = 0; i < _domanda.Risposte.Length; i++)
            _risposteUI[i].Init(_domanda.Risposte[i], NascondiDomanda);

        transform.SetParent(parent);
    }

    private void NascondiDomanda()
    {
        _evtCounter++;
        if (_evtCounter == 5 && _test > 0)
            gameObject.SetActive(false);
    }
}
