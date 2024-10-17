using UnityEngine;
using TMPro;
using System.Collections;  // Make sure to include this for TextMesh Pro

public class RandomEncouragingText : MonoBehaviour
{
    // Array of encouraging texts
    private string[] encouragingTexts = new string[]
    {
        "\"Good Luck! Spin to Win Big!\"",
        "\"Your Prize Awaits! Give it a Spin!\"",
        "\"Feeling Lucky? Let's Find Out!\"",
        "\"The Wheel of Fortune is Turning for You!\"",
        "\"Hold Tight! Your Reward is Just a Spin Away!\"",
        "\"With the Great Power, Comes a Great Spin!\"",
        "\"Here Comes the Fun! Spin for a Surprise!\"",
        "\"Exciting Times Ahead! Spin Now!\"",
        "\"May the Odds Be Ever in Your Favor!\"",
        "\"Spin to Uncover Your Awesome Prize!\"",
        "\"Fingers Crossed! Let's See What You Win!\"",
        "\"The Moment of Truth! Spin to Reveal Your Prize!\"",
        "\"Get Ready! Something Great is Coming Your Way!\"",
        "\"Anticipation is in the Air! Spin to Win!\"",
        "\"Your Lucky Spin is Just a Click Away!\"",
        "\"Great Rewards Await! Spin to Discover Yours!\""
    };

    private string randomText;
    public float typingSpeed = 1.0f;

    // Reference to the TextMesh Pro component
    private TMP_Text tmpText;

    void Start()
    {
        // Get the TextMesh Pro component attached to this GameObject
        tmpText = GetComponent<TMP_Text>();

        // Check if the TextMesh Pro component is found
        if (tmpText == null)
        {
            Debug.LogError("No TextMesh Pro component found on this GameObject.");
            return;
        }

        // Select a random text from the array
        randomText = encouragingTexts[UnityEngine.Random.Range(0, encouragingTexts.Length)];
        tmpText.text = string.Empty;

        // Set the selected text to the TextMesh Pro component
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char text in randomText)
        {
            tmpText.text += text;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
