using SQLite;
using People.Models;

namespace People;

public class PersonRepository
{
    private string _dbPath;
    private SQLiteConnection conn;

    public string StatusMessage { get; set; }

    private void Init()
    {
        if (conn != null)
            return;

        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Person>();
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddNewPerson(string name)
    {
        int result = 0;
        try
        {
            // se inicializa la base de datos por si acaso no lo esté
            Init();

            // excecpción para controlar que se puso el nombre correcto
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            // se inserta el objeto en la base de datos
            result = conn.Insert(new Person { Name = name });

            // y un mensaje de confirmación
            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
        }
        catch (Exception ex)
        {
           
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        }
    }


    public List<Person> GetAllPeople()
    {
        try
        {
            Init();
            // Retrieve a list of Person objects from the database
            return conn.Table<Person>().ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }
}
