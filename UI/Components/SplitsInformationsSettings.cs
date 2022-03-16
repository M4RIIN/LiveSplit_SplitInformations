using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    public partial class SplitsInformationsSettings : UserControl
    {
        public int NbrOfRows { get; set; }
        public List<string> InformationsList { get; set; }

        public LayoutMode Mode { get; set; }
        public SplitsInformationsSettings()
        {
            InitializeComponent();
            NbrOfRows = 0;
            InformationsList = new List<string>();
        }

        private void AddInformationsLayout()
        {
            tableLayoutPanel.RowCount = NbrOfRows;
            Label label = new Label();
            label.Text = "Enter some informations about split number " + (NbrOfRows+1);
            label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label.AutoSize = true;
            TextBox textBox = new TextBox();
            textBox.Name = "Information" + NbrOfRows;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox.AutoSize = true;
            tableLayoutPanel.Controls.Add(label,0,NbrOfRows +1);
            tableLayoutPanel.Controls.Add(textBox,1,NbrOfRows+1);
        }
        
        private void initLayout()
        {
            tableLayoutPanel.RowCount = NbrOfRows;
            Button button = new Button();
            button.Name = "Add";
            button.Text = "Add informations";
            button.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button.AutoSize = true;
            button.Click += buttonAdd_Click;
            Button button2 = new Button();
            button2.Name = "Save";
            button2.Text = "Save";
            button2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button2.AutoSize = true;
            button2.Click += saveButton_Click;
            //tableLayoutPanel2.Controls.Add(button, 0, 0);
            //tableLayoutPanel2.Controls.Add(button2, 1, 0);
            for (int i = 0; i < NbrOfRows; i++)
            {
            Label label = new Label();
            label.Text = "Enter some informations about split number " + (i +1);
            label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label.AutoSize = true;
            TextBox textBox = new TextBox();
            textBox.Name = "Information" + (i);
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            textBox.AutoSize = true;
            textBox.Text = InformationsList[i];
            tableLayoutPanel.Controls.Add(label, 0, (i +1));
            tableLayoutPanel.Controls.Add(textBox, 1, (i +1));
            }

        }

        private void SplitsInformationsSettings_Load(object sender, EventArgs e)
        {
            tableLayoutPanel.Controls.Clear();
            initLayout();
        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            int nbrSettingsAdded = 0;
            SettingsHelper.CreateSetting(document, parent, "Version", "1.0");
            SettingsHelper.CreateSetting(document, parent, "NbrInformations", NbrOfRows);
            nbrSettingsAdded = 1;
            for (int i = 0; i < NbrOfRows; i++)
            {
                SettingsHelper.CreateSetting(document, parent, "Split"+i+"Informations", InformationsList[i]);
                nbrSettingsAdded++;
            }
            return nbrSettingsAdded;
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
            NbrOfRows =  SettingsHelper.ParseInt(element["NbrInformations"]);
            InformationsList.Clear();
            for (int i = 0; i < NbrOfRows; i++)
            {
               InformationsList.Add( SettingsHelper.ParseString(element["Split"+i+"Informations"]));
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            AddInformationsLayout();
            NbrOfRows++;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            InformationsList.Clear();
            for(int i = 0; i < NbrOfRows; i++)
            {
                InformationsList.Add(((TextBox)tableLayoutPanel.Controls["Information" + i]).Text);
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
           if(InformationsList.Count == NbrOfRows && NbrOfRows != 0)
            {
                InformationsList.RemoveAt(InformationsList.Count - 1);
                //Two times, to remove label and TextBox
                tableLayoutPanel.Controls.RemoveAt(tableLayoutPanel.Controls.Count-1);
                tableLayoutPanel.Controls.RemoveAt(tableLayoutPanel.Controls.Count - 1);
                NbrOfRows--;
            }
        }
    }
}
