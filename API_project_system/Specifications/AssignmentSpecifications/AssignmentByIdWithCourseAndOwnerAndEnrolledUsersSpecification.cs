﻿using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.AssigmentSpecifications
{
    public class AssignmentByIdWithCourseAndOwnerAndEnrolledUsersSpecification : Specification<Assignment>
    {
        private readonly int assignmentId;

        public AssignmentByIdWithCourseAndOwnerAndEnrolledUsersSpecification(int assignmentId)
        {
            this.assignmentId = assignmentId;
        }

        public override Expression<Func<Assignment, bool>> ToExpression()
        {
            return f => f.Id == assignmentId;
        }

        public override IQueryable<Assignment> IncludeEntities(IQueryable<Assignment> queryable)
        {
            return queryable
                .Include(f => f.Course)
                    .ThenInclude(f => f.Owner)
                .Include(f => f.Course)
                    .ThenInclude(f => f.EnrolledUsers);
        }
    }
}
