using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Canvas.Model;
using Microsoft.Data.Sqlite;

namespace Canvas.Database;

public static class TemplatesDatabase
{
    private static string _filepath = Directory.GetCurrentDirectory() + "\\Templates.db";
    private static SqliteConnection _connection = new SqliteConnection($@"Data Source={_filepath}");
    private static void CreateLegendTemplateTable(string legendTableName)
    {
        string sqlRequest = $"CREATE TABLE '{legendTableName}' (ID INTEGER, Наименование_шага TEXT, parentID INTEGER, isDone INTEGER)";
        using var command = new SqliteCommand(sqlRequest, _connection);
        command.ExecuteNonQuery();
    }

    private static ObservableCollection<Step> GetLegendTemplate(string templateName)
    {
        ObservableCollection<Step> templateLegend = new ObservableCollection<Step>();
        string query = $"SELECT * FROM '{templateName}'";
        using var command = new SqliteCommand(query, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string stepName = reader.GetString(1);
            int parentID = reader.GetInt32(2);
            var step = new Step() { StepName = stepName, ID = id, ParentId = parentID };
            templateLegend.Add(step);
        }
        templateLegend = UtilsDatabase.CreateLegendTreeStructure(templateLegend);
        return templateLegend;
    }

    public static void AddLegendTemplateTable(string legendName, ObservableCollection<Step> legend)
    {
        _connection.Open();
        CreateLegendTemplateTable(legendName);
        legend.ToList().ForEach(step =>
        {
            string sqlRequest = $"INSERT INTO '{legendName}' (ID, Наименование_шага, parentID, isDone) VALUES (@id, @Name, @parentID, @isDone)";
            using var command = new SqliteCommand(sqlRequest, _connection);
            command.Parameters.AddWithValue("@id", step.ID);
            command.Parameters.AddWithValue("@Name", step.StepName);
            command.Parameters.AddWithValue("@parentID", step.ParentId ?? 0);
            command.Parameters.AddWithValue("@isDone", 0);
            command.ExecuteNonQuery();
        });
    }

    public static void GetLegendTemplates(ref ObservableCollection<LegendTemplate> templates)
    {
        _connection.Open();
        string query = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name!='sqlite_sequence' ORDER BY name";
        using var command = new SqliteCommand(query, _connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            string templateName = reader.GetString(0);
            ObservableCollection<Step> templateLegend = GetLegendTemplate(templateName);
            templates.Add(new LegendTemplate(){LegendName = templateName, Legend = templateLegend});
        }
    }
}