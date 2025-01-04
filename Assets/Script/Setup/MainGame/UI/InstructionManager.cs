using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionManager: MonoBehaviour
{
    private static InstructionManager instance;
    public static InstructionManager Instance { get =>  instance; }

    [SerializeField] private TMP_Text messageText;
    private Transform showingInstruction;
    private bool isShowing = false;

    [Header("Parameters")]
    private Vector3 hidePosition;
    private Vector3 showPosition;
    [SerializeField] private float moveTime;

    private void Awake()
    {
        this.SetSingleton();

        this.showingInstruction = this.messageText.transform.parent;
    }

    private void Start()
    {
        this.LoadPositions();
    }

    private void SetSingleton()
    {
        if(instance == null) 
            instance = this;
        else 
            Destroy (instance);
    }

    private void LoadPositions()
    {
        this.showPosition = this.showingInstruction.position;

        float offset = ((RectTransform)this.showingInstruction).rect.height * 0.5f;
        Vector3[] canvasConners = new Vector3[4];
        RectTransform rootCanvas = Utility.GetRootCanvas(this.showingInstruction);
        rootCanvas.GetWorldCorners(canvasConners);
        this.hidePosition = new Vector3(this.showPosition.x, canvasConners[1].y + offset, this.showPosition.z);

        this.showingInstruction.position = this.hidePosition;
    }

    public void ShowInstruction(string message)
    {
        if (!this.isShowing)
            this.DisplayInstruction(message);
        else
            StartCoroutine(this.WaitToShowInstruction(message));
    }

    private IEnumerator WaitToShowInstruction(string message)
    {
        while(this.isShowing)
            yield return null;
        this.DisplayInstruction(message);
    }

    private void DisplayInstruction(string message)
    {
        this.messageText.text = message;
        this.isShowing = true;
        this.showingInstruction.DOMove(this.showPosition, this.moveTime);
    }

    public void HideInstruction()
    {
        this.showingInstruction.DOMove(this.hidePosition, this.moveTime)
            .OnComplete(() =>
            {
                this.isShowing = false;
            });
    }
}
