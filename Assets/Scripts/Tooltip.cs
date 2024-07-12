using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text _tooltip;
    [SerializeField] private DomandeManager _manager;

    private void Awake() => DomandeManager.TestCaricato += ReadyUpdate;

    void Start()
    {
        _tooltip = GetComponentInChildren<Text>();
        _tooltip.text = string.Empty;
    }

    public void OnPointerEnter(PointerEventData eventData) => _tooltip.text = (_manager.CurrentTest + 1).ToString();

    public void OnPointerExit(PointerEventData eventData) => _tooltip.text = string.Empty;

    private void ReadyUpdate(int test)
    {
        if (_tooltip.text != string.Empty)
            _tooltip.text = test.ToString();
    }
}
