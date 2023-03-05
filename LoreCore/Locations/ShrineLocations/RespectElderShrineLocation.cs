using ItemChanger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LoreCore.Locations.ShrineLocations;

public class RespectElderShrineLocation : ShrineLocation
{
    #region Members

    private Coroutine _coroutine;

    #endregion

    #region Properties

    public override string Text => "It dreams of bowing to one nail masters.";

    #endregion

    #region Event handler

    private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.Scene arg1)
    {
        if (arg1.name.StartsWith("Room_nailmaster") && _coroutine == null)
            _coroutine = GameManager.instance.StartCoroutine(ListenForDown());
        else if (_coroutine != null)
            GameManager.instance.StopCoroutine(_coroutine);
    }

    #endregion

    #region Control

    protected override void OnLoad()
    {
        base.OnLoad();
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    #endregion

    #region Methods

    private IEnumerator ListenForDown()
    {
        while(true)
        {
            if (InputHandler.Instance.inputActions.down.IsPressed)
                ConditionMet = true;
            yield return null;
        }
    }

    #endregion
}
