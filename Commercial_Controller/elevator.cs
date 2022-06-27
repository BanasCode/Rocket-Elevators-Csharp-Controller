using System.Threading;
using System.Collections.Generic;


namespace Commercial_Controller
{
    public class Elevator
    {
        public int ID;
        public string status;
        public int amountOfFloors;
        public int currentFloor;
        public Door door;
        public List<int> floorRequestsList;
        public string direction;
        public bool overweight;
        public List<int> completedRequestsList;

        public Elevator(int _id, string _status, int _amountOfFloors, int _currentFloor)
        {
            ID = _id;
            status = _status;
            amountOfFloors = _amountOfFloors;
            currentFloor = _currentFloor;
            door = new Door(1, "closed");
            floorRequestsList = new List<int>();
            direction = null;
            overweight = false;
            completedRequestsList = new List<int>();

        }
        public void move()
        {
            while(floorRequestsList.Count != 0){
                int destination = floorRequestsList[0];
                status = "moving";
                if (currentFloor < destination){
                    direction = "up";
                    sortFloorList();
                    while(currentFloor < destination){
                        currentFloor++;
                        int screenDisplay = currentFloor;
                    }
                }
                else if(currentFloor > destination){
                    direction = "down";
                    sortFloorList();
                    while(currentFloor > destination){
                        currentFloor--;
                        int screenDisplay = currentFloor;
                    }
                }
                status = "stopped";
                operateDoors();
                completedRequestsList.Add(floorRequestsList[0]);
                floorRequestsList.RemoveAt(0);
            }
            status = "idle";
        }

        public void sortFloorList()
        {
            if(direction == "up"){
                floorRequestsList.Sort((a, b) => a.CompareTo(b)); 
            }
            else{
                floorRequestsList.Sort((a, b) => b.CompareTo(a)); 
            };
        }

        public void operateDoors()
        {
            door.status = "opened";
            // Thread.Sleep(5000);
            // if(elevator.overweight = false){
            //     door.status = "closing";
            //     if("no obstruction"){
            //         door.status = "closed";
            //     }
            //     else{
            //         operateDoors();
            //     }
            // }
            // else{
            //     while(elevator.overweight = true){
            //         overweight.alarm = true;
            //     }
            //     operateDoors();
            // }
        }

        public void addNewRequest(int _requestedFloor)
        {
            if(!floorRequestsList.Contains(_requestedFloor))
            {
                floorRequestsList.Add(_requestedFloor);
            }

            if(currentFloor < _requestedFloor){
                direction = "up";
            }
            if(currentFloor > _requestedFloor){
                direction = "down";
            }
        }
        















    }
}