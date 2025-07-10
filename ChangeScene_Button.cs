using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI Text display (optional)
using UnityEngine.SceneManagement; // Required for scene management (optional)

public class ChangeScene_Button : MonoBehaviour
{
    [SerializeField] private Text text;

    public void ChangeScene()
    {
        // This function will be called when the button is clicked.
        // You can add your scene change logic here.
        Debug.Log("Change Scene button clicked!");
        Debug.Log("Text: " + text.text);

        if (text.text.ToUpper() == "OK")
        {
            SceneManager.LoadScene("GSTestScene");
        }

        // Example: Load a new scene (make sure to include the UnityEngine.SceneManagement namespace)
        // UnityEngine.SceneManagement.SceneManager.LoadScene("YourSceneName");
    }
}
