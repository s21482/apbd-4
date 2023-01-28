using AnimalsApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace AnimalsApi.Services
{
    public interface IDatabaseService
    {
        IEnumerable<Animal> GetAnimals(string orderBy);
        Animal AddAnimal(Animal animal);
        Animal ModifyAnimal(int indexNumber, Animal animal);
        bool DeleteAnimal(int indexNumber);

    }

    public class DatabaseService : IDatabaseService
    {
        string conString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s21482;Integrated Security=True";

        public IEnumerable<Animal> GetAnimals(string? orderBy)
        {
            var animals = new List<Animal>();

            string[] allowedColumns = { "name", "description", "category", "area" };

            if (!allowedColumns.Any(c => c.Equals(orderBy, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Invalid column name for ORDER BY clause");
            }

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "SELECT * FROM Animal ORDER BY " + orderBy + " ASC";

                Console.WriteLine(com.CommandText);

                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {

                    animals.Add(new Animal
                    {
                        IdAnimal = (int)dr["IdAnimal"],
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Category = dr["Category"].ToString(),
                        Area = dr["Area"].ToString(),
                    });
                }
            }

            return animals;
        }


        public Animal AddAnimal(Animal animal)
        {
            using (var con = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "insert into Animal values (@name, @description, @category, @area)";
                com.Parameters.AddWithValue("name", animal.Name);
                com.Parameters.AddWithValue("description", animal.Description);
                com.Parameters.AddWithValue("category", animal.Category);
                com.Parameters.AddWithValue("area", animal.Area);
                con.Open();
                com.ExecuteNonQuery();
            }
            return animal;
        }

        public Animal? ModifyAnimal(int indexNumber, Animal animal)
        {
            using (var con = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "update Animal set Name=@name, Description=@description, Category=@category, Area=@area where IdAnimal=@id";
                com.Parameters.AddWithValue("id", indexNumber);
                com.Parameters.AddWithValue("name", animal.Name);
                com.Parameters.AddWithValue("description", animal.Description);
                com.Parameters.AddWithValue("category", animal.Category);
                com.Parameters.AddWithValue("area", animal.Area);
                con.Open();
                int rows = com.ExecuteNonQuery();
                if (rows == 0)
                {
                    return null;
                }
            }
            return animal;
        }

        public bool DeleteAnimal(int indexNumber)
        {
            using (var con = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "delete from Animal where IdAnimal=@indexNumber";
                com.Parameters.AddWithValue("indexNumber", indexNumber);
                con.Open();
                int rows = com.ExecuteNonQuery();
                if (rows == 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
