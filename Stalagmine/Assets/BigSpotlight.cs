using UnityEngine;

public class BigSpotlight : MonoBehaviour
{
    public Color badLightColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventDispatcher.Instance.OnCoreDestroyed += BadLight;
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.OnCoreDestroyed -= BadLight;
    }

    void BadLight()
    {
        GetComponent<Light>().color = badLightColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
