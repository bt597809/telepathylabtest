using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelepathyLabsTest
{
    class Program
    {
        const string EXIT = "exit";
        static ICommand module;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter input: 1 or 2");
                Console.WriteLine("1 -> Boutique Hotel");
                Console.WriteLine("2 -> Math operations");
                Console.WriteLine("Input exit to close the application");

                var command = Console.ReadLine().ToLower();
                if (!string.IsNullOrWhiteSpace(command))
                {
                    if (command == EXIT)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        if (command == "1")
                        {
                            Console.WriteLine("Commands list:");
                            Console.WriteLine(" list (lists all available rooms)");
                            Console.WriteLine(" book (books the nearest available room from the entrance)");
                            Console.WriteLine(" checkout (checks out the room)");
                            Console.WriteLine(" clean (cleans the room)");
                            Console.WriteLine(" repair (sets the room to repair mode)");
                            Console.WriteLine(" fix (repairs and sets the room to vacant)");
                            Console.WriteLine(" exit (quits the application)");
                            module = Hotel.Instance;
                        }
                        else if (command == "2")
                        {
                            Console.WriteLine("Commands list");
                            Console.WriteLine(" run (1*2)+3");
                            module = MathExpression.Instance;
                        }

                        if (module != null)
                        {
                            Prompt();
                        }
                        else
                        {
                            Console.WriteLine("Invalid Command");
                            Prompt();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error, please try again");
                Console.ReadLine();
            }
        }

        static void Prompt()
        {
            Console.WriteLine("Enter input:");
            var command = Console.ReadLine().ToLower();
            module.ProcessCommand(command);
            Prompt();
        }
    }

    //class MathExpression : ICommand
    //{
    //    public static MathExpression Instance { get; } = new MathExpression();

    //    private MathExpression()
    //    {
    //    }
    //    public void ProcessCommand(string command)
    //    {
    //        if (command == GlobalConstants.EXIT)
    //        {
    //            Environment.Exit(0);
    //        }
    //        else
    //        {

    //        }
    //    }
    //}

    class Hotel : ICommand
    {
        private string _roomsSample = "1a,1b,1c,1d,1e,2e,2d,2c,2b,2a,3a,3b,3c,3d,3e,4e,4d,4c,4b,4a";
        public Dictionary<string, Status> Rooms { get; set; }
        public static Hotel Instance { get; } = new Hotel();

        private Hotel()
        {
            Rooms = new Dictionary<string, Status>();
            foreach (var room in _roomsSample.Split(','))
            {
                Rooms.Add(room, Status.Available);
            }
        }

        void Display()
        {
            var availableRooms = Rooms.Where(p => p.Value == Status.Available).Select(p => p.Key);
            Console.WriteLine(string.Join(", ", availableRooms));
        }

        void Book()
        {
            KeyValuePair<string, Status> room;
            string bookedRoom = null;
            for (int i = 0; i < Rooms.Count; i++)
            {

                room = Rooms.ElementAt(i);
                if (room.Value == Status.Available)
                {
                    Rooms[room.Key] = Status.Occupied;
                    bookedRoom = room.Key;
                    break;
                }
            }

            if (bookedRoom == null)
            {
                Console.WriteLine("No rooms available now.");
            }
            else
            {
                Console.WriteLine("Your booked room - " + bookedRoom);
            }

        }

        void Checkout(string room)
        {
            this._setStatus(room, Status.Occupied, Status.Vacant, "checkout");
        }

        void Clean(string room)
        {
            this._setStatus(room, Status.Vacant, Status.Available, "cleaned");
        }

        void Repair(string room)
        {
            this._setStatus(room, Status.Vacant, Status.Repair, "repaired");
        }

        void Fix(string room)
        {
            this._setStatus(room, Status.Repair, Status.Vacant, "fixed");
        }

        void _setStatus(string room, Status fromStatus, Status toStatus, string activity)
        {
            if (Rooms.ContainsKey(room))
            {
                var status = Rooms[room];
                if (status == fromStatus)
                {
                    Rooms[room] = toStatus;
                    Console.WriteLine(string.Format("room {0} has been {1} successfully", room, activity));
                }
                else
                {
                    Console.WriteLine(string.Format("room {0} could not {1}, Please contact help desk.", room, activity));
                }
            }
            else
            {
                Console.WriteLine("Invalid room number.");
            }
        }
        public void ProcessCommand(string command)
        {
            if (command == GlobalConstants.EXIT)
            {
                Environment.Exit(0);
            }
            else
            {
                var hotel = Hotel.Instance;
                var inputs = command.Split(' ');
                var cmd = inputs[0];
                switch (cmd)
                {
                    case "list":
                        hotel.Display();
                        break;
                    case "book":
                        hotel.Book();
                        break;
                    case "checkout":
                        hotel.Checkout(inputs[1]);
                        break;
                    case "clean":
                        hotel.Clean(inputs[1]);
                        break;
                    case "repair":
                        hotel.Repair(inputs[1]);
                        break;
                    case "fix":
                        hotel.Fix(inputs[1]);
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }

    enum Status
    {
        Available = 1,
        Occupied = 2,
        Vacant = 3,
        Repair = 4
    }

    class GlobalConstants
    {
        public const string EXIT = "exit";
    }

    interface ICommand
    {
        void ProcessCommand(string command);
    }
}
