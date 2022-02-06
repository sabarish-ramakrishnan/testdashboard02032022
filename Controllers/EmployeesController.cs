﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace testdashboard02032022.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            var employees = GetEmployees();
            return employees;
        }
        private IEnumerable<Employee> GetEmployees()
        {
            var employees = new List<Employee>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("SqlServerConnection")))
            {
                var sql = "SELECT Id, FirstName, LastName, Email, PhoneNumber FROM Employee";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var employee = new Employee()
                    {
                        Id = (long)reader["Id"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                    };
                    employees.Add(employee);
                }
            }
            return employees;
        }
    }
}
