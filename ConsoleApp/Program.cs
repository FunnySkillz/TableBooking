using Core.Entities;
using Core.Validations;
using Persistence;
using QRCoder;
using System.ComponentModel.DataAnnotations;

int selection;
while ((selection = Menu()) != 0)
{
    switch (selection)
    {
        case 1:
            await AddNewPerson();
            break;
        case 2:
            await AddNewTable();
            break;
        case 3:
            await PrintAllPersons();
            break;
        case 4:
            await PrintAllTablesByPerson();
            break;
        case 5:
            await PrintAllTables();
            break;
        case 6:
            await PrintPersonTable();
            break;
    }

    Console.WriteLine("\n<Taste drücken>");
    Console.ReadKey();
}

static int Menu()
{
    int selection;
    Console.Clear();
    Console.WriteLine("1.....Person anlegen");
    Console.WriteLine("2.....Tisch anlegen");
    Console.WriteLine("3.....Alle Personen ausgeben");
    Console.WriteLine("4.....Alle Tische zu einer Person ausgeben");
    Console.WriteLine("5.....Alle Tische ausgeben (nach Name)");
    Console.WriteLine("6.....Person + Tische");
    Console.WriteLine("0.....ENDE");
    do
    {
        if (!Int32.TryParse(Console.ReadLine(), out selection))
            selection = -1;
    } while (selection < 0 || selection > 6);
    return selection;
}


/// <summary>
/// Person anlegen. Vor und Nachname werden eingegeben. Mit Validierung, sauber gemacht.
/// </summary>
static async Task AddNewPerson()
{
    string firstName;
    string lastName;
    string email;

    Console.Write("Vornamen eingeben: ");
    firstName = Console.ReadLine()!;
    Console.Write("Nachname eingeben: ");
    lastName = Console.ReadLine()!;
    Console.Write("Email eingeben: ");
    email = Console.ReadLine()!;
    
    Person newPerson = new Person() { FirstName = firstName, LastName = lastName, Email = email };


    List<ValidationResult> valResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(newPerson, new ValidationContext(newPerson), valResults, true))
    {
        Console.WriteLine("\nPerson ungültig:");
        foreach (var result in valResults)
        {
            Console.WriteLine(result.ErrorMessage);
        }
        return;
    }

    DaTable newT = new DaTable() { TableNumber = 12, QRCode = firstName+12+lastName+"QRx" };
    Booking newBooking = new Booking() { Person = newPerson, Table = newT };
    
    using (UnitOfWork uow = new UnitOfWork())
    {
        await uow.PersonRepository.InsertAsync(newPerson);
        await uow.BookingRepository.InsertAsync(newBooking);

        try
        {
            await uow.SaveChangesAsync();
        }
        catch (ValidationException e)
        {

            Console.WriteLine("Speichern nicht möglich!");
            Console.WriteLine(e.ValidationResult.ErrorMessage);
        }
    }
}

/// <summary>
/// Eine neue Person wird angelegt
/// </summary>
static async Task AddNewTable()
{
    int tableNumber;
    string qrCode;
    DaTable newTable;

    Console.Write("Nummer: ");
    tableNumber = Convert.ToInt32(Console.ReadLine()!);
    Console.Write("QRCode: ");
    qrCode = Convert.ToString(Console.ReadLine()!);

    using (UnitOfWork uow = new UnitOfWork())
    {
        newTable = new DaTable()
        {
            TableNumber = tableNumber,
            QRCode = qrCode
        };

        List<ValidationResult> valResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(newTable, new ValidationContext(newTable), valResults, true))
        {
            Console.WriteLine("\nTisch ungültig:");
            foreach (var result in valResults)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            return;
        }

        await uow.TableRepository.AddAsync(newTable);
        try
        {
            await uow.SaveChangesAsync();
        }
        catch (ValidationException e)
        {
            Console.WriteLine("Speichern nicht möglich!");
            Console.WriteLine(e.ValidationResult.ErrorMessage);
        }
    }
}


/// <summary>
/// Alle Personen ausgeben sortiert nach Nachname
/// </summary>
static async Task PrintAllPersons()
{
    using (UnitOfWork uow = new UnitOfWork())
    {
        Console.WriteLine("\nPersonen:");
        Console.WriteLine("===========");
        Console.WriteLine("\n{0,-20} {1,-20}","Nachname", "Vorname");
        var persons = await uow.PersonRepository.GetAllOrderedByLastNameAsync();
        foreach (var person in persons)
        {
            Console.WriteLine("{0,-20} {1,-20}", person.FirstName, person.LastName);
        }
    }
}

/// <summary>
/// Alle Tische zu einer bestimmten Person sortiert nach Datum und Menge ausgeben.
/// Gewünschter Vorname und Nachname werden eingegeben.
/// </summary>
static async Task PrintAllTablesByPerson()
{
    string firstName;
    string lastName;
    Console.Write("Vornamen eingeben: ");
    firstName = Console.ReadLine()!;
    Console.Write("Nachname eingeben: ");
    lastName = Console.ReadLine()!;

    using (UnitOfWork uow = new UnitOfWork())
    {
        Console.WriteLine("\nTische von :" + firstName + " " + lastName);
        Console.WriteLine("{0,-10} {1,10} ", "#", "QRCode");
        var tables = await uow.TableRepository.GetTablesByPerson(firstName, lastName);

        foreach (var tab in tables)
        {
            Console.WriteLine("{0,-10} {1,10} ", tab.TableNumber, tab.QRCode);
        }
    }
}

/// <summary>
/// Alle Tische absteigend sortiert 
/// </summary>
static async Task PrintAllTables()
{
    using (UnitOfWork uow = new UnitOfWork())
    {
        Console.WriteLine("\nAlle Tische:");
        Console.WriteLine("{0,-10} {1,10} ", "TableNumber", "QRCode");
        var tables = await uow.TableRepository.GetAllAsync();
        
        foreach (var tab in tables)
        {
            Console.WriteLine("{0,-10} {1,10} ", tab.TableNumber, tab.QRCode);
        }
    }
}

/// <summary>
/// Alle Personen und Tische ausgeben
/// </summary>
static async Task PrintPersonTable()
{

    using (UnitOfWork uow = new UnitOfWork())
    {
        Console.WriteLine("\nSummary");
        Console.WriteLine("{0,-10} {1, 20} {2,20} {3,-20}", "LastName", "FirstName", "Table#", "QRCode");
        var ptsDTO = await uow.BookingRepository.PersonTableSummaryAsync();

        foreach (var pt in ptsDTO)
        {
            Console.WriteLine("{0,-10} {1, 20} {2,20} {3,-20}", pt.LastName,pt.FirstName,pt.TableNumber,pt.QRCode);
        }
    }
}
