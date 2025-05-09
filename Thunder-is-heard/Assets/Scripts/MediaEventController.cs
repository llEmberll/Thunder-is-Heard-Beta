using System.Collections;
using UnityEngine;

public class MediaEventController : MonoBehaviour
{
    public CameraController cameraController;

    public void Awake()
    {
        cameraController = GameObject.FindGameObjectWithTag(Tags.mainCamera).GetComponent<CameraController>();

        
    }

    public void Start()
    {
        EventMaster.current.BegunMediaEvent += OnBeginMediaEvent;
    }

    public void OnBeginMediaEvent(MediaEventData eventData)
    {
        StartCoroutine(PlayMediaEvent(eventData));
    }

    public IEnumerator PlayMediaEvent(MediaEventData eventData)
    {
        if (eventData == null)
        {
            yield break;
        }

        AudioSource audioSource = null;
        AudioClip audioClip = null;

        // Обработка аудио
        if (!string.IsNullOrEmpty(eventData._audioEventId))
        {
            string audioPath = Config.resources["audioEvents"] + eventData._audioEventId;
            audioClip = Resources.Load<AudioClip>(audioPath);
            
            if (audioClip != null)
            {
                audioSource = cameraController.gameObject.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = cameraController.gameObject.AddComponent<AudioSource>();
                }
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }

        // Обработка фокуса камеры
        if (eventData._point != null)
        {
            Vector2Int position = new Vector2Int(eventData._point._x, eventData._point._y);
            EventMaster.current.FocusCameraOnPosition(position, true);

            // Сначала ждём, пока камера начнёт фокусировку
            while (!cameraController.haveFocus)
            {
                yield return null;
            }

            // Затем ждём, пока камера завершит фокусировку
            while (cameraController.haveFocus)
            {
                yield return null;
            }
        }

        // Ожидание завершения аудио
        if (audioSource != null && audioClip != null)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        // Дополнительное ожидание
        yield return new WaitForSeconds(1.5f);

        // Завершение события
        EventMaster.current.CancelFocus();
        EventMaster.current.OnEndMediaEvent();
    }
}
