using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using BluEditor.Utilities;

namespace BluEditor.GameProject
{
    [DataContract(Name = "Game")]
    public class Project : ViewModelBase
    {
        public static string Extension { get; } = ".CBProj";

        [DataMember]
        public string ProjectName { get; private set; } = "New Project";

        [DataMember]
        public string ProjectPath { get; private set; }

        public string FullPath => $@"{ProjectPath}{ProjectName}\{ProjectName}{Extension}";

        [DataMember(Name = "Scenes")]
        private ObservableCollection<Scene> m_scenes = new ObservableCollection<Scene>();

        public ReadOnlyObservableCollection<Scene> Scenes
        { get; private set; }

        private Scene m_activeScene;

        public Scene ActiveScene
        {
            get => m_activeScene;
            set
            {
                if (m_activeScene != value)
                {
                    m_activeScene = value;
                    OnPropertyChanged(nameof(ActiveScene));
                }
            }
        }

        public static Project Current => (Project)Application.Current.MainWindow.DataContext;

        public static UndoRedo UndoRedo { get; } = new Utilities.UndoRedo();

        public bool IsDirty { get; set; } = false;

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand AddSceneCommand { get; private set; }
        public ICommand RemoveSceneCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        private void AddSceneInternal(string in_sceneName)
        {
            Debug.Assert(!string.IsNullOrEmpty(in_sceneName.Trim()));
            m_scenes.Add(new Scene(this, in_sceneName));
        }

        private void RemoveSceneInternal(Scene in_scene)
        {
            Debug.Assert(m_scenes.Contains(in_scene));
            m_scenes.Remove(in_scene);
        }

        public static Project Load(string in_file)
        {
            Debug.Assert(File.Exists(in_file));
            Logger.Log($"Loading from: {in_file}");
            return Utilities.Serializer.ReadFromFile<Project>(in_file);
        }

        public void Unload()
        {
            UndoRedo.Reset();
        }

        public static void Save(Project in_project)
        {
            Debug.Assert(in_project != null);
            Utilities.Serializer.WriteToFile<Project>(in_project, in_project.FullPath);
            Logger.Log($"Saving {in_project.ProjectName} to: {in_project.FullPath}");
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext in_context)
        {
            if (m_scenes != null)
            {
                Scenes = new ReadOnlyObservableCollection<Scene>(m_scenes);
                OnPropertyChanged(nameof(Scenes));
            }
            ActiveScene = Scenes.FirstOrDefault(x => x.IsActive);

            AddSceneCommand = new RelayCommand<object>(x =>
            {
                AddSceneInternal($"New Scene ({m_scenes.Count})");
                Scene newScene = m_scenes.Last();
                int sceneIndex = m_scenes.Count - 1;
                UndoRedo.Add(new UndoRedoAction(
                    $"Add {newScene.SceneName}",
                    () => RemoveSceneInternal(newScene),
                    () => m_scenes.Insert(sceneIndex, newScene)
                    ));
            });

            RemoveSceneCommand = new RelayCommand<Scene>(x =>
            {
                var sceneIndex = m_scenes.IndexOf(x);
                RemoveSceneInternal(x);
                UndoRedo.Add(new UndoRedoAction(
                    $"Remove {x.SceneName}",
                    () => m_scenes.Insert(sceneIndex, x),
                    () => RemoveSceneInternal(x)
                    ));
            }, x => !x.IsActive); // dont remove scenes that are active

            UndoCommand = new RelayCommand<object>(x => UndoRedo.Undo());
            RedoCommand = new RelayCommand<object>(x => UndoRedo.Redo());
            SaveCommand = new RelayCommand<object>(x => Save(this));
        }

        public Project(string in_projectName, string in_projectPath)
        {
            ProjectName = in_projectName;
            ProjectPath = in_projectPath;

            OnDeserialized(new StreamingContext());
        }
    }
}