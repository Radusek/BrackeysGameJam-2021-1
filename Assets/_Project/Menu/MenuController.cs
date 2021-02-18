using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private float animationDuration;
    [SerializeField] private Ease animationEase;

    [Header("Canvas positions")]
    [SerializeField] private Transform topPos;
    [SerializeField] private Transform centerPos;
    [SerializeField] private Transform bottomPos;
 
    private Stack<GameObject> menuStack;
    private int animationLocks;

    public bool IsAtRootSubmenu => menuStack.Count == 1;


    private void Awake()
    {
        menuStack = new Stack<GameObject>();
        menuStack.Push(menuRoot);
    }

    private void OnEnable() => MakeSubmenuInteractable(menuStack.Peek(), true);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 1 && animationLocks == 0)
            Back();
    }

    public void ResetMenu()
    {
        while (menuStack.Count > 1)
        {
            menuStack.Pop().SetActive(false);
        }

        menuStack.Peek().transform.position = centerPos.transform.position;
        menuStack.Peek().SetActive(true);
    }

    public void ShowRoot()
    {
        var submenu = menuStack.Peek();
        submenu.SetActive(true);
        AnimateSubmenu(submenu, centerPos, topPos);
    }

    public void SelectSubmenu(GameObject submenu)
    {
        HideCurrentOnSelect();
        OpenSubmenu(submenu);
    }

    public void Back()
    {
        HideCurrentOnBack();
        OpenPreviousSubmenu();
    }

    private void HideCurrentOnSelect()
    {
        var submenu = menuStack.Peek();
        AnimateSubmenu(submenu, topPos).OnComplete(() => { submenu.SetActive(false); animationLocks--; });
    }

    private void OpenSubmenu(GameObject submenu)
    {
        menuStack.Push(submenu);
        submenu.SetActive(true);
        AnimateSubmenu(submenu, centerPos, bottomPos);
    }

    private void HideCurrentOnBack()
    {
        var submenu = menuStack.Pop();
        AnimateSubmenu(submenu, bottomPos).OnComplete(() => { submenu.SetActive(false); animationLocks--; });
    }

    private void OpenPreviousSubmenu()
    {
        var submenu = menuStack.Peek();
        submenu.SetActive(true);
        AnimateSubmenu(submenu, centerPos);
    }

    private void MakeSubmenuInteractable(GameObject submenu, bool enable)
    {
        foreach (var selectable in submenu.GetComponentsInChildren<Selectable>())
        {
            selectable.interactable = enable;
        }
    }

    private Tween AnimateSubmenu(GameObject submenu, Transform destination, Transform origin = null)
    {
        animationLocks++;
        MakeSubmenuInteractable(submenu, false);
        float fromValue = origin ? origin.transform.position.y : submenu.transform.position.y;
        return submenu.transform.DOMoveY(destination.transform.position.y, animationDuration).From(fromValue).SetEase(animationEase).SetUpdate(true).OnComplete(() => { MakeSubmenuInteractable(submenu, true); animationLocks--; });
    }
}
