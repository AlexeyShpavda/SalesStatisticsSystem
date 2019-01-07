﻿using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects;
using SalesStatisticsSystem.Contracts.DataAccessLayer.Repositories;
using SalesStatisticsSystem.DataAccessLayer.Repositories.Abstract;
using SalesStatisticsSystem.Entity;

namespace SalesStatisticsSystem.DataAccessLayer.Repositories
{
    public class ManagerRepository : GenericRepository<ManagerDto, Manager>, IManagerRepository
    {
        public ManagerRepository(SalesInformationEntities context, IMapper mapper) : base(context, mapper)
        {
        }

        public void AddUniqueManagerToDatabase(ManagerDto managerDto)
        {
            if (DoesManagerExist(managerDto)) throw new ArgumentException("Manager already exists!");

            Add(managerDto);
        }

        public int? GetId(string managerLastName)
        {
            Expression<Func<ManagerDto, bool>> predicate = x => x.LastName == managerLastName;

            return Find(predicate).First().Id;
        }

        public bool DoesManagerExist(ManagerDto managerDto)
        {
            Expression<Func<ManagerDto, bool>> predicate = x => x.LastName == managerDto.LastName;

            return Find(predicate).Any();
        }
    }
}