using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Util;
using Android.Content;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

namespace Operation_Update_Application
{
    class Data
    {
        SqliteConnection dbConn;


        public Data()
        {

        }

        /// <summary>
        /// Method used for setting up the sqlite database
        /// </summary>
        /// <returns></returns>
        public SqliteConnection SetUpDatabase()
        {
            //set the path of where the db will be kept.
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            //combine with the file name
            string combined = Path.Combine(path, "OperationDatabase.sqlite");

            //check if it exists, if it does not create the file and set the connection.
            if (!File.Exists(combined))
            {
                SqliteConnection.CreateFile(combined);
                dbConn = new SqliteConnection("Data Source =" + combined);
                dbConn.Open();
                //call the recreate db method to set the initial data.
                RecreateDatabase();
            }
            //otherwise if the file exists just open the connection
            else
            {
                dbConn = new SqliteConnection("Data Source =" + combined);
                dbConn.Open();
            }

            //return the connection
            return dbConn;
        }

        /// <summary>
        /// Used to set up the data or reset up the data in the data base with default informaiton
        /// </summary>
        public void RecreateDatabase()
        {
            SqliteCommand command = new SqliteCommand(dbConn);

            string nonQuery;

            //drop tables
            nonQuery = "DROP TABLE IF EXISTS USER";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();
            nonQuery = "DROP TABLE IF EXISTS LU_OPERATION_TYPE";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();
            nonQuery = "DROP TABLE IF EXISTS LU_OPERATION_STATUS";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();
            nonQuery = "DROP TABLE IF EXISTS OPERATION";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            //create tables
            nonQuery = "CREATE TABLE USER (USERNAME VARCHAR PRIMARY KEY, PASSWORD VARCHAR NOT NULL)";

            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            nonQuery = "CREATE TABLE LU_OPERATION_TYPE (OPERATION_TYPE_CODE VARCHAR PRIMARY KEY, OPERATION_TYPE_DESCRIPTION VARCHAR NOT NULL)";

            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            nonQuery = "CREATE TABLE LU_OPERATION_STATUS (OPERATION_STATUS_CODE VARCHAR PRIMARY KEY, OPERATION_STATUS_DESCRIPTION VARCHAR NOT NULL)";

            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            nonQuery = "CREATE TABLE OPERATION (JOB_NO INTEGER PRIMARY KEY, START_DATE DATE  NOT NULL, OPERATION_TYPE VARCHAR NOT NULL, OPERATION_STATUS VARCHAR NOT NULL, RESPONSIBLE_USER VARCHAR NOT NULL, OPERATION_COMPLETION_PERCENTAGE INTEGER NOT NULL," +
                "FOREIGN KEY(OPERATION_TYPE) REFERENCES LU_OPERATION_TYPE(OPERATION_TYPE_CODE), FOREIGN KEY(OPERATION_STATUS) REFERENCES LU_OPERATION_STATUS(OPERATION_STATUS_CODE),FOREIGN KEY(RESPONSIBLE_USER) REFERENCES USER(USERNAME))";

            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            //insert data
            nonQuery = "INSERT INTO user (username,password) VALUES ('john','john'),('smith','smith')";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            nonQuery = "INSERT INTO LU_OPERATION_TYPE VALUES ('BURN','Burning'),('SEED','Seeding'),('HARV','Harvesting')";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            nonQuery = "INSERT INTO LU_OPERATION_STATUS VALUES ('PL','Planned'),('CU','Current'),('CO','Complete')";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

            nonQuery = "INSERT INTO OPERATION VALUES (47215,'2018-01-20','BURN','CU','john',15),(47217,'2018-02-20','HARV','PL','john',0),(47220,'2018-04-15','SEED','CO','smith',100),(47219,'2018-05-19','HARV','PL','smith',0)," +
                "(47216,'2018-02-12','BURN','PL','john',0),(47218,'2018-01-13','HARV','CO','john',100),(47221,'2018-04-17','SEED','CU','smith',57),(47222,'2018-06-21','HARV','CU','smith',16)";
            command.CommandText = nonQuery;
            command.ExecuteNonQuery();

        }

    }
}