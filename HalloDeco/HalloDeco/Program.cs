using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDeco
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** Hallo Decorator ***");

            var lecker = new Käse(new Käse(new Pizza()));
            Console.WriteLine($"{lecker.Name} {lecker.Preis}");
                    
            Console.WriteLine("Ende");
            Console.ReadKey();
        }
    }


    public interface ICompo
    {
        string Name { get; }
        decimal Preis { get; }
    }

    public abstract class Deco : ICompo
    {
        public abstract string Name { get; }
        public abstract decimal Preis { get; }

        protected ICompo parent;
        public Deco(ICompo parent) => this.parent = parent;
    }

    class Pizza : ICompo
    {
        public string Name => "Pizza";

        public decimal Preis => 6m;
    }

    class Käse : Deco
    {
        public Käse(ICompo parent) : base(parent)
        { }

        public override string Name => parent.Name + " Käse";

        public override decimal Preis => parent.Preis + 1.2m;
    }
}
