using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Hotel
{
    public class DBClient
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HotelDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private Facility GetFacility(SqlConnection con, int Facility_ID)
        {
            Console.WriteLine("Calling -> GetFacility");

            string queryStringFacility = $"SELECT * FROM Facility WHERE Facility_ID = {Facility_ID}";
            Console.WriteLine($"SQL applied: {queryStringFacility}");

            SqlCommand cmd = new SqlCommand(queryStringFacility, con);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine($"Finding facility ID: {Facility_ID}");

            if (!reader.HasRows)
            {
                Console.WriteLine("No facilities in DB");
                reader.Close();
                return null;
            }

            Facility facility = null; //TODO kan dette slettes eller er det kritisk?
            if (reader.Read())
            {
                facility = new Facility()
                {
                    Facility_ID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
                Console.WriteLine(facility);
            }

            reader.Close();
            Console.WriteLine();

            return facility;
        }

        private List<Facility> GetAllFacilities(SqlConnection con)
        {
            Console.WriteLine("Calling -> ListAllFacilities");

            string queryStringAllFacilities = "SELECT * FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            SqlCommand cmd = new SqlCommand(queryStringAllFacilities, con);
            SqlDataReader reader = cmd.ExecuteReader();

            Console.WriteLine("List of all facilities:");

            if (!reader.HasRows)
            {
                Console.WriteLine("No facilities in DB");
                reader.Close();
                return null;
            }

            List<Facility> facilities = new List<Facility>();
            while (reader.Read())
            {
                Facility nextFacility = new Facility()
                {
                    Facility_ID = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
                facilities.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            reader.Close();
            Console.WriteLine();
            return facilities;
        }

        private int InsertFacility(SqlConnection con, Facility facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            string queryStringInsert = $"INSERT INTO Facility VALUES({facility.Facility_ID}, '{facility.Name}')";
            Console.WriteLine($"SQL applied: {queryStringInsert}");

            SqlCommand cmd = new SqlCommand(queryStringInsert, con);

            Console.WriteLine($"Creating facility #{facility.Facility_ID}");
            int numberOfRowsAffected = cmd.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection con, Facility facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            string queryStringUpdate = $"UPDATE Facility SET Name='{facility.Name}' WHERE Facility_ID = {facility.Facility_ID}";
            Console.WriteLine($"SQL applied: {queryStringUpdate}");

            SqlCommand cmd = new SqlCommand(queryStringUpdate, con);
            Console.WriteLine($"Update facility #{facility.Facility_ID}");
            int numberOfRowsAffected = cmd.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            return numberOfRowsAffected;
        }

        private int DeleteFacility(SqlConnection con, Facility facility)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            string queryStringDelete = $"DELETE FROM Facility WHERE Facility_ID = {facility.Facility_ID}";
            Console.WriteLine($"SQL applied: {queryStringDelete}");

            SqlCommand cmd = new SqlCommand(queryStringDelete, con);
            Console.WriteLine($"Delete facility #{facility.Facility_ID}");
            int numberOfRowsAffected = cmd.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            return numberOfRowsAffected;
        }

        private int GetMaxFacilityID(SqlConnection con)
        {
            Console.WriteLine("Calling -> GetMaxFacilityID");

            string queryStringMaxFacilityID = "SELECT MAX(Facility_ID) FROM Facility";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityID}");

            SqlCommand cmd = new SqlCommand(queryStringMaxFacilityID, con);
            SqlDataReader reader = cmd.ExecuteReader();

            int MaxFacility_ID = 0;
            if (reader.Read())
            {
                MaxFacility_ID = reader.GetInt32(0);
            }

            reader.Close();

            Console.WriteLine($"Max facility ID: {MaxFacility_ID}");
            Console.WriteLine();

            return MaxFacility_ID;
        }

        public void Start()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                //List all facilities from DB
                GetAllFacilities(con);

                //Create new facility
                Facility newFacility = new Facility()
                {
                    Facility_ID = GetMaxFacilityID(con) +1,
                    Name = "New facility"
                };

                //Insert new facility into DB
                InsertFacility(con, newFacility);

                //List all facilities from DB including "newFacility"
                GetAllFacilities(con);

                //Get the new facility to update it
                Facility facilityToBeUpdated = GetFacility(con, newFacility.Facility_ID);

                //Alter the facility name
                facilityToBeUpdated.Name += "(Updated)";

                //Update facility in the DB
                UpdateFacility(con, facilityToBeUpdated);

                //Show all facilities again including "facilityToBeUpdated"
                GetAllFacilities(con);

                //Get "facilityToBeUpdated" to delete it
                Facility facilityToBeDeleted = GetFacility(con, facilityToBeUpdated.Facility_ID);

                //Delete facility
                DeleteFacility(con, facilityToBeDeleted);

                //Showing the deletion worked
                GetAllFacilities(con);
            }
        }
    }
}
