using Caliburn.Micro;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Windows;
using WPFUI.Interfaces;

namespace WPFUI
{
    public class MefBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        public MefBootstrapper()
        {
            this.Initialize();
        }

        protected override void Configure()
        {
            container = new CompositionContainer();

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;

            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
            {
                return exports.First();
            }

            throw new Exception($"Could not locate any instance of contract {contract}.");
        }

        protected override void BuildUp(object instance)
        {
            this.container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<IShell>();
        }
    }
}