using UnityEngine;

public abstract class Screen : MonoBehaviour, IScreen
{
    protected abstract void Reset();
    protected abstract void Setup();
    protected abstract void Tick();
    
    void IScreen.Hide()
    {
        Reset();
        gameObject.SetActive(false);
    }

    void IScreen.Show()
    {
        gameObject.SetActive(true);
        Setup();
    }

    private void Update() => Tick();
}
