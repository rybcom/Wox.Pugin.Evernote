using System.Collections.Generic;
using System.Diagnostics;
using Wox.Plugin;
using mroot_lib;
using Wox.Infrastructure.Storage;
using System.Windows.Forms;
using System.IO;

namespace Evernote
{

    static class Paths
    {
        public static string EtcFolderPath => mroot_lib.Paths.SystemFolders.etc;
        public static string EvernoteConfigFolderPath => Path.Combine(EtcFolderPath, "wox_plugins", "evernote");
        public static string ConfigFilePath => Path.Combine(EvernoteConfigFolderPath, "Actions.json");
    }

    public class Main : IPlugin, IReloadable
    {
        #region members

        private PluginJsonStorage<Actions> _actionStorage;
        private static Actions _actions;

        #endregion

        #region wox overrides

        public void Init(PluginInitContext context)
        {
            _actionStorage = new PluginJsonStorage<Actions>();
            _actionStorage.DirectoryPath = Paths.EvernoteConfigFolderPath;
            _actionStorage.FilePath = Paths.ConfigFilePath;
            _actions = _actionStorage.Load();
        }

        public void ReloadData()
        {
            _actions = _actionStorage.Load();
        }

        public List<Result> Query(Query query)
        {
            List<Result> resultList = new List<Result>();

            foreach (var action in _actions.ShowNotes)
            {
                if (StringTools.IsEqualOnStart(query.Search, action.Name))
                {
                    resultList.Add(Actions.MakeCommandFromShowNoteDescriptor(action));
                }
            }

            foreach (var action in _actions.Execute)
            {
                if (StringTools.IsEqualOnStart(query.Search, action.Name))
                {
                    resultList.Add(Actions.MakeCommandFromExecuteActionDescriptor(action));
                }
            }

            if (query.Search.Length > 0)
            {
                AddCommand_SearchAll(resultList, query.Search);
            }
            AddCommand_OpenEvernote(resultList);


            return resultList;
        }

        #endregion

        #region commands

        private void AddCommand_SearchAll(List<Result> resultList, string text)
        {
            Result todo = new Result();
            todo.Title = text;
            todo.IcoPath = "Images\\evernote.png";
            todo.SubTitle = $"Search {text} in all notes";
            todo.Score = 30;

            todo.Action = e =>
            {
                Process process = new Process();
                process.StartInfo.FileName = Actions.EvernoteSearchScript;
                process.StartInfo.Arguments = $"\"{text}\"";
                process.Start();
                return true;
            };

            resultList.Add(todo);
        }

        private void AddCommand_OpenEvernote(List<Result> resultList)
        {
            Result todo = new Result();
            todo.Title = "Evernote";
            todo.IcoPath = "Images\\evernote.png";
            todo.SubTitle = "Open evernote";
            todo.Score = 0;

            todo.Action = e =>
            {
                Process process = new Process();
                process.StartInfo.FileName = Actions.EvernoteScriptExec; ;
                process.Start();
                return true;
            };

            resultList.Add(todo);
        }

        #endregion

        #region private methods

        #endregion

    }
}
