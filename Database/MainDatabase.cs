using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using Canvas.Model;
using Canvas.Model.ProjectModel;
using Microsoft.Data.Sqlite;

namespace Canvas.Database;

public static class MainDatabase
{
    private static string _filepath = Directory.GetCurrentDirectory() + "\\MainData.db";
    private static string tableName = "Проекты";
    private static SqliteConnection _connection = new SqliteConnection($@"Data Source={_filepath}");

    private static void CreateLegendTable(string legendTableName)
    {
        string sqlRequest = $"CREATE TABLE '{legendTableName}' (ID INTEGER, Наименование_шага TEXT, parentID INTEGER)";
        using var command = new SqliteCommand(sqlRequest, _connection);
        command.ExecuteNonQuery();
    }

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

    public static void AddProjectLegendTable(string projectName, ObservableCollection<Step> legend)
    {
        _connection.Open();
        string legendTableName = $"{projectName}Legend";
        CreateLegendTable(legendTableName);
        legend.ToList().ForEach(step =>
        {
            string sqlRequest = $"INSERT INTO '{legendTableName}' (ID, Наименование_шага, parentID) VALUES (@id, @Name, @parentID)";
            using var command = new SqliteCommand(sqlRequest, _connection);
            command.Parameters.AddWithValue("@id", step.ID);
            command.Parameters.AddWithValue("@Name", step.StepName);
            command.Parameters.AddWithValue("@parentID", step.ParentId ?? 0);
            command.ExecuteNonQuery();
        });
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