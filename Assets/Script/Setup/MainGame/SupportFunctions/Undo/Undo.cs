using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Undo : MonoBehaviour
{
    public static Undo Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject pauseButton;

    [Header("Parameters")]
    [SerializeField] private float rewindTime;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void OnUndo()
    {
        this.SetInteract(false);

        LastMovedState lastState = StateManager.GetLastState();
        this.RestoreState(lastState);
    }

    private void RestoreState(LastMovedState lastState)
    {
        if (lastState == null)
        {
            this.SetInteract(true);
            return;
        }

        Tween loopingTween;
        Sequence tweenSequence = DOTween.Sequence();
        
        Color loopingColor;
        int index = 0;
        List<RigidbodyType2D> oldBodyTypeList = new();
        List<int> oldLayerList = new();

        foreach(Plate plate in lastState.PlateList)
        {
            if (!plate.gameObject.activeSelf)
            {
                loopingColor = plate.SpriteRenderer.color;
                loopingColor.a = 0;
                plate.SpriteRenderer.color = loopingColor;
                plate.gameObject.SetActive(true);
                loopingTween = plate.SpriteRenderer.DOFade(1, this.rewindTime);
                tweenSequence.Join(loopingTween);
            }

            plate.StopAllCoroutines();

            if (DOTween.IsTweening(plate.SpriteRenderer))
            {
                plate.SpriteRenderer.DOKill();
                loopingTween = plate.SpriteRenderer.DOFade(1, this.rewindTime);
                tweenSequence.Join(loopingTween);
            }

            oldBodyTypeList.Add(plate.RigidBody.bodyType);
            plate.RigidBody.bodyType = RigidbodyType2D.Kinematic;

            oldLayerList.Add(plate.gameObject.layer);
            plate.gameObject.layer = Config.GetSingleLayerFromLayerMask(Config.Instance.IgnoreCollisionLayerMask);

            loopingTween = plate.transform.DOMove(lastState.PlatePositionList[index], this.rewindTime);
            tweenSequence.Join(loopingTween);
            loopingTween = plate.transform.DORotate(lastState.PlateRotationList[index].eulerAngles, this.rewindTime);
            tweenSequence.Join(loopingTween);

            index++;
        }

        if (!lastState.Bolt.gameObject.activeSelf)
        {
            loopingColor = lastState.Bolt.Model.defaultModel.color;
            loopingColor.a = 0;
            lastState.Bolt.Model.defaultModel.color = loopingColor;
            lastState.Bolt.gameObject.SetActive(true);
            loopingTween = lastState.Bolt.Model.defaultModel.DOFade(1, this.rewindTime);
            tweenSequence.Join(loopingTween);
            BoltCounter.UpdateCount(+1);
        }

        lastState.Bolt.SetColliderTrigger(true);
        loopingTween = lastState.Bolt.transform.DOMove(lastState.WallHole.transform.position, this.rewindTime);
        tweenSequence.Join(loopingTween);
        loopingTween = lastState.Bolt.Model.defaultModel.DOFade(1, this.rewindTime);
        tweenSequence.Join(loopingTween);

        tweenSequence.AppendCallback(() => EndTween(oldLayerList, oldBodyTypeList, lastState));
        tweenSequence.Play();
    }

    private void EndTween(List<int> layerList,List<RigidbodyType2D> oldBodyType, LastMovedState lastState)
    {
        int index = 0;
        foreach(Plate plate in lastState.PlateList)
        {
            plate.RigidBody.velocity = lastState.PlateVelocityList[index];
            plate.RigidBody.bodyType = oldBodyType[index];
            plate.gameObject.layer = layerList[index];

            index++;
        }

        lastState.Bolt.SetColliderTrigger(false);
        lastState.Bolt.ChangeHoles(lastState.WallHole, lastState.PlateHoleList);

        this.SetInteract(true);
    }

    private void SetInteract(bool interact)
    {
        SupportFunctions_UI.Instance.ShowSupportFunctions(interact, true);
    }
}
