using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    private TMP_Text _textTMP;
    public float typingSpeed = 1.0f;
    private string _fullText;

    private void Start()
    {
        _textTMP = GetComponent<TMP_Text>();
        _fullText = _textTMP.text;
        Utility.myLog(_fullText);
        _textTMP.text = string.Empty;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char text in _fullText)
        {
            _textTMP.text += text;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
