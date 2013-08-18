﻿using System;
using Bs.Calendar.Core;
using Bs.Calendar.DataAccess.Bases;

namespace Bs.Calendar.DataAccess
{
    public class RepoUnit : IDisposable
    {
        private CalendarContext _context;
        private IUserRepository _user;
        private IRoomRepository _room;
        private ITeamRepository _team;
        private IBookRepository _book;
        private IBookHistoryRepository _bookHistory;
        private IPersonalEventRepository _personalEvent;
        private ITeamEventRepository _teamEvent;

        public IUserRepository User
        {
            get { return _user ?? (_user = getRepository<IUserRepository>()); }
        }

        public IPersonalEventRepository PersonalEvent
        {
            get { return _personalEvent ?? (_personalEvent = getRepository<IPersonalEventRepository>()); }
        }

        public ITeamEventRepository TeamEvent
        {
            get { return _teamEvent ?? (_teamEvent = getRepository<ITeamEventRepository>()); }
        }

        public IRoomRepository Room
        {
            get { return _room ?? (_room = getRepository<IRoomRepository>()); }
        }

        public IBookRepository Book
        {
            get { return _book ?? (_book = getRepository<IBookRepository>()); }
        }

		public ITeamRepository Team
        {
            get { return _team ?? (_team = getRepository<ITeamRepository>()); }
        }  
        
        public IBookHistoryRepository BookHistory
        {
            get { return _bookHistory ?? (_bookHistory = getRepository<IBookHistoryRepository>()); }
        }    
		
        private CalendarContext getContext()
		{
            return _context ?? (_context = new CalendarContext());
        }

        private T getRepository<T>() where T : IContexable
        {
            var repository = Ioc.Resolve<T>();
            repository.SetContext(getContext());

            return repository;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
