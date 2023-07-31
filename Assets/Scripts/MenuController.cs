using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ButtonType 
{ 
    StartButton, RecordsButton, TitlesButton, ExitButton, BackRecordsButton, 
    BackTitlesButton, backDialogButton, quitButton, bgButton 
}

public class MenuController : MonoBehaviour
{
    [SerializeField] private HighScoreTable highScoreTable;
    [SerializeField] private Transform recordsPanel, titlesPanel, dialogPanel;
    [SerializeField] private AudioClip movePanelSound;
    [SerializeField] private List<ButtonController> buttons;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void CheckButton(ButtonType type)
    {
        switch (type)
        {
            case ButtonType.StartButton:
                SceneManager.LoadScene(1);
                break;
            case ButtonType.RecordsButton:
                highScoreTable.ClearTable();
                LeaderboardManager.FillHighScoreTable(highScoreTable);
                ShowPanel(recordsPanel);
                break;
            case ButtonType.TitlesButton:
                ShowPanel(titlesPanel);
                break;
            case ButtonType.ExitButton:
                ShowPanel(dialogPanel);
                break;
            case ButtonType.BackRecordsButton:
                HidePanel(recordsPanel);
                break;
            case ButtonType.BackTitlesButton:
                HidePanel(titlesPanel);
                break;
            case ButtonType.backDialogButton:
                HidePanel(dialogPanel);
                break;
            case ButtonType.bgButton:
                HidePanel(dialogPanel);
                break;
            case ButtonType.quitButton:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public void EnableRaycastButtons(bool value)
    {
        foreach (ButtonController button in buttons) button.RaycastEnable(value);
    }

    private void ShowPanel(Transform panel)
    {
        _audioSource.PlayOneShot(movePanelSound);

        panel.gameObject.SetActive(true);
        panel.DOMoveY(-150f, 0.75f, false).SetEase(Ease.OutBack).From().OnComplete(() =>
        {
            foreach (ButtonController button in buttons) button.UpButtonFalse();
            EnableRaycastButtons(true);
        });
    }

    private void HidePanel(Transform panel)
    {
        _audioSource.PlayOneShot(movePanelSound);

        panel.DOMoveY(-150f, 0.75f, false).SetEase(Ease.InBack).OnComplete(() =>
        {
            foreach (ButtonController button in buttons) button.UpButtonFalse();
            EnableRaycastButtons(true);
            panel.gameObject.SetActive(false);
            panel.position = new Vector3(panel.position.x, panel.position.y + 150f, panel.position.z);
        });
    }
}
