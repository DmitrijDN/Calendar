﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bs.Calendar.DataAccess;
using Bs.Calendar.Models;
using Bs.Calendar.Mvc.ViewModels;
using Bs.Calendar.Rules;

namespace Bs.Calendar.Mvc.Services
{
    public class BookService
    {
        private readonly RepoUnit _repoUnit;

        public BookService(RepoUnit repository)
        {
            _repoUnit = repository;
        }

        public Book Get(int id)
        {
            var book = _repoUnit.Book.Get(id);
            return book;
        }

        public List<Book> Load(string orderby, string searchStr)
        {
            IEnumerable<Book> books = _repoUnit.Book.Load();
            books = _search(books, searchStr);
            books = _orderBy (books, orderby);
            return books.ToList();
        }

        private static IEnumerable<Book> _orderBy(IEnumerable<Book> books, string @orderby)
        {
            bool asc = true;
            if (!String.IsNullOrEmpty(orderby))
            {
                orderby = orderby.ToLower();
                if (orderby[0] == '-')
                {
                    asc = false;
                    orderby = orderby.Substring(1);
                }
            }
            if (orderby == "title")
            {
                return asc ? books.OrderBy(book => book.Title) : books.OrderByDescending(book => book.Title);
            }
            if (orderby == "author")
            {
                return asc ? books.OrderBy(book => book.Author) : books.OrderByDescending(book => book.Author);
            }
            //            if (orderby == "code")
            //            {
            //                return asc ? books.OrderBy(book => book.Code) : books.OrderByDescending(book => book.Code);
            //            }
            return asc ? books.OrderBy(book => book.Id) : books.OrderByDescending(book => book.Id);
        }

        private static IEnumerable<Book> _search(IEnumerable<Book> books, string @searchStr)
        {
            return books.Where(book =>
                               book.Author.ToLower().Contains(searchStr) || book.Title.ToLower().Contains(searchStr));
        }


        public bool IsValid(BookEditVm book)
        {
            bool result = true;
            result = result && (book.Title != string.Empty);
            result = result && (book.Author != string.Empty);
            return result;
        }

        public void Delete(int id)
        {
            _repoUnit.Book.Delete(_repoUnit.Book.Get(id));
        }

        public void Save(BookEditVm bookModel)
        {
            var book = Get(bookModel.BookId) ?? new Book();
            book.Title = bookModel.Title;
            book.Author = bookModel.Author;
            _repoUnit.Book.Save(book);
        }
    }
}