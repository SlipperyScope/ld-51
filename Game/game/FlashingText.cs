using Godot;
using System;

public class FlashingText : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private string _flashingText = "";
    private RichTextLabel rtl;
    private bool fadingOut = false;
    public string flashingText {
        get { return _flashingText; }
        set {
            _flashingText = value;
            if (rtl != null) {
                rtl.BbcodeText = $"[center]{value}[/center]";
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rtl = GetNode<RichTextLabel>("Label");
        rtl.BbcodeText = $"[center]{flashingText}[/center]";
        GetNode<Timer>("Timer").Connect("timeout", this, nameof(Fadeout));
    }

    private void Fadeout() {
        fadingOut = true;
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
    if (fadingOut) {
        rtl.Modulate = new Color(rtl.Modulate, rtl.Modulate.a - 0.1f);
        rtl.MarginTop = rtl.MarginTop + 6;
        if (rtl.Modulate.a <= 0) {
            fadingOut = false;
            QueueFree();
        }
    }
 }
}
