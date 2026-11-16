using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Notifications")]
    [SerializeField] private Transform _notificationContainer;
    [SerializeField] private GameObject _notificationTextPF;

    [Header("Tiempo De vida de Notificacion")]
    [SerializeField] private float _notificationDuration = 4f;
    [SerializeField] private float _fadeDuration = 1.5f;

    [Header("Notification Limit")]
    [SerializeField] private int _maxNotifications = 5;

    private List<GameObject> _activeNotifications = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void ShowNotification(string message)
    {
        while (_activeNotifications.Count >= _maxNotifications)
        {
            GameObject oldestNotification = _activeNotifications[0];

            _activeNotifications.RemoveAt(0);

            if (oldestNotification != null)Destroy(oldestNotification);
        }

        GameObject notification = Instantiate(_notificationTextPF,_notificationContainer);

        _activeNotifications.Add(notification);

        TMP_Text text = notification.GetComponent<TMP_Text>();
        text.text = message;

        StartCoroutine(RemoveNotification(notification, text));
    }

    private IEnumerator RemoveNotification(GameObject notification, TMP_Text text)
    {
        yield return new WaitForSeconds(_notificationDuration);

        float elapsed = 0f;
        Color originalColor = text.color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);

            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        _activeNotifications.Remove(notification);
        Destroy(notification);
    }

}
