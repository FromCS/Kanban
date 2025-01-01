using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Canvas.Model.ProjectModel;
using Microsoft.Data.Sqlite;

namespace Canvas.Database;

public static class MainDatabase
{
    private static string _filepath = Directory.GetCurrentDirectory() + "\\MainData.db";
    private static string tableName = "Проекты";
    private static SqliteConnection _connection = new SqliteConnection($@"Data Source={_filepath}");

    public static void AddNewProject(IProject project)
    {
        
        _connection.Open();
        string sqlRequest = $"INSERT INTO {tableName} (Наименование, Категория, Приоритет) VALUES (@Name, @Category, @Prior)";
        using var command = new SqliteCommand(sqlRequest, _connection);
        command.Parameters.AddWithValue("@Name", project.Name);
        command.Parameters.AddWithValue("@Category", project.WorkCategory);
        command.Parameters.AddWithValue("@Prior", project.Priority);
        command.ExecuteNonQuery();
    }

    public static void RemoveProject(IProject project)
    {
        
    }

    public static void ToArchiveProject(IProject project)
    {
        
    }

    public static ObservableCollection<IProject> GetAllProjects()
    {
        ObservableCollection<IProject> projects = new ObservableCollection<IProject>();
        _connection.Open();
        string sqlRequest = $"SELECT * FROM {tableName}";
        using var command = new SqliteCommand(sqlRequest, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            string projectName = reader.GetString(1);
            string category = reader.GetString(2);
            string priority = reader.GetString(3);
            projects.Add(new Project() {Name = projectName, WorkCategory = category, Priority = priority});
        }
        return projects;
    }
}