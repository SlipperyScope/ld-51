using Godot;
using ld51.Utils;
using ld51.Weapons;
using ld51.Willie;
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

    /// <summary>
    /// Time bow draw started in ms
    /// </summary>
    private UInt64 BowDrawStartTime = 0ul;

    /// <summary>
    /// Bow is being drawn
    /// </summary>
    private Boolean DrawingBow = false;

    /// <summary>
    /// True if willie is alive
    /// </summary>
    public Boolean Alive = true;

    #region Node
    public override void _EnterTree()
    {
        if (_Bow is null || ArrowScene is null) throw new ArgumentNullException($"{nameof(_Bow)}{(_Bow is null && ArrowScene is null ? " and " : "")}{nameof(ArrowScene)} {(_Bow is null && ArrowScene is null ? "are" : "is")} not set");

        Connect("body_entered", this, nameof(OnBodyEntered));
        Connect("body_exited", this, nameof(OnBodyExited));
    }

    public override void _Ready()
    {
        // Bow will not be ready if it is lower in the child list
        CallDeferred(nameof(GetBow));
        GetNode<BodyPart>("Head").DamageTaken += OnHeadDamage;
    }

    public override void _Process(Single delta)
    {
        if (DrawingBow is true)
        {
            if (Bow.DrawPosition < 1f)
            {
                Bow.DrawPosition = Mathf.Clamp((Time.GetTicksMsec() - BowDrawStartTime) / (DrawTime * 1000f), 0f, 1f);
            }
        }
    }

    public override void _Input(InputEvent e)
    {
        switch(e)
        {
            case InputEventMouseMotion motion:
                UpdateBow(motion.GlobalPosition);
                break;

            case InputEvent when e.IsActionPressed("Shoot"):
                BowDrawStartTime = Time.GetTicksMsec();
                DrawingBow = true;
                break;

            case InputEvent when e.IsActionReleased("Shoot"):
                DrawingBow = false;
                Bow.FireArrow();
                NockArrow();
                break;

            default:
                break;
        }
    }
    #endregion

    /// <summary>
    /// Handles damage event from the head
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnHeadDamage(System.Object sender, DamageTakenEventArgs e)
    {
        foreach(var child in GetChildren())
        {
            if (child is BodyPart bodyPart)
            {
                bodyPart.Detach();
                Alive = false;
            }
        }
    }

    /// <summary>
    /// Gets bow from path
    /// </summary>
    private void GetBow()
    {
        Bow = GetNode<Bow>(_Bow);
        NockArrow();
    }

    /// <summary>
    /// Create and nock arrow
    /// </summary>
    private void NockArrow()
    {
        var arrow = ArrowScene.Instance<Arrow>();
        arrow.Instigator = this;
        arrow.Held = true;
        Bow.NockArrow(arrow);
    }

    /// <summary>
    /// Updates the bow position and rotation
    /// </summary>
    /// <param name="mousePosition">Global position of the mouse</param>
    private void UpdateBow(Vector2 mousePosition)
    {
        if (Bow is null) return;

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
    private void OnBodyEntered(PhysicsBody2D body)
    {
        if (Alive is false && body is Arrow arrow)
        {
            arrow.Activate();
        }
    }
    
    /// <summary>
         /// Physics body exited Willie's range
         /// </summary>
         /// <param name="body">Physics body</param>
    private void OnBodyExited(PhysicsBody2D body)
    {
        if (Alive is true && body is Arrow arrow)
        {
            arrow.Activate();
        }
    }
}
