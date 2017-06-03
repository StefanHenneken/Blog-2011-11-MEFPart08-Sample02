using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Reflection;

namespace Sample02
{     
    public class Program
    {
        [ImportMany]
        private ICarContract[] CarParts { get; set; }
      
        static void Main(string[] args)
        {
            new Program().Run();
        }

        void Run()
        {
            CompositionContainer container = null;

            try
            {
                var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
                var provider = new MyExportProvider(typeof(ICarContract).FullName, typeof(BMW).FullName);
                container = new CompositionContainer(catalog, provider);
                container.ComposeParts(this);

                foreach (ICarContract carPart in CarParts)
                    Console.WriteLine(carPart.GetName());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            container.Dispose();
        }
    }

    public interface ICarContract
    {
        string GetName();
    }

    [Export(typeof(ICarContract))]
    public class Mercedes : ICarContract
    {
        public string GetName()
        {
            return "Mercedes";
        }
    }
    
    public class BMW : ICarContract
    {
        public string GetName()
        {
            return "BMW";
        }
    }
}
