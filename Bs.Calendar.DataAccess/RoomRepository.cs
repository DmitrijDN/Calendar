﻿using System.Linq;
using Bs.Calendar.Core;
using Bs.Calendar.DataAccess.Bases;
using Bs.Calendar.Models;

namespace Bs.Calendar.DataAccess
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public IQueryable<Room> Load(RoomFilter filter)
        {
            var query = Load()
                .WhereIf(filter.SearchString.IsNotEmpty(), room => room.Name.Contains(filter.SearchString));

            return query;
        }
    }
}
