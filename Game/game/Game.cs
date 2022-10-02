using Godot;
using System;
using System.Collections.Generic;

public delegate void TimerCallback();

public class TimedAction {
    public ulong time;
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

    private List<PackedScene> lvls;
    private List<TimedAction> timers = new List<TimedAction>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("WAP");

        lvls = new List<PackedScene>(Levels);
        StartGame();

        // 1. Show instructions modal
        // 2. On dismissmal, start 3 second count down
        // 3. Load first scene, start timer
        // 4. On success or timeout, proceed to next level
    }

    public void StartGame() {
        // Close modal
        // Update flashing numbers
        Delay(0, () => GD.Print("Three!"));
        Delay(1000, () => GD.Print("Two!"));
        Delay(2000, () => GD.Print("One!"));
        Delay(3000, () => GD.Print("Fuckin start yeah!!!"));
    }

    public void Delay(ulong ms, TimerCallback cb) {
        timers.Add(new TimedAction(ms + Time.GetTicksMsec(), cb));
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
    }
}
