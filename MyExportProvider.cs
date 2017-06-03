using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;

namespace Sample02
{
    public class MyExportProvider : ExportProvider
    {
        private List<Export> Exports { get; set; }
        private string TypeName { get; set; }

        public MyExportProvider(string contractName, string typeName)
        {
            Func<object> funcCreatePart = new Func<object>(CreatePart);
            this.TypeName = typeName;

            this.Exports = new List<Export>();
            var metadata = new Dictionary<string, object>();
            metadata.Add(CompositionConstants.ExportTypeIdentityMetadataName, contractName);

            var exportDefinition = new ExportDefinition(contractName, metadata);
            var export = new Export(exportDefinition, funcCreatePart);
            this.Exports.Add(export);
        }

        public object CreatePart()
        {
            Type partType = Assembly.GetExecutingAssembly().GetType(this.TypeName);
            object instance = Activator.CreateInstance(partType); 
            return instance;
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            return this.Exports.Where(x => definition.IsConstraintSatisfiedBy(x.Definition));
        }
    }
}
