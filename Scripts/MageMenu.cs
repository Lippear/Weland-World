using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MageMenu : MonoBehaviour
{
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _stunButton;
    [SerializeField] private Mage _mageCharacter;
    [SerializeField] private Color _reloadedStunColor;
    [SerializeField] private Color _ReloadingStunColor;
    [SerializeField] private Button _healButton;
    [SerializeField] private Color _healButtonReadyToUseColor;
    [SerializeField] private Color _reloadingHealButtonColor;
    [SerializeField] private TMP_Text _numberOFSecondsToReloadHealSkill;
    [SerializeField] private TMP_Text _numberOfSecondsToReloadStunSkill;

    private void OnEnable()
    {
        _attackButton.onClick.AddListener(Attacking);
        _stunButton.onClick.AddListener(Stuning);
        _healButton.onClick.AddListener(Healing);
    }

    private void Healing()
    {
        if (_mageCharacter.Healing())
        {
            _healButton.image.color = _reloadingHealButtonColor;
            StartCoroutine(ReloadHealButton());
        }
    }

    private IEnumerator ReloadHealButton()
    {
        float numberOFSecondsToReloadHealSkill = _mageCharacter.SecondsToReloadHealSkill();
        do
        {
            _numberOFSecondsToReloadHealSkill.text=numberOFSecondsToReloadHealSkill.ToString();
            yield return new WaitForSeconds(1);
            numberOFSecondsToReloadHealSkill--;
        }
        while (numberOFSecondsToReloadHealSkill > 0);
        _numberOFSecondsToReloadHealSkill.text = "";
        _healButton.image.color = _healButtonReadyToUseColor;
    }

    private void Attacking()
    {
        _mageCharacter.Attacking();
    }

    private void Stuning()
    {
        if (_mageCharacter.Stuning())
        {
            ReloadStunButton();
        }
    }

    private void ReloadStunButton()
    {
        _stunButton.image.color = _ReloadingStunColor;
        StartCoroutine(ReloadingStunButton());
    }

    private IEnumerator ReloadingStunButton()
    {
        float timeToReload = _mageCharacter.TimeReloadingStun();
        do
        {
            _numberOfSecondsToReloadStunSkill.text = timeToReload.ToString();
            yield return new WaitForSeconds(1);
            timeToReload--;
        }
        while (timeToReload > 0);
        _numberOfSecondsToReloadStunSkill.text = "";
        _stunButton.image.color = _reloadedStunColor;
        Debug.Log("f dwf");
    }
}
