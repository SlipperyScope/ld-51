using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class RobNHood : Node2D
    {
        [Export]
        private PackedScene ArrowScene;

        [Export]
        private PackedScene BloodEmmitterScene;

        [Export]
        private Single MaxBowDistance = 500f;

        [Export]
        private Single MinAngle = -45f;

        [Export]
        private Single MaxAngle = 45f;

        [Export]
        private Single DrawTime = 0.5f;

        private Hood Hood;
        private Node2D Gibs;
        private Gib Head;
        private Gib Body;
        private Apple Apple;
        private AudioStreamPlayer Dangit;
        private AudioStreamPlayer Ouch;
        private Boolean Dead = false;
        private UInt64 StartDraw = 0;

        public override void _Ready()
        {
            Hood = GetNode<Hood>("Hood");
            Dangit = GetNode<AudioStreamPlayer>("Dangit");
            Ouch = GetNode<AudioStreamPlayer>("Ouch");
            Apple = GetNode<Apple>("Apple");
            Gibs = GetNode<Node2D>("Gibs");
            Body = Gibs.GetNode<Gib>("GibBody");

            Head = Gibs.GetNode<Gib>("GibHead");
            Head.GibHit += OnHeadHit;

            foreach (var child in Gibs.GetChildren())
            {
                if (child is Gib gib)
                {
                    gib.GibHit += (_, data) =>
                    {
                        if (gib.Enabled is false)
                        {
                            Ouch.Play();
                            if (gib.Bleeds is true)
                            {
                                var emmitter = BloodEmmitterScene.Instance<Particles2D>();
                                gib.AddChild(emmitter);
                                emmitter.GlobalPosition = data.Data.Location;
                                emmitter.LookAt(data.Data.Location - data.Data.Direction);
                                
                            }
                        }
                    };
                }
            }
        }

        public override void _Process(Single delta)
        {
            var center = Body.GlobalPosition;
            var mouse = GetGlobalMousePosition();
            var dir = center.DirectionTo(mouse);
            var angle = Vector2.Right.AngleTo(dir);
            var max = Mathf.Deg2Rad(MaxAngle);
            var min = Mathf.Deg2Rad(MinAngle);
            if (angle > max)
            {
                dir = Vector2.Right.Rotated(max);
            }
            else if (angle < min)
            {
                dir = Vector2.Right.Rotated(min);
            }

            Hood.GlobalPosition = center + dir * Mathf.Max(60f, Mathf.Min(center.DistanceTo(mouse), MaxBowDistance));
            Hood.LookAt(Hood.GlobalPosition + dir);

            if (StartDraw != 0)
            {
                Bownimate();
            }
        }

        private void Bownimate()
        {
            var delta = Time.GetTicksMsec() - StartDraw;
            Hood.SetAnim(Mathf.Min(1f, delta / (DrawTime * 1000f)));
        }

        public override void _Input(InputEvent e)
        {
            //if (Dead is true) return;

            var delta = Time.GetTicksMsec() - StartDraw;

            if (e.IsActionPressed("Shoot"))
            {
                StartDraw = Time.GetTicksMsec();
            }
            else if (e.IsActionReleased("Shoot") && StartDraw > 0)
            {
                SpawnArrow(Mathf.Min(1f, delta / (DrawTime * 1000f)));

                if (Global.paused is false && delta / (DrawTime * 1000f) <= 0.2f)
                {
                    Dangit.Play();
                }

                StartDraw = 0;
                Hood.SetAnim(0f);
            }
        }

        private void SpawnArrow(Single power)
        {
            if (ArrowScene is null)
            {
                GD.PrintErr("Arrow scene is not specified");
                return;
            }

            if (Global.paused is true && Input.IsKeyPressed((Int32)KeyList.Q) is false) return;

            var arrow = ArrowScene.Instance<Arrow>();
            var trans = Hood.SpawnPosition.GlobalTransform;
            GetParent().AddChild(arrow);
            arrow.GlobalTransform = trans;
            arrow.Velocity = trans.x * 2000f * power;
            Hood.Twang.Play();
            Global.arrowsShottened++;
        }

        private void OnHeadHit(object source, GibHitEventArgs e)
        {
            if (e.IsActive is true && Input.IsKeyPressed((Int32)KeyList.Q) is false)
            {
                Kill();
            }
        }

        public void Kill()
        {
            Dead = true;
            foreach (var child in Gibs.GetChildren())
            {
                if (child is Gib gib)
                {
                    gib.Enable();
                }
            }
            Apple.Enable();
        }
    }
}
