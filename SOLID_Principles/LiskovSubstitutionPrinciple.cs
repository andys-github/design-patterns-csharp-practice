using static System.Console;

namespace SOLID_Principles
{
    public class LiskovSubstitutionPrinciple
    {
        public static void Execute()
        {
            WriteLine("\n*** Liskov-Substitution Principle ***\n");
            // 1. Create Rectangle class
            Rectangle rect = new Rectangle(10, 20);
            WriteLine($"Rectangle - {rect}, Area: {Area(rect)}");

            // 2. Create Square class by inheriting Rectangle
            Square sq = new Square();
            sq.Width = 10;
            WriteLine($"Square - {sq}, Area: {Area(sq)}");
            
            // 3. A reference to Square object can be stored
            //    in a Rectangle variable due to Inheritance
            Rectangle rect1 = new Square();
            rect1.Width = 10;
            WriteLine($"Rectangle1 - {rect1}, Area: {Area(rect1)}");
            
            // 4. Using virtual (in Parent properties) and
            //    override (in Child properties), we can correctly
            //    assign a Square (child) to a Rectangle (parent)
            NewRectangle rectNew = new NewSquare();
            rectNew.Width = 10;
            WriteLine($"NewRectangle - {rectNew}, Area: {Area(rectNew)}");
        }

        static int Area(Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }
        
        static int Area(NewRectangle newRectangle)
        {
            return newRectangle.Width * newRectangle.Height;
        }
    }

    public class Rectangle
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle() { }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        // This way of over-ridding properties
        // is already a code-smell
        public new int Width
        {
            set => base.Width = base.Height = value;
        }
        
        public new int Height
        {
            set => base.Width = base.Height = value;
        }
    }

    public class NewRectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public NewRectangle() { }

        public NewRectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }
    
    public class NewSquare : NewRectangle
    {
        public override int Width
        {
            set => base.Width = base.Height = value;
        }
        
        public override int Height
        {
            set => base.Width = base.Height = value;
        }
    }
}