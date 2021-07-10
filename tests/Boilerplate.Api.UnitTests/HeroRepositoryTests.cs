using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Infrastructure.Context;
using Boilerplate.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Boilerplate.Api.UnitTests
{
    public class HeroRepositoryTests
    {
        private ApplicationDbContext CreateDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
            return new ApplicationDbContext(options);
        }

        [Theory]
        [InlineData("4e1a20db-0533-4838-bd97-87d2afc89832")]
        [InlineData("ff57101b-d9c6-4b8a-959e-2d64c7ae8967")]
        [InlineData("2c0176d6-47d6-4ce1-b5e8-bed9a52b9e64")]
        [InlineData("bf15a502-37db-4d4c-ba4c-e231fb5487e6")]
        [InlineData("e141a755-98d4-44d3-a84f-528e6e75f543")]
        public async Task GetById_existing_heroes(Guid id)
        {
            // Arrange

            using (var context = CreateDbContext("GetById_existing_heroes"))
            {
                context.Set<Hero>().Add(new Hero { Id = id });
                await context.SaveChangesAsync();
            }
            Hero hero = null;

            // Act
            using (var context = CreateDbContext("GetById_existing_heroes"))
            {
                var repository = new HeroRepository(context);
                hero = await repository.GetById(id);
            }
            // Assert
            hero.Should().NotBeNull();
            hero.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("4e1a20db-0533-4838-bd97-87d2afc89832")]
        [InlineData("ff57101b-d9c6-4b8a-959e-2d64c7ae8967")]
        [InlineData("2c0176d6-47d6-4ce1-b5e8-bed9a52b9e64")]
        [InlineData("bf15a502-37db-4d4c-ba4c-e231fb5487e6")]
        [InlineData("e141a755-98d4-44d3-a84f-528e6e75f543")]
        public async Task GetById_inexistent_heroes(Guid id)
        {
            // Arrange
            using (var context = CreateDbContext("GetById_inexisting_heroes"))
            {
                context.Set<Hero>().Add(new Hero { Id = id });
                await context.SaveChangesAsync();
            }
            Hero hero = null;

            // Act
            using (var context = CreateDbContext("GetById_inexisting_heroes"))
            {
                var repository = new HeroRepository(context);
                hero = await repository.GetById(Guid.NewGuid());
            }
            // Assert
            hero.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetAll_heroes(int count)
        {
            // Arrange
            using (var context = CreateDbContext($"GetAll_with_heroes_{count}"))
            {
                for (var i = 0; i < count; i++) context.Set<Hero>().Add(new Hero());
                await context.SaveChangesAsync();
            }
            List<Hero> heroes = null;
            // Act
            using (var context = CreateDbContext($"GetAll_with_heroes_{count}"))
            {
                var repository = new HeroRepository(context);
                heroes = await repository.GetAll().ToListAsync();
            }
            // Assert
            heroes.Should().NotBeNull();
            heroes.Count.Should().Be(count);
        }

        [Fact]
        public async Task Create_Hero()
        {
            // Arrange
            int result;


            // Act
            var hero = new Hero
            {
                Age = 10,
                Name = "Izuku Midoriya",
                Nickname = "Deku",
                Individuality = "All for one",
                Team = "Team Midoriya",
                HeroType = HeroType.Student
            };

            using (var context = CreateDbContext("Create_Hero"))
            {
                var repository = new HeroRepository(context);
                repository.Create(hero);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Create_Hero"))
            {
                (await context.Heroes.CountAsync()).Should().Be(1);
                (await context.Heroes.FirstAsync()).Should().BeEquivalentTo(hero);
            }
        }

        [Fact]
        public async Task Update_Hero()
        {
            // Arrange
            int result;
            Guid? id;
            using (var context = CreateDbContext("Update_Hero"))
            {
                var createdHero = new Hero
                {
                    Age = 10,
                    Name = "Izuku Midoriya",
                    Nickname = "Deku",
                    Individuality = "All for one",
                    Team = "Team Midoriya",
                    HeroType = HeroType.Student
                };
                context.Set<Hero>().Add(createdHero);
                context.Set<Hero>().Add(new Hero { Name = "Another Hero", HeroType = HeroType.Vigilante, Age = 17 });
                await context.SaveChangesAsync();
                id = createdHero.Id; //receive autogenerated guid to get the entity later
            }

            // Act

            Hero updateHero;
            using (var context = CreateDbContext("Update_Hero"))
            {
                updateHero = await context.Set<Hero>().FirstOrDefaultAsync(x => x.Id == id);
                updateHero.Age = 15;
                updateHero.Individuality = "Blackwhip";
                updateHero.Team = null;
                var repository = new HeroRepository(context);
                repository.Update(updateHero);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Update_Hero"))
            {
                (await context.Heroes.FirstAsync(x => x.Id == updateHero.Id)).Should().BeEquivalentTo(updateHero);
            }
        }

        [Fact]
        public async Task Delete_Hero()
        {
            // Arrange
            int result;
            Guid? id;
            using (var context = CreateDbContext("Delete_Hero"))
            {
                var createdHero = new Hero
                {
                    Age = 10,
                    Name = "Izuku Midoriya",
                    Nickname = "Deku",
                    Individuality = "All for one",
                    Team = "Team Midoriya",
                    HeroType = HeroType.Student
                };
                context.Set<Hero>().Add(createdHero);
                context.Set<Hero>().Add(new Hero { Name = "Another Hero", HeroType = HeroType.Vigilante, Age = 17 });
                await context.SaveChangesAsync();
                id = createdHero.Id; //receive autogenerated guid to get the entity later
            }

            // Act
            using (var context = CreateDbContext("Delete_Hero"))
            {
                var repository = new HeroRepository(context);
                await repository.Delete(id.Value);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Delete_Hero"))
            {
                (await context.Set<Hero>().FirstOrDefaultAsync(x => x.Id == id)).Should().BeNull();
                (await context.Set<Hero>().ToListAsync()).Should().NotBeEmpty();
            }
        }

        [Fact]
        public void NullDbContext_Throws_ArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new HeroRepository(null);
            });
            exception.Should().NotBeNull();
            exception.ParamName.Should().Be("dbContext");
        }
    }
}
