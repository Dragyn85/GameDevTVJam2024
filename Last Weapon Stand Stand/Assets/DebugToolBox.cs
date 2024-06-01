using UnityEngine;

public class DebugToolBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            UnaliveAllAlieans();
        }
        
    }

    private void UnaliveAllAlieans()
    {
        var aliens = FindObjectsByType<AlianBrain>(FindObjectsSortMode.None);

        foreach (var alianBrain in aliens)
        {
            alianBrain.UnaliveAlien();
        }
    }
}
