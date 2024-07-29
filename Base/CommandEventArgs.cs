using System;

namespace Base{
    public class CommandEventArgs : EventArgs{
        public Command Command { get; private set; }

        public CommandEventArgs(string text){
            this.Command = new Command(text);
        }
    }
}
