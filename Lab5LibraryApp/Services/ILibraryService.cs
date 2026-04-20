using Lab5LibraryApp.Models;

namespace Lab5LibraryApp.Services
{
    public interface ILibraryService
    {
        List<Book> GetBooks();
        List<User> GetUsers();

        void AddBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int id);

        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int id);

        bool BorrowBook(int bookId, int userId);
        bool ReturnBook(int userId, int bookId);

        Dictionary<int, List<Book>> GetBorrowedBooks();
    }
}