using Godot;
using System;
using System.Collections.Generic;

public delegate void TimerCallback();

public class TimedAction {
	public ulong time;
	public readonly float duration;
	public float timeRemaining {
		get {
			if (Time.GetTicksMsec() > time) return 0;
			float remainder = time - Time.GetTicksMsec();
			return remainder / 1000f;
		}
	}
	public TimerCallback cb;

	public TimedAction(ulong time, TimerCallback cb) {
		this.time = time + Time.GetTicksMsec();
		this.duration = time;
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

	private ulong LEVEL_LENGTH = 10000;
	private float PENALTY = 5f;
	private TimedAction currentTimer;
	public string SummaryScene = "res://game/Summary.tscn";
	private Node currentLevel;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lvls = new List<PackedScene>(Levels);
		Global.game = this;

		ConstructLevel();
		Global.paused = true;
		GetNode<Button>("StartModal/StartGame").Connect("pressed", this, nameof(StartGame));
	}

	public void ConstructLevel() {
		if (currentLevel != null) {
			currentLevel.QueueFree();
		}

		PackedScene lvl = lvls[0];
		lvls.RemoveAt(0);
		Global.level++;

		currentLevel = lvl.Instance();
		GetNode<Node2D>("LevelContainer").AddChild(currentLevel);
		Global.paused = false;
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
			Global.paused = false;
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

	public void NextLevel(bool success = false) {
		if (Global.paused) return;

		Global.paused = true;
		Global.score += (LEVEL_LENGTH / 1000) - currentTimer.timeRemaining;

		if (!success) {
			Global.score += PENALTY;
		}

		CancelTimer(currentTimer);

		Flash(success ? "Nice!" : $"Bummer (+{PENALTY}s)");
		Delay(300, () => {
			if (lvls.Count != 0) {
				ConstructLevel();
				currentTimer = Delay(LEVEL_LENGTH, () => NextLevel());
			} else {
				GetTree().ChangeScene(SummaryScene);
			}
		});
	}

	public TimedAction Delay(ulong ms, TimerCallback cb) {
		var timer = new TimedAction(ms, cb);
		timers.Add(timer);
		return timer;
	}

	public void CancelTimer(TimedAction timer) {
		currentTimer = null;
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
		} else {
			Global.timeRemaining = 0;
		}
	}
}
