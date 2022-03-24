using System.Collections.Generic;
using static System.Console;

namespace SOLID_Principles
{
    public class OpenClosedPrinciple
    {
        public static void Execute()
        {
            // Violating Open-Closed Principle
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            WriteLine("Green Products (old): ");
            foreach (var greenProduct in ProductFilter.FilterByColor(products, Color.Green))
            {
                WriteLine($"  - {greenProduct.Name}");
            }
            
            // ---------------------------------
            // Using the better filter
            var bf = new BetterFilter();
            var greenColorSpec = new ColorSpecification(Color.Green);
            WriteLine("Green Products (better filter): ");
            foreach (var greenProduct in bf.Filter(products, greenColorSpec))
            {
                WriteLine($"  - {greenProduct.Name}");
            }

            var largeAndGreenSpec =
                new AndSpecification(new SizeSpecification(Size.Large), new ColorSpecification(Color.Green));
            WriteLine("Large Green Products (better filter): ");
            foreach (var largeGreenProduct in bf.Filter(products, largeAndGreenSpec))
            {
                WriteLine($"  - {largeGreenProduct.Name}");
            }
        }
    }

    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Huge
    }

    public class Product
    {
        public readonly string Name;
        public readonly Color Color;
        public readonly Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name;
            Color = color;
            Size = size;
        }
    }
    
    // We need to filter the products
    public static class ProductFilter
    {
        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var product in products)
            {
                if (product.Size == size)
                    yield return product;
            }
        }
        
        public static IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var product in products)
            {
                if (product.Color == color)
                    yield return product;
            }
        }
        
        // If we want to filter by both Size and Color
        // we would have to create another method
        // THIS IS OCP Violation
        public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var product in products)
            {
                if (product.Size == size && product.Color == color)
                    yield return product;
            }
        }
        
        // To avoid this we would have to use INTERFACES
    }

    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private readonly Color _color;

        public ColorSpecification(Color color)
        {
            _color = color;
        }
        
        public bool IsSatisfied(Product t)
        {
            return t.Color == _color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private readonly Size _size;

        public SizeSpecification(Size size)
        {
            _size = size;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Size == _size;
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                if (spec.IsSatisfied(item))
                    yield return item;
            }
        }
    }
    
    // Better filter by both Size and COlor
    // We need to create a new Filter
    public class AndSpecification : ISpecification<Product>
    {
        private readonly ISpecification<Product> _first, _second;

        public AndSpecification(ISpecification<Product> first, ISpecification<Product> second)
        {
            _first = first;
            _second = second;
        }
        public bool IsSatisfied(Product t)
        {
            return _first.IsSatisfied(t) && _second.IsSatisfied(t);
        }
    }
}