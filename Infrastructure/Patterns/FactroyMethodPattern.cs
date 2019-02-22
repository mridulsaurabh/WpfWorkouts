using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Patterns
{
    /// <summary>
    /// Factroy Method Pattern
    /// Frequently used, fairly easy to implement and useful for centralizing object lifetime management and avoiding object creation code duplication
    /// </summary>
    public abstract class BookReader
    {
        public BookReader()
        {
            this.Book = this.BuyBook();
        }

        public Book Book { get; set; }

        public abstract Book BuyBook();

        public Book BuyBook<T>()
            where T : Book, new()
        {
            return new T();
        }

        public void DisplayOwnedBooks()
        {
            Console.WriteLine(Book.GetType().ToString());
        }
    }


    public class HorrorBookReader : BookReader
    {
        public override Book BuyBook()
        {
            return new Dracula();
        }
    }

    public class FantasyBookReader : BookReader
    {
        public override Book BuyBook()
        {
            return new FiftyShadesOfGrey();
        }
    }

    public class AdventureBookReader : BookReader
    {
        public override Book BuyBook()
        {
            return new TreasureIsland();
        }
    }


    public abstract class Book
    {
    }

    public class Dracula : Book
    {

    }

    public class FiftyShadesOfGrey : Book
    {

    }

    public class TreasureIsland : Book
    {

    }

    public class ConsumerApplication
    {
        private static void FactoryMethod()
        {
            var bookReaderList = new List<BookReader>();

            bookReaderList.Add(new AdventureBookReader());
            bookReaderList.Add(new FantasyBookReader());
            bookReaderList.Add(new HorrorBookReader());

            foreach (var reader in bookReaderList)
            {
                Console.WriteLine(reader.GetType().ToString());
                reader.DisplayOwnedBooks();
                Console.WriteLine();
            }


            var genericReader = new AdventureBookReader();
            Book book = genericReader.BuyBook<FiftyShadesOfGrey>(); 

            Console.WriteLine(book.GetType().ToString());
            Console.ReadKey();
        }
    }

}
