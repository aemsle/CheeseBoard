using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BluEditor.GameProject
{
    [DataContract]
    public class ProjectData
    {
        [DataMember]
        public string ProjectName { get; set; }

        [DataMember]
        public string ProjectPath { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        public string FullPath { get => $@"{ProjectPath}{ProjectName}{Project.Extension}"; }

        public byte[] Icon { get; set; }
        public byte[] Thumbnail { get; set; }
    }

    [DataContract]
    public class ProjectDataList
    {
        [DataMember]
        public List<ProjectData> Projects { get; set; }
    }

    public class OpenProjectVM
    {
        private static readonly string m_applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\BluEditor\";
        private static readonly string m_projectDataPath;

        private static readonly ObservableCollection<ProjectData> m_projects = new ObservableCollection<ProjectData>();

        public static ReadOnlyObservableCollection<ProjectData> Projects
        { get; }

        private static void ReadProjectData()
        {
            if (File.Exists(m_projectDataPath))
            {
                List<ProjectData> projects = Utilities.Serializer.ReadFromFile<ProjectDataList>(m_projectDataPath).Projects.OrderByDescending(x => x.Date).ToList();
                m_projects.Clear();
                foreach (ProjectData project in projects)
                {
                    if (File.Exists($@"{project.FullPath}"))
                    {
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.CB\CBLogo.png");
                        project.Thumbnail = File.ReadAllBytes($@"{project.ProjectPath}\.CB\PHThumb.png");
                        m_projects.Add(project);
                    }
                }
            }
        }

        private static void WriteProjectData()
        {
            List<ProjectData> projects = m_projects.OrderBy(x => x.Date).ToList();
            Utilities.Serializer.WriteToFile(new ProjectDataList() { Projects = projects }, m_projectDataPath);
        }

        public static Project Open(ProjectData in_projectData)
        {
            ReadProjectData();
            ProjectData project = m_projects.FirstOrDefault(x => x.FullPath == in_projectData.FullPath);
            if (project != null)
            {
                project.Date = DateTime.Now;
            }
            else
            {
                project = in_projectData;
                project.Date = DateTime.Now;
                m_projects.Add(project);
            }
            WriteProjectData();

            return Project.Load(project.FullPath);
        }

        static OpenProjectVM()
        {
            try
            {
                if (!Directory.Exists(m_applicationDataPath)) Directory.CreateDirectory(m_applicationDataPath);
                m_projectDataPath = $@"{m_applicationDataPath}ProjectData.xml";
                Projects = new ReadOnlyObservableCollection<ProjectData>(m_projects);
                ReadProjectData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Utilities.Logger.Log($"Failed to open project at: {m_projectDataPath}", Utilities.MessageType.ERROR);
            }
        }
    }
}