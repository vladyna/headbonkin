using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisappearingMessageUI : MonoBehaviour
{
    #region Serialized Fields
    [Header("Settings")]
    [SerializeField] private TextMeshPro templateLabel;
    [SerializeField] private int messagePool = 10;
    [SerializeField] private float disappearTimeSeconds = 2f;
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private Color defaultColor = Color.white;
    #endregion
    #region Private Fields
    private Queue<TextMeshPro> availablePool;
    private Queue<(string text, Vector3 worldPos)> textQueue;
    private List<MessageData> activeMessages;
    #endregion
    #region Class Helper
    private class MessageData
    {
        public TextMeshPro label;
        public float creationTime;
        public Vector3 startPos;
    }
    #endregion
    #region Unity's Methods

    private void Awake()
    {
        availablePool = new Queue<TextMeshPro>();
        textQueue = new Queue<(string text, Vector3 worldPos)>();
        activeMessages = new List<MessageData>();

        for (int i = 0; i < messagePool; i++)
        {
            var newLabel = Instantiate(templateLabel, transform);
            newLabel.gameObject.SetActive(false);
            availablePool.Enqueue(newLabel);
        }
    }

    private void Update()
    {
        if (textQueue.Count > 0 && availablePool.Count > 0)
        {
            var (text, pos) = textQueue.Dequeue();
            SpawnText(text, pos);
        }

        for (int i = activeMessages.Count - 1; i >= 0; i--)
        {
            var msg = activeMessages[i];
            float elapsed = Time.time - msg.creationTime;

            if (elapsed > disappearTimeSeconds)
            {
                ResetLabel(msg.label);
                availablePool.Enqueue(msg.label);
                activeMessages.RemoveAt(i);
            }
            else
            {
                MoveAndFade(msg.label, msg.startPos, elapsed);
            }
        }
    }
    #endregion
    #region Public Methods
    public void ShowText(string message, Vector3 worldPosition)
    {
        textQueue.Enqueue((message, worldPosition));
    }
    #endregion
    #region Private Methods

    private void SpawnText(string text, Vector3 worldPos)
    {
        var label = availablePool.Dequeue();
        label.text = text;
        label.color = defaultColor;
        label.rectTransform.position = worldPos;
        label.gameObject.SetActive(true);
        activeMessages.Add(new MessageData 
        { 
            label = label, 
            creationTime = Time.time,
            startPos = worldPos
        });
    }

    private void MoveAndFade(TextMeshPro label, Vector3 startPos, float elapsed)
    {
        float alpha = 1f - (elapsed / disappearTimeSeconds);
        var color = label.color;
        color.a = alpha;
        label.color = color;

        Vector3 pos = startPos + Vector3.up * moveSpeed * elapsed;
        label.rectTransform.position = pos;

        label.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    private void ResetLabel(TextMeshPro label)
    {
        label.text = "";
        label.color = defaultColor;
        label.gameObject.SetActive(false);
    }
    #endregion
}
