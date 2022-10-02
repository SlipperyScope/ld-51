using Godot;
using System;
using System.Collections.Generic;

public delegate void TimerCallback();

public class TimedAction {
    public ulong time;
    public float timeRemaining {
        get {
            return (float)(time - Time.GetTicksMsec()) / 1000f;
        }
    }
    public TimerCallback cb;

    public TimedAction(ulong time, TimerCallback cb) {
        this.time = time;
        this.cb = cb;
    }
}

public class Game : Node2D
{
    [Export]
    public PackedScene[] Levels;
    [Export]
    public PackedScene FlashMessage;

    private List<PackedScene> lvls;
    private List<TimedAction> timers = new List<TimedAction>();

    private ulong LEVEL_LENGTH = 3000;
    private TimedAction currentTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lvls = new List<PackedScene>(Levels);
        Global.game = this;

        ConstructLevel();
        GetNode<Button>("StartModal/StartGame").Connect("pressed", this, nameof(StartGame));
    }

    public void ConstructLevel() {
        PackedScene lvl = lvls[0];
        lvls.RemoveAt(0);
        Global.level++;

        var scene = lvl.Instance();
        GetNode<Node2D>("LevelContainer").AddChild(scene);
    }

    public void StartGame() {
        // Close modal
        GetNode<CanvasLayer>("StartModal").Visible = false;

        // Update flashing numbers
        Delay(0, () => Flash("Three!"));
        Delay(1000, () => Flash("Two!"));
        Delay(2000, () => Flash("One!"));
        Delay(3000, () => {
            Flash("Do Tell!");
            currentTimer = Delay(LEVEL_LENGTH, () => NextLevel());
        });
    }

    public void Flash(string message) {
        var msg = FlashMessage.Instance() as FlashingText;
        msg.flashingText = message;

        var size = GetViewport().GetVisibleRect().Size;
        msg.MarginLeft = size.x / 2;
        msg.MarginTop = size.y / 2;

        GetNode<Node2D>("MessageContainer").AddChild(msg);
    }

    public void NextLevel() {
        CancelTimer(currentTimer);
        if (lvls.Count != 0) {
            ConstructLevel();
            currentTimer = Delay(LEVEL_LENGTH, () => NextLevel());
        }
    }

    public TimedAction Delay(ulong ms, TimerCallback cb) {
        var timer = new TimedAction(ms + Time.GetTicksMsec(), cb);
        timers.Add(timer);
        return timer;
    }

    public void CancelTimer(TimedAction timer) {
        timers.Remove(timer);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Process all dem timers
        var now = Time.GetTicksMsec();
        var garbage = new List<TimedAction>();
        foreach (var timer in timers)
        {
           if (timer.time < now) {
                garbage.Add(timer);
           }
        }

        foreach (var timer in garbage)
        {
            timer.cb();
            timers.Remove(timer);
        }

        if (currentTimer != null) {
            Global.timeRemaining = currentTimer.timeRemaining;
        }
    }
}
