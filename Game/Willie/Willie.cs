using Godot;
using ld51.Weapons;
using System;

/// <summary>
/// Player-possessible entity that fires arrows
/// </summary>
public class Willie : Area2D
{
    /// <summary>
    /// Number of seconds to fully draw back the bow
    /// </summary>
    [Export]
    public Single DrawTime { get; private set; }

    /// <summary>
    /// Strength of the launch impulse
    /// </summary>
    [Export]
    public Single Strength { get; private set; }

    /// <summary>
    /// Arrow to fire
    /// </summary>
    [Export]
    public PackedScene ArrowScene { get; private set; }

    /// <summary>
    /// Bow to weild
    /// </summary>
    [Export]
    private NodePath _Bow;

    /// <summary>
    /// Distance away from body
    /// </summary>
    [Export]
    private Single BowDistance;

    /// <summary>
    /// Max angle from x-> in degrees
    /// </summary>
    [Export]
    private Single MaxAngle
    {
        get => Mathf.Rad2Deg(MaxRad);
        set => MaxRad = Mathf.Deg2Rad(value);
    }

    /// <summary>
    /// Max angle from x-> in radians
    /// </summary>
    private Single MaxRad;


    /// <summary>
    /// Min angle from x-> in degrees
    /// </summary>
    [Export]
    private Single MinAngle
    {
        get => Mathf.Rad2Deg(MinRad);
        set => MinRad = Mathf.Deg2Rad(value);
    }

    /// <summary>
    /// Min angle from x-> in radians
    /// </summary>
    private Single MinRad;

    /// <summary>
    /// Ref to bow
    /// </summary>
    private Bow Bow;

    #region Node
    public override void _EnterTree()
    {
        if (_Bow is null || ArrowScene is null) throw new ArgumentNullException($"{nameof(_Bow)}{(_Bow is null && ArrowScene is null ? " and " : "")}{nameof(ArrowScene)} {(_Bow is null && ArrowScene is null ? "are" : "is")} not set");

        //Connect("body_entered", this, nameof(OnBodyEntered));
        Connect("body_exited", this, nameof(OnBodyExited));
    }

    public override void _Ready()
    {
        Bow = GetNode<Bow>(_Bow);
    }

    public override void _Process(Single delta)
    {
        
    }

    public override void _Input(InputEvent e)
    {
        switch(e)
        {
            case InputEventMouseMotion motion:
                UpdateBow(motion.GlobalPosition);
                break;
        }
    }
    #endregion

    /// <summary>
    /// Updates the bow position and rotation
    /// </summary>
    /// <param name="mousePosition">Global position of the mouse</param>
    private void UpdateBow(Vector2 mousePosition)
    {
        var position = GlobalPosition;
        var transform = GlobalTransform;
        transform.Rotation = Mathf.Clamp(Vector2.Right.AngleTo(position.DirectionTo(mousePosition)), MinRad, MaxRad);
        transform.origin += transform.x * BowDistance;
        Bow.GlobalTransform = transform;
    }

    /// <summary>
    /// Physics body exited Willie's range
    /// </summary>
    /// <param name="body">Physics body</param>
    private void OnBodyExited(PhysicsBody2D body)
    {
        if (body is Arrow arrow)
        {
            arrow.Activate();
        }
    }
}
