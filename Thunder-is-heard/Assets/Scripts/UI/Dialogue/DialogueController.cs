using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class DialogueController : UIElement
{
    public Replic[] _replics;
    public int _currentReplicIndex;

    public Dictionary<string, Sprite> _spriteByCharName;

    // char
    public Image _charImage;
    public string _charName;
    public string _charSide;
    public int charXPositionForLeftSide = -720;
    public int charXPositionForRightSide = 720;
    // char

    // replicWindow
    public Image _replicWindow;
    public int replicWindowXPositionForLeftSide = -260;
    public int replicWindowXPositionForRightSide = 260;

    // ambient
    public Transform ambient;
    public int dialoguePanelRotationForLeftSide = 0;
    public int dialoguePanelRotationForRightSide = 180;

    // text
    public TMP_Text _replicTextComponent;
    public string _replicTextComponentValue;
    // text

    // scrollbar
    public Scrollbar _replicScrollbar;
    public Image _scrollbarAsImage;
    public int scrollbarXPositionForLeftSide = -74;
    public int scrollbarXPositionForRightSide = 74;
    // scrollbar


    public void Start()
    {
        Init();
        Hide();
    }

    public void Init()
    {
        InitSprites();
        EnableListeners();
    }

    public void InitSprites()
    {
        _spriteByCharName = new Dictionary<string, Sprite>();
        string spritesSection = Config.resources["chars"];
        Sprite[] sprites = Resources.LoadAll<Sprite>(spritesSection);

        foreach (Sprite sprite in sprites)
        {
            _spriteByCharName.Add(sprite.name, sprite);
        }
    }

    public void EnableListeners()
    {
        EventMaster.current.BegunDialogue += BeginDialogue;

    }

    public void EnablePassReplicListener()
    {
        EventMaster.current.ReplicPassed += ToNextReplic;
    }

    public void DisablePassReplicListener()
    {
        EventMaster.current.ReplicPassed -= ToNextReplic;
    }

    public void ToNextReplic()
    {
        _currentReplicIndex++;
        if (_replics.Length > _currentReplicIndex)
        {
            UpdateDialogue(_replics[_currentReplicIndex]);
        }
        else
        {
            OverDialogue();
        }
    }

    public void OverDialogue()
    {
        DisablePassReplicListener();

        EventMaster.current.OnEndDialogue();

        Hide();
    }

    public void BeginDialogue(Replic[] replics)
    {
        EnablePassReplicListener();

        _replics = replics;
        _currentReplicIndex = 0;

        UpdateDialogue(replics[_currentReplicIndex]);
        Show();
    }

    public void UpdateDialogue(Replic replic)
    {
        UpdateDialogue(replic._charName, replic._charSide, replic._text);
    }

    public void UpdateDialogue(string charName, string side, string text)
    {
        _charSide = side;
        _charName = charName;
        _replicTextComponentValue = text;
        UpdateChar();
        UpdateText();
        UpdateScrollbar();
        UpdateAmbient();
    }

    public void UpdateChar()
    {
        _charImage.sprite = _spriteByCharName[_charName];

        Vector2 oldPosition = _charImage.rectTransform.anchoredPosition;
        int xPosition;
        if (_charSide == Sides.federation)
        {
            xPosition = charXPositionForLeftSide;
        }
        else
        {
            xPosition= charXPositionForRightSide;
        }

        _charImage.rectTransform.anchoredPosition = new Vector2(xPosition, oldPosition.y);
    }

    public void UpdateText()
    {
        _replicTextComponent.text = _replicTextComponentValue;

        Vector2 oldPosition = _replicWindow.rectTransform.anchoredPosition;
        int xPosition;
        if (_charSide == Sides.federation)
        {
            xPosition = replicWindowXPositionForLeftSide;
        }
        else
        {
            xPosition = replicWindowXPositionForRightSide;
        }

        _replicWindow.rectTransform.anchoredPosition = new Vector3(xPosition, oldPosition.y);

    }

    public void UpdateScrollbar()
    {
        _replicScrollbar.value = 1;

        Vector2 oldPosition = _scrollbarAsImage.rectTransform.position;
        int xPosition;
        if (_charSide == Sides.federation)
        {
            xPosition = scrollbarXPositionForLeftSide;
        }
        else
        {
            xPosition = scrollbarXPositionForRightSide;
        }

        _scrollbarAsImage.rectTransform.position = new Vector2(xPosition, oldPosition.y);
    }

    public void UpdateAmbient()
    {
        Vector3 currentRotate = ambient.gameObject.transform.eulerAngles;
        int newRotationByY;
        if (_charSide == Sides.federation)
        {
            newRotationByY = dialoguePanelRotationForLeftSide;
        }
        else
        {
            newRotationByY = dialoguePanelRotationForRightSide;
        }

        ambient.gameObject.transform.rotation = Quaternion.Euler(currentRotate.x, newRotationByY, currentRotate.z);
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