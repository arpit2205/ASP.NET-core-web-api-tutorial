# ASP.NET-core-web-api-tutorial

## Overview 💭

### 1. Creating an ASP dotnet core web API app 
- Open Visual Studio
- Create new project
- Select ASP.NET Core Web API [Template]
- Enter project name
- Leave rest to default and proceed to create

### 2. Folder Structure
- Connected services, Dependencies [IGNORE]
- DELETE WeatherForecast.cs and WeatherForecastController.cs [DUMMY FILES]
- **launchSettings.json, appsettings.json** [Settings for how the app launches and runs respectively]
- **Program.cs** [The main and most important file where we mention the functionalities required]

## Hands-on 🖐️

### 1. Models
1. Create a new folder "Models"
2. For every model, we create a new .cs file [example: Pokemon.cs --> A C# class template]
3. This file will be a schema (Pokemon schema) and we can add properties to the schema
4. Make multiple .cs files for all the required schemas

_Example: Pokemon.cs_
```
namespace PokemonReviewApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }
    }
}

```

➡️ **Relationships**

_Refer to this image for relations_      
![UMLDiagram](https://github.com/arpit2205/ASP.NET-core-web-api-tutorial/assets/51786177/7daa5312-7b6b-4a81-9564-811b5c3c9e93)


1. **One to Many [One side]**
```
namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gym { get; set; }

        // ONE-to-many: An owner will have ONE country
        public Country Country { get; set; }

    }
}

```
2. **One to Many [Many side]**
```
namespace PokemonReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // one-to-MANY: A country can have MANY owners
        public ICollection<Owner> Owners { get; set; }
    }
}

```
3. **Many to Many**  
For many-to-many, we actually need to create models (schemas) of the joined tables that we want. In this example, we want to create 2 joined tables, one is Pokemon-Categories and other is Pokemon-Owners. Both of these are many-to-many relationships.  

```
// Creating the joined table schema
1. Pokemon-Owner

namespace PokemonReviewApp.Models
{
    public class PokemonOwner
    {
        public int PokemonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }
    }
}

2. Pokemon-Category
namespace PokemonReviewApp.Models
{
    public class PokemonCategory
    {
        public int PokemonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }
    }
}

```
Now, we need to add the ICollection<JoinedTableScema> to the individual schemas (pokemon, category, owner) just like we did in one-to-many relations  

```
1. Pokemon.cs
namespace PokemonReviewApp.Models
{
    public class Pokemon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        // one-to-MANY: A pokemon can have MANY reviews
        public ICollection<Review> Reviews { get; set; }

        // many-to-many
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}

2. Owner.cs
namespace PokemonReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gym { get; set; }

        // ONE-to-many: An owner will have ONE country
        public Country Country { get; set; }

        // many-to-many
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }
}

3. Category.cs
namespace PokemonReviewApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // many-to-many
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}

```

### 2. Database Connection
1. Install Microsoft SQL Server Management Studio
2. Right-click on databases -> Create new database
3. Open SQL Server Object Explorer in VS
4. Click on add SQL Server
5. Go back to SSMS, right-click on "DESKTOP..." -> Select properties -> Copy name
6. Enter this name as server name in VS
7. Select the created DB from the dropdown
8. From the server explorer, click on the new DB -> right click -> properties
9. Copy connection string from here
10. Enter the connection string in appsettings.json like this        
```
    "ConnectionStrings": {
    "DefaultConnection": "Data Source=DESKTOP-D4KDER9\\SQLEXPRESS;Initial Catalog=pokemonreview;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  },
```

### 3. Install Entity Framework
1. Go to manage NuGet packages
2. Install Microsoft.EntityFrameworkCore.SqlServer and Microsoft.EntityFrameworkCore.Design

### 4. Creating Data Context
The data context allows us to get and manipulate data from the database.
- Create a new folder data
- Create a new file inside it -> DataContext.cs

1. The class inherits from DbContext (EF core)
```
using Microsoft.EntityFrameworkCore;
namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
    }
}
```
2. Create datacontext constructor
```
using Microsoft.EntityFrameworkCore;
namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
```
3. Add tables from database
```
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

    }
}

```
4. Let EF know the relations we want to create
```
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Pokemon-Category relation
            // Letting EF know the relation
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new {pc.PokemonId, pc.CategoryId});

            // Pokemon
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);

            // Category
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(c => c.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.CategoryId);
            // --------------------------------------------------

            // 2. Pokemon-Owner relation
            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });

            // Pokemon
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);

            // Owner
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(o => o.Owner)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(o => o.OwnerId);
            // --------------------------------------------------
        }
    }
}

```
