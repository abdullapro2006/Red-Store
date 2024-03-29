﻿using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}
