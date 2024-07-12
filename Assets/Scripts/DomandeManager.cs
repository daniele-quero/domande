using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DomandeManager : MonoBehaviour
{
    [SerializeField] private List<Domanda> _domandeBiologia;
    [SerializeField] private List<Domanda> _domandeCompetenze;
    [SerializeField] private List<Domanda> _domandeLogica;

    [SerializeField] private int _numBiologia;
    [SerializeField] private int _numCompetenze;
    [SerializeField] private int _numLogica;
    [SerializeField] private int _numberOfTests;
    private int _total;
    [SerializeField] private int _currentTest;

    [SerializeField] private GameObject _domandaTemplate;
    [SerializeField] private Transform _scrollableContent;
    [SerializeField] private Slider _testSelector;
    [SerializeField] private Image _cover;
    [SerializeField] private Image _loadingBar;

    List<List<Domanda>> _biologiaSub = new List<List<Domanda>>();
    List<List<Domanda>> _logicaSub = new List<List<Domanda>>();
    List<List<Domanda>> _competenzeSub = new List<List<Domanda>>();
    List<List<Domanda>> _tests = new List<List<Domanda>>();
    Dictionary<int, List<GameObject>> _testsUI = new Dictionary<int, List<GameObject>>();


    public static Action<int> TestCaricato;
    public int CurrentTest { get => _currentTest; }

    void Start()
    {

        _domandeBiologia = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\biologia.pdf")));

        _domandeCompetenze = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\competenze.pdf")));

        _domandeLogica = DomandeParser.ExtractDomande(DomandeParser.ExtractDomande(
        PdfParser.ReadPdfFile("C:\\Users\\dquero\\unity\\domande\\Assets\\logica.pdf")));

        _total = _domandeBiologia.Count + _domandeCompetenze.Count + _domandeLogica.Count;

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
            DurstenfeldShuffler<Domanda>.shuffle(test);
            _testsUI.Add(i, test.Select(d => InitDomanda(d, i)).ToList());

            _tests.Add(test);
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
        for (_numberOfTests = 22; _numberOfTests <= 60; _numberOfTests++)
        {
            _numLogica = Mathf.RoundToInt(_domandeLogica.Count / (float)_numberOfTests);
            _numBiologia = Mathf.RoundToInt(_domandeBiologia.Count / (float)_numberOfTests);
            _numCompetenze = Mathf.RoundToInt(_domandeCompetenze.Count / (float)_numberOfTests);
            Debug.Log(_numberOfTests);
            _biologiaSub = _domandeBiologia.Select((x, i) => new { Index = i, Value = x })
                   .GroupBy(x => x.Index / _numBiologia)
                   .Select(x => x.Select(v => v.Value).ToList())
                   .ToList();

            _logicaSub = _domandeLogica.Select((x, i) => new { Index = i, Value = x })
              .GroupBy(x => x.Index / _numLogica)
              .Select(x => x.Select(v => v.Value).ToList())
              .ToList();

            _competenzeSub = _domandeCompetenze.Select((x, i) => new { Index = i, Value = x })
               .GroupBy(x => x.Index / _numCompetenze)
               .Select(x => x.Select(v => v.Value).ToList())
               .ToList();


            if (Mathf.Max(new int[] { _biologiaSub.Count, _competenzeSub.Count, _logicaSub.Count }) - Mathf.Min(new int[] { _biologiaSub.Count, _competenzeSub.Count, _logicaSub.Count }) <= 2) break;
        }

        _numberOfTests = Mathf.Min(new int[] { _biologiaSub.Count, _competenzeSub.Count, _logicaSub.Count });
    }


    private void SetActiveTest(int test, bool active)
    {
        _testsUI[test].ForEach(d => d.SetActive(active));
        //foreach (var d in _testsUI[test])
        //{
        //    d.SetActive(active);
        //}
    }


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
