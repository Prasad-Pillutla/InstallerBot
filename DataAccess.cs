using System.Data;
using System.Data.SqlClient;
using System;

namespace SampleBot
{
    class DataAccess
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        public DataAccess()
        {
            //The name of the Azure SQL Database server 
            //Example: myFirstDatabase.database.windows.net 
            builder["Server"] = "gfe.database.windows.net";

            //User ID of the entity attempting to connect to the database 
            builder["User ID"] = "gfeadmin";

            //The password associated the User ID 
            builder["Password"] = "Nvidia12#$";

            //Name of the target database 
            builder["Database"] = "GFE Error Codes";

            //Denotes that the User ID and password are specified in the connection 
            builder["Integrated Security"] = false;

            //By default, Azure SQL Database uses SSL encryption for all data sent between 
            //the client and the database 
            builder["Encrypt"] = true;
        }

        public string queryError(string errorCode)
        {
            String response = "No record found for ErrorCode " + errorCode;
            //Create a SqlConnection from the provided connection string 
            using (SqlConnection connection = new SqlConnection(builder.ToString()))
            {
                //Begin to formulate the command 
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                //Sets the wait time (10 seconds) before terminating the attempt to execute a command and generate an error 
                //Default value is 30 seconds; 
                command.CommandTimeout = 10;

                //Specify the query to be executed 
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Select * from dbo.InstallerErrorCodes where ErrorCode = '" + errorCode + "'";

                //Open connection to database 
                connection.Open();

                //Read data from the query 
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    response = null;
                    response += "[ErrorCode:";
                    response += reader[0];
                    response += "]";
                    response += Environment.NewLine;

                    response += "[Reason:";
                    response += reader[2];
                    response += "]";
                    response += Environment.NewLine;

                    response += "[User Action:";
                    response += reader[1];
                    response += "]";
                    response += Environment.NewLine;
                }

                connection.Close();
                return response;
            }
        }
    }
}