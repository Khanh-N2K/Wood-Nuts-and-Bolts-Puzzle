using UnityEngine;

public class PlateHole : WallHole
{
    public Plate Plate { get; private set; }

    private void Awake()
    {
        this.LoadReferences();
    }

    private void LoadReferences()
    {
        Plate = transform.parent.GetComponent<Plate>();
        if (Plate == null)
            Debug.LogError("Can't find plate for " + name);
    }

    public override void PinBolt(Bolt bolt)
    {
        base.PinBolt(bolt);

        if(bolt != null)
            Plate.PinNewBolt();
        else
            Plate.UnpinBolt();
    }

    public void InitPinnedBolt(Bolt bolt)
    {
        base.PinBolt(bolt);
    }
}
