using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    public TextMeshProUGUI txt_bubble;
    public RectTransform bubbleImage;
    // Start is called before the first frame update
    void Start()
    {
        txt_bubble = transform.Find("Canvas/txt_dialogue").GetComponent<TextMeshProUGUI>();
        bubbleImage = transform.Find("Canvas/bubbleImage").GetComponent<RectTransform>();
        txt_bubble.text = "";
        ShowHideBubble(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    float timeOfStringLength = 0.1f;
    float waitTime = 5f;
    public void ShowContent(int dialogueId)
    {
        var cfg = ConfigManager.table.TbDialogue.Get(dialogueId);
        string content = cfg.Content;
        //waitTime = content.Length * timeOfStringLength;
        txt_bubble.text = content;
    }
    IEnumerator DelayShowBubble()
    {
        ShowHideBubble(true);

        yield return new WaitForSeconds(waitTime);
        ShowHideBubble(false);
    }
    void ShowHideBubble(bool isshow)
    {
        this.bubbleImage.gameObject.SetActive(isshow);
        this.txt_bubble.gameObject.SetActive(isshow);
    }
}
