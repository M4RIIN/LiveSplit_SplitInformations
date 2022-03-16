using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public class SplitsInformationsComponent : IComponent
    {
        // This is how we will access all the settings that the user has set.
        public SplitsInformationsSettings Settings { get; set; }
        // This object contains all of the current information about the splits, the timer, etc.
        protected LiveSplitState CurrentState { get; set; }

        public string ComponentName => "Splits Informations";

        public float HorizontalWidth => 0;
        public float MinimumWidth => 0;
        public float VerticalHeight => 0;
        public float MinimumHeight => 0;

        public float PaddingTop => 0;
        public float PaddingLeft => 0;
        public float PaddingBottom => 0;
        public float PaddingRight => 0;

        protected bool ResetChancesValid { get; set; }
        protected bool CurrentSplitValid { get; set; }

        // I'm going to be honest, I don't know what this is for, but I know we don't need it.
        public IDictionary<string, Action> ContextMenuControls => null;

        // The list that stores the chance of resetting on each split.
        protected List<float> ResetChances { get; set; }
        // The reset chance of the current split.
        protected float CurrentResetChance { get; set; }

        protected Form Form { get; set; }

        protected Label Label { get; set; }

        // This function is called when LiveSplit creates your component. This happens when the
        // component is added to the layout, or when LiveSplit opens a layout with this component
        // already added.
        public SplitsInformationsComponent(LiveSplitState state)
        {
            Settings = new SplitsInformationsSettings();
            //InternalComponent = new InfoTextComponent("ok", "ok");

            state.OnStart += state_OnStart;
            state.OnSplit += state_OnSplitChange;
            state.OnSkipSplit += state_OnSplitChange;
            state.OnUndoSplit += state_OnSplitChange;
            state.OnReset += state_OnReset;
            CurrentState = state;
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
        }

        // We will be adding the ability to display the component across two rows in our settings menu.
        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {

        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        // This is the function where we decide what needs to be displayed at this moment in time,
        // and tell the internal component to display it. This function is called hundreds to
        // thousands of times per second.
        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {

        }
     
       
        // This function is called when the component is removed from the layout, or when LiveSplit
        // closes a layout with this component in it.
        public void Dispose()
        {
            CurrentState.OnStart -= state_OnStart;
            CurrentState.OnSplit -= state_OnSplitChange;
            CurrentState.OnSkipSplit -= state_OnSplitChange;
            CurrentState.OnUndoSplit -= state_OnSplitChange;
            CurrentState.OnReset -= state_OnReset;
        }

        // I do not know what this is for.
        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();

        void state_OnStart(object sender, EventArgs e)
        {
            Form = new Form();
            Form.Size = new System.Drawing.Size(1200,150);
            Form.BackColor = Color.Green;
            Label = new Label();
            Form.Text = GetNewWindowName();
            Label.Text = Settings.InformationsList.First();
            Label.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            Label.AutoSize = false;
            Label.Width = 1200;
            Label.Height = 150;
            Label.ForeColor = Color.White;
            Label.MinimumSize = new System.Drawing.Size(150,150);
            Label.Font = new Font("Arial",24,FontStyle.Bold);
            Form.Controls.Add(Label);
            Form.Show();
        }

        private string GetNewWindowName()
        {
            return "Informations Split of " + CurrentState.Run.GameName; ;
        }

        void state_OnSplitChange(object sender, EventArgs e)
        {
            Label.Text = Settings.InformationsList[CurrentState.CurrentSplitIndex];
        }

        void state_OnReset(object sender, TimerPhase e)
        {
            Form.Close();
        }

    }


}


