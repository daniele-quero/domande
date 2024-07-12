using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DomandeManager : MonoBehaviour
{

    [SerializeField] private int _numberOfTests;
    private int _total;
    [SerializeField] private int _currentTest;

    [SerializeField] private GameObject _domandaTemplate;
    [SerializeField] private Transform _scrollableContent;
    [SerializeField] private Slider _testSelector;
    [SerializeField] private Image _cover;
    [SerializeField] private Image _loadingBar;

    private List<List<Domanda>> _biologiaSub = new List<List<Domanda>>();
    private List<List<Domanda>> _logicaSub = new List<List<Domanda>>();
    private List<List<Domanda>> _competenzeSub = new List<List<Domanda>>();
    private List<List<Domanda>> _chimicaSub = new List<List<Domanda>>();
    private Dictionary<int, List<GameObject>> _testsUI = new Dictionary<int, List<GameObject>>();


    public static Action<int> TestCaricato;
    public int CurrentTest { get => _currentTest; }

    void Start()
    {
        FindBestDomandeDistribution();
        FillTests();

        _testSelector.onValueChanged.AddListener(LoadTest);
        _testSelector.maxValue = _numberOfTests;
        _testSelector.minValue = 1;

    }

    private void FillTests()
    {
        for (int i = 0; i < _numberOfTests; i++)
        {
            var test = new List<Domanda>();
            test.AddRange(_biologiaSub[i]);
            test.AddRange(_logicaSub[i]);
            test.AddRange(_competenzeSub[i]);
            test.AddRange(_chimicaSub[i]);
            DurstenfeldShuffler<Domanda>.shuffle(test);
            _testsUI.Add(i, test.Select(d => InitDomanda(d, i)).ToList());
        }
        _currentTest = 0;
    }

    private GameObject InitDomanda(Domanda d, int test)
    {
        var domandObj = Instantiate(_domandaTemplate, GameObject.Find("Canvas").transform);

        domandObj.GetComponent<DomandaUI>().Init(d, _scrollableContent, test);
        StartCoroutine(Progress());

        return domandObj;
    }

    private IEnumerator Progress()
    {
        _loadingBar.fillAmount += 1f / _total;
        yield return new WaitForEndOfFrame();
        if (_loadingBar.fillAmount == 1f)
        {
            _loadingBar.enabled = false;
            _cover.enabled = false;
        }
    }

    private void FindBestDomandeDistribution()
    {
        List<Domanda> domandeBiologia;
        List<Domanda> domandeCompetenze;
        List<Domanda> domandeLogica;
        List<Domanda> domandeChimica;

        domandeBiologia = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\biologia.pdf")));

        domandeCompetenze = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\competenze.pdf")));

        domandeLogica = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\logica.pdf")));

        domandeChimica = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\chimica.pdf")));

        _total = domandeBiologia.Count + domandeCompetenze.Count + domandeLogica.Count;

        int _numBiologia;
        int _numCompetenze;
        int _numLogica;
        int _numChimica;

        for (_numberOfTests = 22; _numberOfTests <= 60; _numberOfTests++)
        {
            _numLogica = Mathf.RoundToInt(domandeLogica.Count / (float)_numberOfTests);
            _numBiologia = Mathf.RoundToInt(domandeBiologia.Count / (float)_numberOfTests);
            _numCompetenze = Mathf.RoundToInt(domandeCompetenze.Count / (float)_numberOfTests);
            _numChimica = Mathf.RoundToInt(domandeChimica.Count / (float)_numberOfTests);

            _biologiaSub = domandeBiologia.Select((x, i) => new { Index = i, Value = x })
                   .GroupBy(x => x.Index / _numBiologia)
                   .Select(x => x.Select(v => v.Value).ToList())
                   .ToList();

            _logicaSub = domandeLogica.Select((x, i) => new { Index = i, Value = x })
              .GroupBy(x => x.Index / _numLogica)
              .Select(x => x.Select(v => v.Value).ToList())
              .ToList();

            _competenzeSub = domandeCompetenze.Select((x, i) => new { Index = i, Value = x })
               .GroupBy(x => x.Index / _numCompetenze)
               .Select(x => x.Select(v => v.Value).ToList())
               .ToList();

            _chimicaSub = domandeChimica.Select((x, i) => new { Index = i, Value = x })
               .GroupBy(x => x.Index / _numChimica)
               .Select(x => x.Select(v => v.Value).ToList())
               .ToList();

            Debug.Log(string.Format("logica: {0}, biologia: {1}, competenze: {2}, chimica: {3}", _logicaSub.Count, _biologiaSub.Count, _competenzeSub.Count, _chimicaSub.Count));
            if (BestDistrib())
                break;
        }

        _numberOfTests = Mathf.Min(new int[] { _biologiaSub.Count, _competenzeSub.Count, _logicaSub.Count, _chimicaSub.Count });
    }

    private bool BestDistrib() => Mathf.Max(new int[] { _biologiaSub.Count, _competenzeSub.Count, _logicaSub.Count, _chimicaSub.Count })
        - Mathf.Min(new int[] { _biologiaSub.Count, _competenzeSub.Count, _logicaSub.Count, _chimicaSub.Count }) <= 2;

    private void SetActiveTest(int test, bool active) => _testsUI[test].ForEach(d => d.SetActive(active));


    public void LoadTest(float test)
    {
        var newTest = (int)test - 1;

        if (newTest != _currentTest)
        {
            SetActiveTest(_currentTest, false);
            SetActiveTest(newTest, true);
            _currentTest = newTest;
            TestCaricato?.Invoke(_currentTest + 1);
        }

    }

}
