using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeBaseNameModal : UIElement
{
    public bool focusOn = false;
    public TMP_Text description;
    public TMP_InputField nameInput;
    public Button confirmButton;
    public bool isCurrentNameValid = false;


    public void Start()
    {
        InitListeners();
        Hide();
    }

    public virtual void InitListeners()
    {
        EnableListeners();
    }

    public virtual void EnableListeners()
    {
        EventMaster.current.ToggledToBuildMode += Hide;
    }

    public virtual void DisableListeners()
    {
        EventMaster.current.ToggledToBuildMode -= Hide;
    }

    public virtual void Update()
    {
        if (IsClickedOutside())
        {
            OnClickOutside();
        }
    }

    public virtual void OnClickOutside()
    {
        Hide();
    }

    public bool IsClickedOutside()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return !focusOn;
        }

        return false;
    }


    public void Confirm()
    {
        if (IsAvailableToRename())
        {
            OnRename();
            Hide();
        }
    }

    public void OnInputChange(string value)
    {
        // Фильтрация ввода от запрещенных символов
        string filteredText = System.Text.RegularExpressions.Regex.Replace(
            nameInput.text,
            @"[^a-zA-Zа-яА-Я0-9\-_\.]",
            ""
        );
        
        // Если текст изменился после фильтрации, обновляем значение в поле ввода
        if (filteredText != nameInput.text)
        {
            nameInput.text = filteredText;
            return; // Выходим, так как изменение текста вызовет новый OnInputChange
        }

        ValidateName();
        confirmButton.interactable = isCurrentNameValid;
    }

    public void ValidateName()
    {
        string name = nameInput.text;

        // Проверка длины
        if (name.Length < 3 || name.Length > 23)
        {
            isCurrentNameValid = false;
            return;
        }

        // Регулярное выражение для проверки допустимых символов
        // [a-zA-Zа-яА-Я0-9\-_\.] - разрешенные символы
        // ^[a-zA-Zа-яА-Я0-9\-_\.]+$ - строка должна состоять только из разрешенных символов
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я0-9\-_\.]+$"))
        {
            isCurrentNameValid = false;
            return;
        }

        // Проверка наличия хотя бы одной буквы
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"[a-zA-Zа-яА-Я]"))
        {
            isCurrentNameValid = false;
            return;
        }

        isCurrentNameValid = true;
    }

    public bool IsAvailableToRename()
    {
        return isCurrentNameValid;
    }

    public void OnRename()
    {
        ResourcesProcessor.ChangeBaseName(nameInput.ToString());
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        focusOn = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        focusOn = false;
    }

    public override void Hide()
    {
        base.Hide();
        EventMaster.current.OnUIPanelToggle(false);
    }

    public override void Show()
    {
        base.Show();
        EventMaster.current.OnUIPanelToggle(true);
    }
}
