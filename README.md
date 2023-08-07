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
### 5. Setup program.cs file
Add this to the program.cs file
```
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```
### 6. Seeding
1. This step involves adding some initial data to the database so that everything works fine. Create a new seed.cs file in the root folder and copy paste the following code
```
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.PokemonOwners.Any())
            {
                var pokemonOwners = new List<PokemonOwner>()
                {
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Pikachu",
                            BirthDate = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Pikachu",Text = "Pickahu is the best pokemon, because it is electric",
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title="Pikachu", Text = "Pickachu is the best a killing rocks",
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Pikachu",Text = "Pickchu, pickachu, pikachu",
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            Name = "Jack London",

                            Gym = "Brocks Gym",
                            Country = new Country()
                            {
                                Name = "Kanto"
                            }
                        }
                    },
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Squirtle",
                            BirthDate = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Water"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title= "Squirtle", Text = "squirtle is the best pokemon, because it is electric",
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title= "Squirtle",Text = "Squirtle is the best a killing rocks",
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title= "Squirtle", Text = "squirtle, squirtle, squirtle",
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            Name = "Harry Potter",
                            Gym = "Mistys Gym",
                            Country = new Country()
                            {
                                Name = "Saffron City"
                            }
                        }
                    },
                                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Venasuar",
                            BirthDate = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Leaf"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Veasaur",Text = "Venasuar is the best pokemon, because it is electric",
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title="Veasaur",Text = "Venasuar is the best a killing rocks",
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Veasaur",Text = "Venasuar, Venasuar, Venasuar",
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            Name = "Ash Ketchum",
                            Gym = "Ashs Gym",
                            Country = new Country()
                            {
                                Name = "Millet Town"
                            }
                        }
                    }
                };
                dataContext.PokemonOwners.AddRange(pokemonOwners);
                dataContext.SaveChanges();
            }
        }
    }
}
```
2. Add this code to the program.cs file below builder.Services.AddControllers();
```
builder.Services.AddTransient<Seed>();
```
3. Add this code to the program.cs file below var app = builder.Build();
```
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}
```
### 7. Migrations 
Migrations generate the code (SQL) from the EF (ORM) that would be executed in order to manipulate the database.      

1. Run the following commands in the package manager console. This will create the migrations files.
```
Add-Migration InitialCreate
```
2. Then, run this. This will create the tables in the database.
```
Update-Database
```
3. Navigate inside the PokemonReviewApp folder, open a terminal and then run this. This will seed the database.
```
dotnet run seeddata
```

## API
To create API endpoints, we need to follow the repository pattern and thus we create interfaces, repositories and controllers.

### 1. Interfaces
Interfaces define contracts that classes must adhere to. They declare a set of methods and properties that implementing classes must provide.     

- Create a new folder Interfaces
- Add a new interface item **IPokemonRepository.cs**

Our repository will inherit this interface. 
```
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
    }
}

```
### 2. Repository
Repositories are a pattern used to abstract and encapsulate data access logic. They provide an interface for interacting with data storage, such as databases or external APIs. They contain the API calls which are then utilized inside the controllers.

- Create a new folder Repository
- Add a new class item **PokemonRepository.cs**
```
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    // inheriting the interface
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        // bringing in the data context 
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            // DB API call
            return _context.Pokemon.OrderBy(p => p.Id).ToList();
        }
    }
}

```
