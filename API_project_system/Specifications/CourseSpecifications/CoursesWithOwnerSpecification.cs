﻿using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.RepositorySpecification
{
    public class CoursesWithOwnerSpecification : Specification<Course>
    {
        private readonly string? searchPhrase;

        public CoursesWithOwnerSpecification(string? searchPhrase)
        {
            this.searchPhrase = searchPhrase;
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return f => (searchPhrase == null || f.Name.Contains(searchPhrase, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IQueryable<Course> IncludeEntities(IQueryable<Course> queryable)
        {
            return queryable.Include(f => f.Owner);
        }
    }
}
