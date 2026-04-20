using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab5LibraryApp.Services;
using Lab5LibraryApp.Models;
using System.Linq;
using System.IO;

[assembly: DoNotParallelize]

namespace Lab5LibraryApp.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private LibraryService service = null!;

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory("Data");
            File.WriteAllText("Data/Books.csv", "");
            File.WriteAllText("Data/Users.csv", "");

            service = new LibraryService();
        }

        [TestMethod]
        public void AddBook_ShouldIncreaseBookCount()
        {
            int before = service.GetBooks().Count;

            service.AddBook(new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                ISBN = "123"
            });

            int after = service.GetBooks().Count;
            Assert.AreEqual(before + 1, after);
        }

        [TestMethod]
        public void EditBook_ShouldUpdateBookTitle()
        {
            service.AddBook(new Book
            {
                Title = "Old Title",
                Author = "Author",
                ISBN = "111"
            });

            var addedBook = service.GetBooks().First();
            addedBook.Title = "New Title";

            service.EditBook(addedBook);

            var updatedBook = service.GetBooks().FirstOrDefault(b => b.Id == addedBook.Id);
            Assert.IsNotNull(updatedBook);
            Assert.AreEqual("New Title", updatedBook.Title);
        }

        [TestMethod]
        public void DeleteBook_ShouldRemoveBook()
        {
            service.AddBook(new Book
            {
                Title = "Delete Me",
                Author = "Author",
                ISBN = "222"
            });

            int id = service.GetBooks().First().Id;

            service.DeleteBook(id);

            var deletedBook = service.GetBooks().FirstOrDefault(b => b.Id == id);
            Assert.IsNull(deletedBook);
        }

        [TestMethod]
        public void AddUser_ShouldIncreaseUserCount()
        {
            int before = service.GetUsers().Count;

            service.AddUser(new User
            {
                Name = "Joshua",
                Email = "joshua@test.com"
            });

            int after = service.GetUsers().Count;
            Assert.AreEqual(before + 1, after);
        }

        [TestMethod]
        public void DeleteUser_ShouldRemoveUser()
        {
            service.AddUser(new User
            {
                Name = "Delete User",
                Email = "delete@test.com"
            });

            int id = service.GetUsers().First().Id;

            service.DeleteUser(id);

            var deletedUser = service.GetUsers().FirstOrDefault(u => u.Id == id);
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public void BorrowBook_ShouldReturnTrue_WhenBookAndUserExist()
        {
            service.AddBook(new Book
            {
                Title = "Borrow Test",
                Author = "Author",
                ISBN = "333"
            });

            service.AddUser(new User
            {
                Name = "Borrow User",
                Email = "borrow@test.com"
            });

            int bookId = service.GetBooks().First().Id;
            int userId = service.GetUsers().First().Id;

            bool result = service.BorrowBook(bookId, userId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BorrowBook_ShouldReturnFalse_WhenBookDoesNotExist()
        {
            service.AddUser(new User
            {
                Name = "User Only",
                Email = "useronly@test.com"
            });

            int userId = service.GetUsers().First().Id;

            bool result = service.BorrowBook(9999, userId);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ReturnBook_ShouldReturnTrue_WhenBookWasBorrowed()
        {
            service.AddBook(new Book
            {
                Title = "Return Test",
                Author = "Author",
                ISBN = "444"
            });

            service.AddUser(new User
            {
                Name = "Return User",
                Email = "return@test.com"
            });

            int bookId = service.GetBooks().First().Id;
            int userId = service.GetUsers().First().Id;

            service.BorrowBook(bookId, userId);

            bool result = service.ReturnBook(userId, bookId);

            Assert.IsTrue(result);
        }
    }
}