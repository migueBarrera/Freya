﻿namespace Models.Core.Employees;

public class EmployeeSignInRequest
{
    public string Email { get; set; } = string.Empty;

    public string Pass { get; set; } = string.Empty;
}
