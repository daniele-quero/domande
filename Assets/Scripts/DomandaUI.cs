using UnityEngine;
using UnityEngine.UI;

public class DomandaUI : MonoBehaviour
{
    [SerializeField] private Domanda _domanda;
    [SerializeField] private Text _quesito;
    [SerializeField] private RispostaUI[] _risposteUI;
    [SerializeField] private Toggle[] _toggles;
    [SerializeField] private int _n;

    public Domanda Domanda { get => _domanda; set => _domanda = value; }

    void Start()
    {
        _domanda = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
       PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\biologia.pdf")))[_n];
        _quesito.text = _domanda.N + " - " + _domanda.Quesito;

        _risposteUI = GetComponentsInChildren<RispostaUI>();
        for (int i = 0; i < _domanda.Risposte.Length; i++)
        {
            _risposteUI[i].Init(_domanda.Risposte[i]);
        }
    }


}
