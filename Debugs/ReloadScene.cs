using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private Unit _attachedTo;
    private Timer timer = new Timer(2f);

    private void Start()
    {
        _attachedTo.Health.OnDeath += OnDeath;
        timer.Stop();
    }

    private void Update()
    {
        timer.Update(Time.deltaTime);
    }
    private void OnDeath(DamageArgs obj)
    {
        Destroy(_attachedTo?.gameObject);
        timer.Start();
        timer.OnPeriodReached += Timer;
    }

    private void Timer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
