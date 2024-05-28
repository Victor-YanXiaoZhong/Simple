// See https://aka.ms/new-console-template for more information
using Simple.Utils;

Console.WriteLine("Hello, World!", "123123");
console.WriteLine("Hello, World!" + "123123");

var machineInfo = MachineInfo.Instance;
try
{
    ConsoleHelper.Info(machineInfo.Json());
    throw new NotImplementedException();
}
catch (Exception ex)
{
    ConsoleHelper.Err(ex.Message);
}
ConsoleHelper.Info("Hello, World!");
ConsoleHelper.Pause();