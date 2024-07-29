using System;
using System.Collections.Generic;
using System.Linq;
using Base;

namespace Query.Core{
    sealed class CommandProcessor{
        private readonly Dictionary<string, IApp> appNames = new Dictionary<string, IApp>(8);
        private readonly HashSet<IApp> appSet = new HashSet<IApp>();
        
        public Func<string, bool> SingleTokenProcessor { get; set; }

        public void AddApp<T>() where T : IApp, new(){
            IApp app = new T();

            foreach(string name in app.RecognizedNames){
                appNames.Add(name, app);
                appSet.Add(app);
            }
        }

        public string Run(Command cmd){
            cmd = cmd.ReplaceBrackets(match => Run(new Command(match.Groups[1].Value)));

            string appName = cmd.PotentialAppName;
            IApp app;
            
            if (appName != null && appNames.TryGetValue(appName.ToLowerInvariant(), out app)){
                return app.ProcessCommand(new Command(cmd.Text.Substring(appName.Length+1)));
            }

            if (cmd.IsSingleToken && SingleTokenProcessor != null && SingleTokenProcessor(cmd.Text)){
                return null;
            }

            var list = appSet.Select(iapp => new { App = iapp, Confidence = iapp.GetConfidence(cmd) }).OrderByDescending(obj => obj.Confidence).Where(obj => obj.Confidence != MatchConfidence.None).ToList();

            if (list.Count == 0){
                throw new CommandException("Could not find any suitable app, please write the app name and press Up Arrow.");
            }
            else if (list.Count == 1){
                app = list[0].App;
            }
            else{
                List<IApp> plausible = new List<IApp>{ list[0].App };
                MatchConfidence topConfidence = list[0].Confidence;

                for(int index = 1; index < list.Count; index++){
                    if (list[index].Confidence == topConfidence){
                        plausible.Add(list[index].App);
                    }
                }

                if (plausible.Count == 1){
                    app = plausible.First();
                }
                else{
                    throw new CommandException("Command is ambiguous, please write the app name and press Up Arrow. Suggested apps: "+string.Join(", ",plausible.Select(iapp => iapp.RecognizedNames.First())));
                }
            }

            return app.ProcessCommand(cmd);
        }
    }
}
