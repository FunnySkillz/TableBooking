using Persistence;

Console.WriteLine("Kunden und Tische werden eingelesen");
using (UnitOfWork uow = new())
{
    await uow.FillDbAsync();

    int cntPersons  = await uow.PersonRepository.CountAsync();
    int cntRuns = await uow.TableRepository.CountAsync();

    Console.WriteLine(cntPersons + " Kunden eingelesen!");
    Console.WriteLine(cntRuns + " Tische eingelesen!");
}
Console.Write("Beenden mit Eingabetaste ...");
Console.ReadLine();
