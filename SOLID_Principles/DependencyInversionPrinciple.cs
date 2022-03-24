using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace SOLID_Principles
{
    public class DependencyInversionPrinciple
    {
        public static void Execute()
        {
            WriteLine("\n *** Dependency Inversion Principle *** \n");
            
            Research.Initialize();
            
            NewResearch.Initialize();
        }
    }

    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name { get; set; }
        // public DateTime DateOfBirth { get; set; }
    }

    public class Relationships
    {
        private List<(Person, Relationship, Person)> _relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            _relations.Add((parent, Relationship.Parent, child));
            _relations.Add((child, Relationship.Child, parent));
        }

        // This property should be exposed.
        // It makes the entire Relationships DB public
        public List<(Person, Relationship, Person)> Relations => _relations;
    }

    public class Research
    {
        public static void Initialize()
        {
            WriteLine("Old Research:");
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            // This is wrong!
            // Here we are forcing the Relationships DB
            // to expose the entire Database, so that we
            // can filter out the desired data.
            var personName = "John";
            var relations = relationships.Relations;
            foreach (var relation in relations
                         .Where(r => r.Item1.Name == personName 
                                     && r.Item2 == Relationship.Parent))
            {
                WriteLine($"  - {personName} has a child named {relation.Item3.Name}");
            }
        }
    }

    // Rectifying the above structure
    
    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }
    
    public class NewRelationships : IRelationshipBrowser
    {
        private List<(Person, Relationship, Person)> _relations = new List<(Person, Relationship, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            _relations.Add((parent, Relationship.Parent, child));
            _relations.Add((child, Relationship.Child, parent));
        }

        // The inner structure of the relationships DB
        // should not be exposed
        // public List<(Person, Relationship, Person)> Relations => _relations;
        
        // Rather use an interface to return the desired values from DB
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            foreach (var relation in _relations
                         .Where(r => r.Item1.Name == "John" 
                                     && r.Item2 == Relationship.Parent))
            {
                yield return relation.Item3;
            }
        }
    }
    
    public class NewResearch
    {
        public static void Initialize()
        {
            WriteLine("New Research:");
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new NewRelationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            // Instead of loading the entire DB,
            // we would use very specific methods to get
            // our desired result
            var personName = "John";
            foreach (var relation in relationships.FindAllChildrenOf(personName))
            {
                WriteLine($"  - {personName} has a child named {relation.Name}");
            }
        }
    }
}