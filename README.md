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
