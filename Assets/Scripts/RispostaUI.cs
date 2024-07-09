using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RispostaUI : MonoBehaviour
{
    [SerializeField] private Risposta _risposta;
    private Text _testo;
    private Toggle _toggle;
    private Image _bg;
    public Risposta Risposta { get => _risposta; set => _risposta = value; }

    private void Start()
    {
        _bg = GetComponentInChildren<Image>();
        _testo = GetComponentInChildren<Text>();
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(Rispondi);
    }

    public void Init(Risposta r)
    {
        _risposta = r;
        _testo.text = _risposta.Testo;
    }


    private void Rispondi(bool value)
    {
        if (value)
        {
            if (_risposta.IsEsatta)
            {
                _bg.color = Color.green;
                _toggle.group.GetComponentsInChildren<Toggle>().ToList().ForEach(t => t.interactable = false);
            }
            else
                _bg.color = Color.red;

        }
        else
            _bg.color = Color.white;
    }

}
