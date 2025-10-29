using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStack.BDDfy.Factories;

public interface IFluentScannerFactory
{
    IFluentScanner Create<TScenario>(TScenario testObject) where TScenario : class;
}
