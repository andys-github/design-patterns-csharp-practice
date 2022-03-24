using static System.Console;

namespace SOLID_Principles
{
    public class InterfaceSegregationPrinciple
    {
        public static void Execute()
        {
            WriteLine("\n *** Interface Segregation Principle \n");
        }
    }

    public class Document
    {
        
    }

    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class MultiFunctionPrinter : IMachine
    {
        public void Print(Document d)
        {
            // Print
        }

        public void Scan(Document d)
        {
            // Scan
        }

        public void Fax(Document d)
        {
            // Fax
        }
    }

    // An Old-fashioned Printer may not
    // have multi-function capabilities
    public class OldFashionPrinter : IMachine
    {
        public void Print(Document d)
        {
            // Print
        }

        // This is where we need Interface-Segregation
        // instead of having one giant interface, we should
        // have lots of smaller interfaces,
        // preferably with single capability
        public void Scan(Document d)
        {
            throw new System.NotImplementedException();
        }

        public void Fax(Document d)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }
    
    public class PhotoCopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            // Print
        }

        public void Scan(Document d)
        {
            // Scan
        }
    }

    public interface IMultiFunctionMachine : IPrinter, IScanner { }
    
    public class MultiFunctionMachine : IMultiFunctionMachine
    {
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;

        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            _printer = printer;
            _scanner = scanner;
        }

        public void Print(Document d)
        {
            // Use print from the interface, or use the 
            // built-in print functionality
            _printer.Print(d);
        }

        public void Scan(Document d)
        {
            // Scan using the external device (interface)
            // or use the built-in scanner
            _scanner.Scan(d);
        }
    }
}