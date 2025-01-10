using System;
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
    public static void AddProjectLegendTable(string projectName, ObservableCollection<Step> legend)
    {
        _connection.Open();
        string legendTableName = $"{projectName}Legend";
        CreateLegendTable(legendTableName);
        legend.ToList().ForEach(step =>
        {
            string sqlRequest = $"INSERT INTO '{legendTableName}' (ID, Наименование_шага, parentID, isDone) VALUES (@id, @Name, @parentID, @isDone)";
            using var command = new SqliteCommand(sqlRequest, _connection);
            command.Parameters.AddWithValue("@id", step.ID);
            command.Parameters.AddWithValue("@Name", step.StepName);
            command.Parameters.AddWithValue("@parentID", step.ParentId ?? 0);
            command.Parameters.AddWithValue("@isDone", step.IsDone ? 1 : 0);
            command.ExecuteNonQuery();
        });
    }
    private static void CreateLegendTable(string legendTableName)
    {
        string sqlRequest = $"CREATE TABLE '{legendTableName}' (ID INTEGER, Наименование_шага TEXT, parentID INTEGER, isDone INTEGER)";
        using var command = new SqliteCommand(sqlRequest, _connection);
        command.ExecuteNonQuery();
    }

    public static ObservableCollection<Step> GetProjectLegend(string projectName)
    {
        var result = new ObservableCollection<Step>();
        _connection.Open();
        string legendName = projectName + "Legend";
        string sqlRequest = $"SELECT * FROM '{legendName}'";
        using var command = new SqliteCommand(sqlRequest, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string stepName = reader.GetString(1);
            int parentID = reader.GetInt32(2);
            bool isDone = reader.GetInt32(3) == 0 ? false : true;
            var step = new Step() { ID = id, StepName = stepName, ParentId = parentID, IsDone = isDone};
            result.Add(step);
        }

        result = UtilsDatabase.CreateLegendTreeStructure(result);
        return result;
    }

    public static void ToArchiveProject(IProject project)
    {
        
    }

    
}