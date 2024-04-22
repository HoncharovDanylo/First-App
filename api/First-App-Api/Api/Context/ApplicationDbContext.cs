using Microsoft.EntityFrameworkCore;

namespace Api.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    
}