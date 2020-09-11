using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DNI.Core.Services.Extensions;

namespace DNI.Core.Tests
{
    public class Animal
    {
        public string Name { get; set; }
        public bool LivesOnLand { get; set; }
        public bool HasHairOrFur { get; set; }
        public bool CanSwim { get; set; }
    }
    [TestFixture]
    public class ExpressionExtensionTests
    {

        [Test]
        public void Combine()
        {
            var bear = new Animal { Name = "Bear", CanSwim = true, LivesOnLand = true, HasHairOrFur = true };
            var fish = new Animal { Name = "Fish", CanSwim = true, LivesOnLand = false, HasHairOrFur = false };
            var tiger = new Animal { Name = "Tiger", CanSwim = false, LivesOnLand = true, HasHairOrFur = true };
            var lion =  new Animal { Name = "Lion", CanSwim = true, LivesOnLand = true, HasHairOrFur = true };

            var animalList = new List<Animal>
            {
                bear, fish, tiger, lion
            };

            Expression<Func<Animal, bool>> canSwim = animal => animal.CanSwim;
            Expression<Func<Animal, bool>> livesOnLand = animal => animal.LivesOnLand;

            var filteredAnimals = animalList.Where(canSwim.Combine(livesOnLand)).ToArray();

            Assert.Contains(bear, filteredAnimals);
            Assert.Contains(lion, filteredAnimals);
        }
    }
}
