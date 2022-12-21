using UnityEngine;

public class InteractablesDetector : MonoBehaviour
{
    [SerializeField] private CameraUnitAttacher _attacher;
    [SerializeField] private GameObject[] _activateOnDetect;
    private Unit _attached;
    private InteractablesScaner _scaner;
    private Interactable _interactable;

    private void Start()
    {
        _attacher.OnUnitChange += Change;
        Change(_attacher.Attached);
    }

    private void Change(Unit obj)
    {
        if (obj == null) return;

        if (_scaner != null)
        {
            _scaner.OnScan -= Scan;
        }

        if (obj.ComponentSystem.TryToGetComponent<InteractablesScaner>(out _scaner))
        {
            _scaner.OnScan += Scan;
        }

        Scan(null);
        _attached = obj;
    }

    private void Scan(Interactable obj)
    {
        _interactable = obj;
        for (int i = 0, length = _activateOnDetect.Length; i < length; i++)
        {
            _activateOnDetect[i].SetActive(obj != null);
        }
    }

    public void Interact()
    {
        if (_interactable == null || !_interactable.CanInteract(_attached)) return;
        _interactable.Interact(_attached);
    }
}
