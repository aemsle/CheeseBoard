using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BluEditor.GameProject
{
    [DataContract] // serialization do-hicker
    public class ProjectTemplate // used for serialization of xml data
    {
        [DataMember] // serialization thingamy-jig
        public string ProjectType { get; set; }

        [DataMember]
        public string ProjectFile { get; set; }

        [DataMember]
        public List<string> ProjectFolders { get; set; }

        [DataMember]
        public byte[] Icon { get; set; }

        [DataMember]
        public byte[] Thumbnail { get; set; }

        [DataMember]
        public string IconFilePath { get; set; }

        [DataMember]
        public string ThumbnailFilePath { get; set; }

        [DataMember]
        public string ProjectFilePath { get; set; }
    }

    // This class reads template XML files and populates a list in the NewProjectView.xaml file
    // It also allows for users to create new projects based off of these templates.
    // #TODO: better docs
    public class NewProjectVM : ViewModelBase
    {
        // #TODO: get a better relative path
        private readonly string m_templatePath = @"..\..\BluEditor\ProjectTemplates";

        private string m_projectName = "New Project";

        public string ProjectName
        {
            get => m_projectName;
            set
            {
                if (value != m_projectName)
                {
                    m_projectName = value;
                    ValidateProjectPath();
                    OnPropertyChanged(m_projectName);
                }
            }
        }

        private string m_projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\CBProjects\";

        public string ProjectPath
        {
            get => m_projectPath;
            set
            {
                if (value != m_projectPath)
                {
                    m_projectPath = value;
                    ValidateProjectPath();
                    OnPropertyChanged(m_projectPath);
                }
            }
        }

        private bool m_isValid;

        public bool IsValid
        {
            get => m_isValid;
            set { if (value != m_isValid) m_isValid = value; OnPropertyChanged(nameof(IsValid)); }
        }

        private string m_errorMessgae;

        public string ErrorMsg
        {
            get { return m_errorMessgae; }
            set
            {
                if (m_errorMessgae != value)
                {
                    m_errorMessgae = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> m_templates = new ObservableCollection<ProjectTemplate>();

        public ReadOnlyObservableCollection<ProjectTemplate> Templates // updates the listbox display with templates found in template directory
        { get; }

        private bool ValidateProjectPath()
        {
            string tempPath = ProjectPath;
            if (!Path.EndsInDirectorySeparator(tempPath)) tempPath += @"\";
            tempPath += $@"{ProjectName}\";

            IsValid = false;

            if (string.IsNullOrWhiteSpace(ProjectName.Trim()))
            {
                ErrorMsg = "Enter project name.";
                return IsValid;
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMsg = "Project name contains invalid characters.";
                return IsValid;
            }
            else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
            {
                ErrorMsg = "Enter valid project path.";
                return IsValid;
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ErrorMsg = "Project path contains invalid characters.";
                return IsValid;
            }
            else if (Directory.Exists(tempPath) && Directory.EnumerateFileSystemEntries(tempPath).Any())
            {
                ErrorMsg = "Project path is not empty.";
                return IsValid;
            }
            else
            {
                ErrorMsg = String.Empty;
                IsValid = true;
            }

            return IsValid;
        }

        public string CreateProject(ProjectTemplate in_template)
        {
            ValidateProjectPath();

            if (!IsValid)
                return String.Empty;

            if (!Path.EndsInDirectorySeparator(ProjectPath)) ProjectPath += @"\";
            string fullpath = $@"{ProjectPath}{ProjectName}\";

            try
            {
                if (!Directory.Exists(fullpath)) Directory.CreateDirectory(fullpath);

                foreach (string subDir in in_template.ProjectFolders)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(fullpath, subDir)));
                }
                DirectoryInfo dirInfo = new DirectoryInfo(fullpath + @".CB\");
                dirInfo.Attributes |= FileAttributes.Hidden;
                File.Copy(in_template.IconFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "CBLogo.png")));
                File.Copy(in_template.ThumbnailFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "PHThumb.png")));

                string projectXml = File.ReadAllText(in_template.ProjectFilePath);
                projectXml = String.Format(projectXml, ProjectName, ProjectPath);
                string projectPath = Path.GetFullPath(Path.Combine(fullpath, $"{ProjectName}{Project.Extension}"));

                File.WriteAllText(projectPath, projectXml);

                return fullpath;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                Utilities.Logger.Log($"Failed to create {in_template}.", Utilities.MessageType.ERROR);
                return String.Empty;
            }
        }

        public NewProjectVM()
        {
            Templates = new ReadOnlyObservableCollection<ProjectTemplate>(m_templates);

            try // attempts to read from template xml files
            {
                List<string> templateFiles = Directory.GetFiles(m_templatePath, "template.xml", SearchOption.AllDirectories).ToList();
                Debug.Assert(templateFiles.Count > 0);
                foreach (string file in templateFiles)
                {
                    ProjectTemplate template = Utilities.Serializer.ReadFromFile<ProjectTemplate>(file);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "CBLogo.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ThumbnailFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "PHThumb.png"));
                    template.Thumbnail = File.ReadAllBytes(template.ThumbnailFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));

                    m_templates.Add(template);
                }
                ValidateProjectPath();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                Utilities.Logger.Log("Failed to create project.", Utilities.MessageType.ERROR);
            }
        }
    }
}