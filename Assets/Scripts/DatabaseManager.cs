using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class DatabaseManager : MonoBehaviour
{
#if UNITY_ANDROID
    [DllImport("sqlite3")]
#endif
    private static extern IDbCommand CreateCommand();

#if UNITY_ANDROID
    [DllImport("sqlite3")]
#endif
    private static extern IDataReader ExecuteReader();

#if UNITY_ANDROID
    [DllImport("sqlite3")]
#endif
    private static extern int ExecuteNonQuery();

#if UNITY_ANDROID
    [DllImport("sqlite3")]
#endif
    private static extern void Open();



    public InputField inputField;
    public InputField outputField;
    IDbConnection db;

    private void Start()
    {
        CreateAndOpenDatabase();
    }

    private void CreateAndOpenDatabase()
    {
        // Open a connection to the database.
        string dbUri = "URI=file:HeartburnDatabase.sqlite"; // Aquí definimos el nombre de la base de datos, en este caso se llama "MyDatabase"
        db = new SqliteConnection(dbUri);
        db.Open();
        // Aquí creamos la tabla 

        IDbCommand dbCommandCreateTable = db.CreateCommand();
        dbCommandCreateTable.CommandText = @"CREATE TABLE IF NOT EXISTS 
                                                    Records (
                                                      id INTEGER PRIMARY KEY, 
                                                      id_record_food INTEGER, 
                                                      FOREIGN KEY(id_record_food) REFERENCES RecordsFoods(id), 
                                                      id_record_event INTEGER, 
                                                      FOREIGN KEY(id_record_event) REFERENCES RecordsEvents(id), 
                                                      record_date date(YYYY-MM-DD HH:MM),
                                                      intensity INTEGER(1)
                                                      water_so_far REAL
                                                      observations VARCHAR(255)
                                                      medicine BOOLEAN
                                                    )";
        dbCommandCreateTable.ExecuteReader();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Foods (id INTEGER PRIMARY KEY, name VARCHAR UNIQUE)";
        dbCommandCreateTable.ExecuteNonQuery();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS RecordsFoods (id INTEGER PRIMARY KEY, id_food INTEGER, FOREIGN KEY(id_food) REFERENCES Foods(id))";
        dbCommandCreateTable.ExecuteNonQuery();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Events (id INTEGER PRIMARY KEY, name VARCHAR UNIQUE)";
        dbCommandCreateTable.ExecuteNonQuery();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS RecordsEvents (id INTEGER PRIMARY KEY, id_event INTEGER, FOREIGN KEY(id_event) REFERENCES Events(id))";
        dbCommandCreateTable.ExecuteNonQuery();


        // return dbConnection;
    }

    public void OpenDatabase()
    {
        string dbUri = "URI=file:HeartburnDatabase.sqlite"; // Aquí definimos el nombre de la base de datos, en este caso se llama "MyDatabase"
        db = new SqliteConnection(dbUri);
        db.Open();

    }

    public void ExecuteQuery()
    {
        if (db == null) OpenDatabase();

        IDbCommand dbCommand = db.CreateCommand();
        dbCommand.CommandText = inputField.text;

        IDataReader reader = dbCommand.ExecuteReader(); // esta instrucción creo que hace efectiva la query
        
        while (reader.Read())
        {
            print(reader.GetString(0));
        }
    }

    public void ExecuteOtherQuery(string query)
    {
        if (db == null) OpenDatabase();
        IDbCommand dbCommand = db.CreateCommand();
        dbCommand.CommandText = query;

        IDataReader reader = dbCommand.ExecuteReader(); // esta instrucción creo que hace efectiva la query
        outputField.text = "";
        while (reader.Read())
        {
           
            //outputField.text += reader.GetString(0);
            //outputField.text += reader.GetString(0);
            outputField.text += reader.ToString()+"\n\n";
        }
    }

}
