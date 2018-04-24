using Microsoft.Extensions.Configuration;
using Modbus.Data;
using Modbus.Device;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Advantech.Wise4012e.Modbus.SimulationApp
{
    internal class Program
    {
        public static IConfiguration Configuration { get; set; }

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("settings.json");

            Configuration = builder.Build();

            var address = Configuration["address"];

            Console.WriteLine($"Start Advantech Wise4012e Modbus simulation at {address}");

            var slaveTcpListener = new TcpListener(IPAddress.Parse(address), 502);

            slaveTcpListener.Start();

            byte SlaveID = 1;

            var slave = ModbusTcpSlave.CreateTcp(SlaveID, slaveTcpListener);
            slave.DataStore = DataStoreFactory.CreateDefaultDataStore();

            slave.DataStore.DataStoreReadFrom += DataStore_DataStoreReadFrom;
            slave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;

            slave.DataStore.HoldingRegisters[1] = (ushort)0;
            slave.DataStore.HoldingRegisters[2] = (ushort)0;

            slave.DataStore.CoilDiscretes[1] = false;
            slave.DataStore.CoilDiscretes[2] = false;
            slave.DataStore.CoilDiscretes[17] = false;
            slave.DataStore.CoilDiscretes[18] = false;

            slave.Listen();

            Console.WriteLine($"Slave {SlaveID} supports switch 1, switch 2, relay 17, relay 18, knob 40001, knob 40002");

            while (true)
            {
                Console.WriteLine($"> {slave.DataStore.CoilDiscretes[1]} {slave.DataStore.CoilDiscretes[2]} {slave.DataStore.CoilDiscretes[17]} {slave.DataStore.CoilDiscretes[18]} {slave.DataStore.HoldingRegisters[1]:0000} {slave.DataStore.HoldingRegisters[2]:0000} - Write '1 [bit]' '2 [bit]' '40001 [int]' '40002 [int]' or Q to exit");
                Console.Write("> ");

                var input = Console.ReadLine();

                if (input.ToUpper() == "Q")
                {
                    break;
                }

                var fields = input.Split(' ');

                switch (fields[0])
                {
                    case "1":
                        slave.DataStore.CoilDiscretes[1] = Convert.ToBoolean(Convert.ToInt16(fields[1]));
                        break;

                    case "2":
                        slave.DataStore.CoilDiscretes[2] = Convert.ToBoolean(Convert.ToInt16(fields[1]));
                        break;

                    case "40001":
                        var value1 = Math.Abs(Convert.ToInt32(fields[1]));
                        slave.DataStore.HoldingRegisters[1] = value1 > 4500 ? (ushort)4500 : (ushort)value1;
                        break;

                    case "40002":
                        var value2 = Math.Abs(Convert.ToInt32(fields[1]));
                        slave.DataStore.HoldingRegisters[2] = value2 > 4500 ? (ushort)4500 : (ushort)value2;
                        break;
                }
            }
        }

        private static void DataStore_DataStoreReadFrom(object sender, DataStoreEventArgs e)
        {
            {
                if (e.ModbusDataType == ModbusDataType.HoldingRegister)
                {
                    Console.WriteLine($"\nClient reads {e.ModbusDataType} address {e.StartAddress + 40001} - read {e.Data.B[0]}");
                    Console.Write("> ");
                }
                else
                {
                    Console.WriteLine($"\nClient reads {e.ModbusDataType} address {e.StartAddress + 1} - read {e.Data.A[0]}");
                    Console.Write("> ");
                }
            }
        }

        private static void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs e)
        {
            Console.WriteLine($"\nClient writes {e.ModbusDataType} address {e.StartAddress + 1} - write {e.Data.A[0]}");
            Console.Write("> ");
        }
    }
}