﻿namespace API_project_system.ModelsDto.CourseDto
{
    public class AddCourseDto
    {
        public string Name { get; set; }
        public string IconPath { get; set; }
        public int YearStart { get; set; } = DateTime.UtcNow.Year;
    }
}
