using Lab5LibraryApp.Models;
using System.Linq;

namespace Lab5LibraryApp.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly string booksFilePath = Path.Combine("Data", "Books.csv");
        private readonly string usersFilePath = Path.Combine("Data", "Users.csv");

        private List<Book> books = new();
        private List<User> users = new();
        private Dictionary<int, List<Book>> borrowedBooks = new();

        public LibraryService()
        {
            Directory.CreateDirectory("Data");

            if (!File.Exists(booksFilePath))
            {
                File.WriteAllText(booksFilePath, "");
            }

            if (!File.Exists(usersFilePath))
            {
                File.WriteAllText(usersFilePath, "");
            }

            ReadBooks();
            ReadUsers();
        }

        public List<Book> GetBooks()
        {
            return books;
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public Dictionary<int, List<Book>> GetBorrowedBooks()
        {
            return borrowedBooks;
        }

        private void ReadBooks()
        {
            books.Clear();

            if (!File.Exists(booksFilePath))
                return;
            foreach (var line in File.ReadAllLines(booksFilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var fields = line.Split(',');

                if (fields.Length >= 4)
                {
                    books.Add(new Book
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Title = fields[1].Trim(),
                        Author = fields[2].Trim(),
                        ISBN = fields[3].Trim()
                    });
                }
            }
        }

        private void ReadUsers()
        {
            users.Clear();

            if (!File.Exists(usersFilePath))
                return;

            foreach (var line in File.ReadAllLines(usersFilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var fields = line.Split(',');

                if (fields.Length >= 3)
                {
                    users.Add(new User
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Name = fields[1].Trim(),
                        Email = fields[2].Trim()
                    });
                }
            }
        }

        private void SaveBooks()
        {
            var lines = books.Select(b => $"{b.Id},{b.Title},{b.Author},{b.ISBN}");
            File.WriteAllLines(booksFilePath, lines);
        }

        private void SaveUsers()
        {
            var lines = users.Select(u => $"{u.Id},{u.Name},{u.Email}");
            File.WriteAllLines(usersFilePath, lines);
        }

        public void AddBook(Book book)
        {
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            SaveBooks();
        }

        public void EditBook(Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.ISBN = updatedBook.ISBN;
                SaveBooks();
            }
        }

        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                SaveBooks();
            }
        }

        public void AddUser(User user)
        {
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            SaveUsers();
        }

        public void EditUser(User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                SaveUsers();
            }
        }

        public void DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SaveUsers();
            }
        }

        public bool BorrowBook(int bookId, int userId)
        {
            var book = books.FirstOrDefault(b => b.Id == bookId);
            var user = users.FirstOrDefault(u => u.Id == userId);

            if (book == null || user == null)
                return false;

            if (!borrowedBooks.ContainsKey(userId))
            {
                borrowedBooks[userId] = new List<Book>();
            }

            borrowedBooks[userId].Add(book);
            books.Remove(book);
            SaveBooks();
            return true;
        }

        public bool ReturnBook(int userId, int bookId)
        {
            if (!borrowedBooks.ContainsKey(userId))
                return false;

            var book = borrowedBooks[userId].FirstOrDefault(b => b.Id == bookId);
            if (book == null)
                return false;

            borrowedBooks[userId].Remove(book);
            books.Add(book);
            SaveBooks();
            return true;
        }
    }
}