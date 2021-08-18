using mroot_lib;
using System.Collections.Generic;
using System.Diagnostics;
using Wox.Plugin;

namespace Evernote
{

    public class ShowNoteActionDescriptor
    {
        public string Name { get; set; }
        public string Stack { get; set; }
        public string Intitle { get; set; }
        public string Notebook { get; set; }
        public string Tag { get; set; }
    }

    public class ExecuteActionDescriptor
    {
        public string Name { get; set; }
        public string Args { get; set; }
        public string ExecPath { get; set; }
    }

    public class Actions
    {
        public static string EvernoteScriptExec => mroot.substitue_enviro_vars("||evernote||");

        public static string EvernoteSearchScript => mroot.substitue_enviro_vars("||bin||\\AHK_script\\evernote\\evernote_search_for.exe");

        public List<ShowNoteActionDescriptor> ShowNotes { get; set; } = new List<ShowNoteActionDescriptor>();
        public List<ExecuteActionDescriptor> Execute { get; set; } = new List<ExecuteActionDescriptor>();

        public static Result MakeCommandFromShowNoteDescriptor(ShowNoteActionDescriptor desc)
        {
            Result result = new Result();
            result.Title = desc.Name;
            result.SubTitle = GetShowNoteCommand(desc);
            result.Score = 50;
            result.IcoPath = "Images\\evernote.png";
            result.Action = e =>
            {
                Process process = new Process();
                process.StartInfo.FileName = Actions.EvernoteScriptExec;
                process.StartInfo.Arguments = GetShowNoteCommand(desc); 
                process.Start();
                return true;
            };

            return result;
        }

        public static Result MakeCommandFromExecuteActionDescriptor(ExecuteActionDescriptor desc)
        {
            Result result = new Result();
            result.Title = desc.Name;
            result.SubTitle = GetExecuteCommand(desc);
            result.Score = 50;
            result.IcoPath = "Images\\Program.png";
            result.Action = e =>
            {
                Process process = new Process();
                process.StartInfo.FileName = mroot.substitue_enviro_vars( desc.ExecPath);
                process.StartInfo.Arguments = mroot.substitue_enviro_vars(desc.Args);
                process.Start();
                return true;
            };

            return result;
        }

        public static string GetShowNoteCommand(ShowNoteActionDescriptor desc)
        {
            string tag = desc.Tag.IsNullOrWhitespace() ? "" : $" tag:{desc.Tag}";
            string notebook = desc.Notebook.IsNullOrWhitespace() ? "" : $" notebook:{desc.Notebook}";
            string intitle = desc.Intitle.IsNullOrWhitespace() ? "" : $" intitle:{desc.Intitle}";
            string stack = desc.Stack.IsNullOrWhitespace() ? "" : $" stack:{desc.Stack}";

            return $"showNotes /q \" {tag}{intitle}{notebook}{stack} \"";
        }

        public static string GetExecuteCommand(ExecuteActionDescriptor desc)
        {
            string args = desc.Args.IsNullOrWhitespace() ? "" : $" args:{desc.Args}";

            return $"{desc.ExecPath} {args}";
        }


    }

}
