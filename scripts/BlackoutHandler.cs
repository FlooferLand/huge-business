using Godot;

namespace Project
{
    public class BlackoutHandler : ColorRect
    {
        // Nodes
        Timer timer;

        // Signals
        [Signal] public delegate void end(string reason);

        // Variables
        public enum STATE { None, FadingIn, FadingOut };
        public STATE state = STATE.None;
        public string id;

        // -- Functions
        public override void _Ready()
        {
            Global.blackoutHandler = this;
            timer = GetNode<Timer>("Timer");
            timer.Connect("timeout", this, nameof(_Timeout));
        }

        public override void _Process(float delta)
        {
            switch (state)
            {
                case STATE.FadingIn:
                    Color = new Color(
                        Color.r, Color.g, Color.b,
                        Mathf.Lerp(Color.a, 1f, delta * 5f)
                    );
                    break;
                case STATE.FadingOut:
                    Color = new Color(
                        Color.r, Color.g, Color.b,
                        Mathf.Lerp(Color.a, 0f, delta * 10f)
                    );
                    if (Color.a <= 0.01f)
                        state = STATE.None;
                    break;
                case STATE.None:
                    break;
            }
        }

        void _Timeout()  // blackout starts fading out
        {
            state = STATE.FadingOut;
            EmitSignal(nameof(end), id);
        }

        public void TriggerBlackout(string id)  // blackout starts
        {
            this.id = id;
            state = STATE.FadingIn;
            timer.Start();
        }
    }
}