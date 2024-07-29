using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Query.Controls{
    sealed partial class QueryHistoryLog : UserControl{
        public enum EntryType{
            UserInput, CommandResult, Information, Error
        }

        private static readonly Dictionary<EntryType, Color> EntryColorMap = new Dictionary<EntryType, Color>{
            { EntryType.UserInput, Color.FromArgb(160, 160, 160) },
            { EntryType.CommandResult, Color.FromArgb(240, 240, 240) },
            { EntryType.Information, Color.FromArgb(160, 255, 140) },
            { EntryType.Error, Color.FromArgb(255, 40, 40) }
        };

        public QueryHistoryLog(){
            InitializeComponent();
        }

        public void AddEntry(string text, EntryType type){
            int width = container.Width-SystemInformation.VerticalScrollBarWidth;

            Label label = new Label{
                AutoSize = true,
                Font = container.Font,
                ForeColor = EntryColorMap[type],
                Text = text,
                Margin = new Padding(0,1,0,1),
                MaximumSize = new Size(width, 0),
                Width = width
            };

            container.Controls.Add(label);
            container.AutoScrollPosition = new Point(0, container.VerticalScroll.Maximum);
        }

        public void ClearEntries(){
            container.Controls.Clear();
        }
    }
}
